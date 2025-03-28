﻿using System;
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
        private Edge[] Edges => edges.ToArray();
        private TriggerGraphNode[] Nodes => nodes.Cast<TriggerGraphNode>().ToArray();

        public TriggerGraphView(EditorWindow editorWindow, TriggerGraph triggerGraph)
        {
            _editor = editorWindow;
            styleSheets.Add(Resources.Load<StyleSheet>("TriggerGraph"));
            _triggerGraph = triggerGraph;
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ContentZoomer());
            graphViewChanged = OnGraphChanged;
            var grid = new GridBackground();
            Insert(0, grid);
            var searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
            searchWindow.Init(_editor, this, triggerGraph);
            nodeCreationRequest = ctx =>
                SearchWindow.Open(new SearchWindowContext(ctx.screenMousePosition), searchWindow);
            LoadGraph();
            serializeGraphElements = CopyData;
            canPasteSerializedData = CanPasteData;
            unserializeAndPaste = PasteData;
        }

        [Serializable]
        private class CopyPasteWrapper
        {
            public List<NodeData> Nodes;
        }

        private void PasteData(string operationname, string data)
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            };
            var wrapper = JsonConvert.DeserializeObject<CopyPasteWrapper>(data, settings);
            foreach (var item in wrapper.Nodes)
            {
                item.ID = Guid.NewGuid();
                _triggerGraph.nodes.Add(item);
                var node = TriggerGraphNode.Create(_triggerGraph, item);
                var rect = new Rect(item.nodePosition + new Vector2(50, 50), TriggerGraphNode.DefaultSize);
                node.SetPosition(rect);
                AddElement(node);
            }
            
        }

        private static string CopyData(IEnumerable<GraphElement> elements)
        {
            var list = new List<NodeData>();
            foreach (var element in elements)
            {
                if (element is TriggerGraphNode node)
                {
                    list.Add(node.userData as NodeData);
                }
            }
            
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Objects
            };
            var json =  JsonConvert.SerializeObject(new CopyPasteWrapper { Nodes = list }, settings);
            return json;
        }

        private static bool CanPasteData(string data) => !string.IsNullOrEmpty(data) && data != "{}";


        private GraphViewChange OnGraphChanged(GraphViewChange graphViewChange)
        {
            if (_triggerGraph == null) return graphViewChange;
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

            EditorUtility.SetDirty(_triggerGraph);
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
            var node = TriggerGraphNode.Create(_triggerGraph, data);
            AddElement(node);
            return node;
        }
    }
}