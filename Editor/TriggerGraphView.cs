using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace PeartreeGames.TriggerGraph.Editor
{
    public class TriggerGraphView : GraphView
    {
        private readonly TriggerGraph _triggerGraph;
        private readonly EditorWindow _editor;
        public readonly NodeSearchWindow SearchWindow;
        private Vector2 _lastMousePosition;
        private float _lastViewTransformSaveTime;
        private const float ViewTransformSaveDelay = 0.25f;
        private Edge[] Edges => edges.ToArray();
        private TriggerGraphNode[] Nodes => nodes.Cast<TriggerGraphNode>().ToArray();

        public TriggerGraphView(EditorWindow editorWindow, TriggerGraph triggerGraph)
        {
            styleSheets.Add(Resources.Load<StyleSheet>("TriggerGraph"));
            _triggerGraph = triggerGraph;
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ContentZoomer());
            graphViewChanged = OnGraphChanged;
            var grid = new GridBackground();
            Insert(0, grid);
            SearchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
            SearchWindow.Init(editorWindow, this, triggerGraph);
            nodeCreationRequest = ctx =>
                UnityEditor.Experimental.GraphView.SearchWindow.Open(
                    new SearchWindowContext(ctx.screenMousePosition), SearchWindow);
            LoadGraph();
            RestoreViewTransform();

            serializeGraphElements = CopyData;
            canPasteSerializedData = CanPasteData;
            unserializeAndPaste = PasteData;

            RegisterCallback<MouseMoveEvent>(OnMouseMove);
            RegisterCallback<MouseDownEvent>(OnMouseDown);

            RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
            RegisterCallback<KeyDownEvent>(OnKeyDown);

            Undo.undoRedoPerformed += OnUndoRedoPerformed;
        }

        ~TriggerGraphView()
        {
            Undo.undoRedoPerformed -= OnUndoRedoPerformed;
        }

        private static void OnKeyDown(KeyDownEvent evt)
        {
            if (!evt.ctrlKey && !evt.commandKey) return;
            switch (evt.keyCode)
            {
                case KeyCode.Z:
                    if (evt.shiftKey)
                        Undo.PerformRedo();
                    else
                        Undo.PerformUndo();
                    evt.StopPropagation();
                    break;
                case KeyCode.Y:
                    Undo.PerformRedo();
                    evt.StopPropagation();
                    break;
            }
        }

        private void OnUndoRedoPerformed() => LoadGraph();

        private void OnGeometryChanged(GeometryChangedEvent evt)
        {
            SaveViewTransform();
        }


        private void SaveViewTransform()
        {
            if (_triggerGraph == null) return;
            if (Time.realtimeSinceStartup - _lastViewTransformSaveTime < ViewTransformSaveDelay)
            {
                _triggerGraph.viewPosition = viewTransform.position;
                _triggerGraph.viewScale = viewTransform.scale;
                EditorUtility.SetDirty(_triggerGraph);
            }
            _lastViewTransformSaveTime = Time.realtimeSinceStartup;
        }


        private void RestoreViewTransform()
        {
            if (_triggerGraph == null) return;
            UpdateViewTransform(_triggerGraph.viewPosition, _triggerGraph.viewScale);
        }

        private void OnMouseDown(MouseDownEvent evt) => _lastMousePosition = evt.mousePosition;
        private void OnMouseMove(MouseMoveEvent evt) => _lastMousePosition = evt.mousePosition;

        [Serializable]
        private class CopyPasteWrapper
        {
            public List<NodeData> Nodes;
            public Vector2 centerPosition;
        }

        private static string CopyData(IEnumerable<GraphElement> elements)
        {
            var list = new List<NodeData>();
            var positions = new List<Vector2>();
            foreach (var element in elements)
            {
                if (element is TriggerGraphNode node)
                {
                    if (node.userData is not NodeData nodeData) continue;
                    var settings = new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        TypeNameHandling = TypeNameHandling.Objects,
                        TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
                        ContractResolver = new UnitySerializeFieldContractResolver(),
                        Converters = new List<JsonConverter> { new UnityObjectJsonConverter() }
                    };
                    var json = JsonConvert.SerializeObject(nodeData, settings);
                    var copiedNodeData = JsonConvert.DeserializeObject<NodeData>(json, settings);

                    list.Add(copiedNodeData);
                    positions.Add(nodeData.nodePosition);
                }
            }

            var centerPosition = Vector2.zero;
            if (positions.Count > 0)
            {
                centerPosition =
                    positions.Aggregate(centerPosition, (current, pos) => current + pos);
                centerPosition /= positions.Count;
            }

            var wrapper = new CopyPasteWrapper
            {
                Nodes = list,
                centerPosition = centerPosition
            };

            var wrapperSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
                ContractResolver = new UnitySerializeFieldContractResolver(),
                Converters = new List<JsonConverter> { new UnityObjectJsonConverter() }
            };
            var finalJson = JsonConvert.SerializeObject(wrapper, wrapperSettings);
            return finalJson;
        }

        private void PasteData(string operationname, string data)
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
                ContractResolver = new UnitySerializeFieldContractResolver(),
                Converters = new List<JsonConverter> { new UnityObjectJsonConverter() }
            };
            var wrapper = JsonConvert.DeserializeObject<CopyPasteWrapper>(data, settings);
            Undo.RecordObject(_triggerGraph, "Paste Nodes");

            var worldMousePosition = contentViewContainer.parent.ChangeCoordinatesTo(
                contentViewContainer.parent.parent,
                _lastMousePosition);
            var localMousePosition = contentViewContainer.WorldToLocal(worldMousePosition);
            var offset = localMousePosition - wrapper.centerPosition;

            var pastedNodeIds = new List<Guid>();

            foreach (var item in wrapper.Nodes)
            {
                item.ID = Guid.NewGuid();
                item.nodePosition += offset;
                _triggerGraph.nodes.Add(item);
                pastedNodeIds.Add(item.ID);
            }

            EditorUtility.SetDirty(_triggerGraph);

            var pastedNodes = new List<TriggerGraphNode>();
            foreach (var nodeId in pastedNodeIds)
            {
                var nodeData = _triggerGraph.nodes.FirstOrDefault(n => n.ID == nodeId);
                if (nodeData != null)
                {
                    var node = TriggerGraphNode.Create(this, _triggerGraph, nodeData);
                    var rect = new Rect(nodeData.nodePosition, TriggerGraphNode.DefaultSize);
                    node.SetPosition(rect);
                    AddElement(node);
                    pastedNodes.Add(node);
                }
            }

            ClearSelection();
            foreach (var node in pastedNodes) AddToSelection(node);
        }

        private static bool CanPasteData(string data) =>
            !string.IsNullOrEmpty(data) && data != "{}";


        private GraphViewChange OnGraphChanged(GraphViewChange graphViewChange)
        {
            if (_triggerGraph == null) return graphViewChange;

            var hasStructuralChanges = graphViewChange.edgesToCreate != null ||
                                       graphViewChange.elementsToRemove != null ||
                                       graphViewChange.movedElements != null;

            if (hasStructuralChanges) Undo.RecordObject(_triggerGraph, "Graph Changed");
            if (graphViewChange.edgesToCreate != null)
            {
                foreach (var edge in graphViewChange.edgesToCreate)
                {
                    var outputNode = (TriggerGraphNode)edge.output.node;
                    var inputNode = (TriggerGraphNode)edge.input.node;
                    var outputPortName = edge.output.portName;
                    var inputPortName = edge.input.portName;
                    if (_triggerGraph.edges.Exists(e =>
                            e.OutputId == outputNode.ID && e.InputId == inputNode.ID &&
                            e.outputPortName == outputPortName &&
                            e.inputPortName == inputPortName)) continue;
                    _triggerGraph.edges.Add(new EdgeData
                    {
                        OutputId = outputNode.ID,
                        outputPortName = outputPortName,
                        InputId = inputNode.ID,
                        inputPortName = inputPortName
                    });
                }
            }

            if (graphViewChange.elementsToRemove != null)
            {
                foreach (var elem in graphViewChange.elementsToRemove)
                {
                    if (elem.GetType() == typeof(TriggerGraphNode))
                    {
                        var id = ((TriggerGraphNode)elem).ID;
                        _triggerGraph.nodes.RemoveAll(n => n.ID == id);
                        _triggerGraph.edges.RemoveAll(e => e.OutputId == id || e.InputId == id);
                    }

                    if (elem.GetType() != typeof(Edge)) continue;

                    var outputNode = (TriggerGraphNode)((Edge)elem).output.node;
                    var inputNode = (TriggerGraphNode)((Edge)elem).input.node;
                    var outputPortName = ((Edge)elem).output.portName;
                    var inputPortName = ((Edge)elem).input.portName;
                    _triggerGraph.edges.RemoveAll(e =>
                        e.OutputId == outputNode.ID && e.InputId == inputNode.ID &&
                        e.outputPortName == outputPortName && e.inputPortName == inputPortName);
                }
            }

            if (graphViewChange.movedElements != null)
            {
                foreach (var elem in graphViewChange.movedElements)
                {
                    if (elem.GetType() != typeof(TriggerGraphNode)) continue;
                    var node = (TriggerGraphNode)elem;
                    if (node.ID == Guid.Empty) continue;
                    var referenceNode = _triggerGraph.nodes.FirstOrDefault(n => n.ID == node.ID);
                    if (referenceNode == null) continue;
                    referenceNode.nodePosition = node.GetPosition().position;
                }
            }

            if (hasStructuralChanges) EditorUtility.SetDirty(_triggerGraph);
            return graphViewChange;
        }

        private void LoadGraph()
        {
            if (_triggerGraph == null) return;
            ClearGraph();
            RecreateGraph();
        }

        private void RecreateGraph()
        {
            if (_triggerGraph == null || _triggerGraph.nodes == null ||
                _triggerGraph.edges == null) return;
            foreach (var nodeData in _triggerGraph.nodes)
            {
                var node = CreateNode(nodeData);
                if (node == null) continue;
                AddElement(node);
                node.SetPosition(new Rect(nodeData.nodePosition, TriggerGraphNode.DefaultSize));
            }

            var cachedNodes = Nodes;
            foreach (var node in cachedNodes)
            {
                for (var i = 0; i < node.outputContainer.childCount; i++)
                {
                    var port = node.outputContainer[i].Q<Port>();
                    var edgeData = _triggerGraph.edges
                        .Where(e => e.OutputId == node.ID && e.outputPortName == port.portName)
                        .ToArray();
                    foreach (var edge in edgeData)
                    {
                        var targetId = edge.InputId;
                        var targetNode = cachedNodes.FirstOrDefault(n => n.ID == targetId);
                        if (targetNode == null) continue;
                        var nodeData =
                            _triggerGraph.nodes.FirstOrDefault(n => n.ID == targetNode.ID);
                        if (nodeData == null) continue;
                        if (targetNode.inputContainer.childCount == 0) continue;

                        var inputPort = targetNode.inputContainer.Children()
                            .OfType<Port>()
                            .FirstOrDefault(p => p.portName == edge.inputPortName);
                        if (inputPort != null) LinkNodes(port, inputPort);
                    }
                }
            }
        }

        private void ClearGraph()
        {
            foreach (var edge in Edges) RemoveElement(edge);
            foreach (var node in Nodes) RemoveElement(node);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter) =>
            ports.Where(port => startPort != port && startPort.node != port.node).ToList();

        private void LinkNodes(Port output, Port input)
        {
            var edge = new Edge
            {
                output = output,
                input = input
            };
            edge.input.Connect(edge);
            edge.output.Connect(edge);
            Add(edge);
        }

        public TriggerGraphNode CreateNode(NodeData data)
        {
            if (data == null) return null;
            Undo.RecordObject(_triggerGraph, "Create Node");
            var node = TriggerGraphNode.Create(this, _triggerGraph, data);
            AddElement(node);
            return node;
        }

        public void AutoConnectToCreatedNode(TriggerGraphNode createdNode, Port sourcePort)
        {
            if (createdNode == null || sourcePort == null) return;

            Port targetPort = null;

            if (sourcePort.direction == Direction.Output)
            {
                targetPort = createdNode.inputContainer.Children()
                    .OfType<Port>()
                    .FirstOrDefault();
            }
            else if (sourcePort.direction == Direction.Input)
            {
                targetPort = createdNode.outputContainer.Children()
                    .OfType<Port>()
                    .FirstOrDefault();
            }

            if (targetPort != null)
            {
                var edge = new Edge
                {
                    output = sourcePort.direction == Direction.Output ? sourcePort : targetPort,
                    input = sourcePort.direction == Direction.Input ? sourcePort : targetPort
                };

                edge.input.Connect(edge);
                edge.output.Connect(edge);
                Add(edge);

                var outputNode = (TriggerGraphNode)edge.output.node;
                var inputNode = (TriggerGraphNode)edge.input.node;

                if (!_triggerGraph.edges.Exists(e =>
                        e.OutputId == outputNode.ID && e.InputId == inputNode.ID &&
                        e.outputPortName == edge.output.portName &&
                        e.inputPortName == edge.input.portName))
                {
                    _triggerGraph.edges.Add(new EdgeData
                    {
                        OutputId = outputNode.ID,
                        outputPortName = edge.output.portName,
                        InputId = inputNode.ID,
                        inputPortName = edge.input.portName
                    });
                }

                EditorUtility.SetDirty(_triggerGraph);
            }
        }
    }
}