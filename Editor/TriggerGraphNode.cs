using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using SerializedObject = UnityEditor.SerializedObject;

namespace PeartreeGames.TriggerGraph.Editor
{
    public class TriggerGraphNode : Node
    {
        public Guid ID;

        public static readonly Vector2 DefaultSize = new(100, 200);

        private static readonly string[] IgnoredFields =
            { "m_Script", "nodeIdString", "nodePosition" };


        public static TriggerGraphNode Create(TriggerGraph graph, NodeData data)
        {
            var node = new TriggerGraphNode
            {
                ID = data.ID,
                title = data.GetType().Name,
                userData = data,
            };
            
            var box = CreatePropertyBox(graph, data);
            node.extensionContainer.Add(box);

            var properties = data.GetType().GetProperties(BindingFlags.Static |
                                                          BindingFlags.Public |
                                                          BindingFlags.FlattenHierarchy);
            foreach (var property in properties)
            {
                if (Attribute.IsDefined(property, typeof(InputAttribute)))
                {
                    if (property.PropertyType != typeof(string))
                        throw new CustomAttributeFormatException(
                            "Input can only be added to Strings");
                    var input =
                        CreatePort(node, Direction.Input, Port.Capacity.Multi);
                    input.portName = property.GetValue(data) as string;
                    node.inputContainer.Add(input);
                }
                else if (Attribute.IsDefined(property, typeof(OutputAttribute)))
                {
                    if (property.PropertyType != typeof(string))
                        throw new CustomAttributeFormatException(
                            "Output can only be added to Strings");
                    var attr = property.GetCustomAttribute<OutputAttribute>();

                    var output =
                        CreatePort(node, Direction.Output, Port.Capacity.Multi,
                            attr.Orientation == PortOrientation.Horizontal
                                ? Orientation.Horizontal
                                : Orientation.Vertical);
                    output.portName = property.GetValue(data) as string;
                    output.portColor = attr.Color.AsColor();

                    if (attr.Orientation == PortOrientation.Vertical)
                    {
                        output.style.alignSelf = Align.Center;
                        node.extensionContainer.Add(output);
                    }
                    else node.outputContainer.Add(output);
                }
            }

            if (data is TriggerNode)
            {
                node.Q<VisualElement>("node-border").style.borderLeftWidth = 5;
                node.Q<VisualElement>("node-border").style.borderLeftColor = Color.cyan;
            }

            node.RefreshExpandedState();
            node.RefreshPorts();
            node.SetPosition(new Rect(data.nodePosition, DefaultSize));
            return node;
        }

        private static Port CreatePort(TriggerGraphNode node, Direction direction,
            Port.Capacity capacity, Orientation orientation = Orientation.Horizontal) =>
            node.InstantiatePort(orientation, direction, capacity, typeof(float));

        private static VisualElement CreatePropertyBox(TriggerGraph graph, NodeData data)
        {
            var serializedGraph = new SerializedObject(graph);
            var foldOut = new Foldout();
            foldOut.contentContainer.AddToClassList("property-foldout");
            var arr = serializedGraph.FindProperty("nodes");
            var idx = graph.nodes.FindIndex(n => n.ID == data.ID);
            if (idx < 0)
            {
                Debug.LogError($"Could not find NodeData {data.ID} in array");
                return new VisualElement();
            } 
            var node = arr.GetArrayElementAtIndex(idx);
            var fields = data.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.Public |
                           BindingFlags.NonPublic);
            if (fields.Length == 0) return new VisualElement();
            foreach (var field in fields)
            {
                if (IgnoredFields.Contains(field.Name)) continue;
                var prop = node.FindPropertyRelative(field.Name);
                var propField = new PropertyField(prop);
                propField.Bind(serializedGraph);
                foldOut.contentContainer.Add(propField);
            }

            if (fields.Length == 1) return foldOut.contentContainer.Children().First();
            var expand = node.FindPropertyRelative("isFoldoutExpanded");
            foldOut.value = expand.boolValue;
            foldOut.BindProperty(expand);
            foldOut.value = true;
            return foldOut;
        }
    }
}