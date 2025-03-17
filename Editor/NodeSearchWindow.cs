using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace PeartreeGames.TriggerGraph.Editor
{
    public class NodeSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private TriggerGraphView _view;
        private EditorWindow _editor;
        private TriggerGraph _triggerGraph;

        public void Init(EditorWindow editor, TriggerGraphView view, TriggerGraph graph)
        {
            _editor = editor;
            _view = view;
            _triggerGraph = graph;
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var tree = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("Create Node"))
            };
            var types = TypeCache.GetTypesDerivedFrom<NodeData>();
            var entries = new List<(string fullPath, Type type)>();

            foreach (var type in types)
            {
                var attribute = type.GetCustomAttribute<SearchTreeAttribute>();
                if (attribute != null)
                {
                    entries.Add((attribute.Name, type));
                }
            }


            var addedGroups = new HashSet<string>();
            var sortedEntries = entries
                .OrderBy(e => e.fullPath.Split('/')[0])
                .ThenByDescending(e => e.fullPath.Count(c => c == '/'))
                .ThenBy(e => e.fullPath)
                .ToList();

            foreach (var (fullPath, type) in sortedEntries)
            {
                var pathParts = fullPath.Split('/');
                var currentPath = "";
                var level = 1;
                for (var i = 0; i < pathParts.Length - 1; i++)
                {
                    currentPath = string.IsNullOrEmpty(currentPath)
                        ? pathParts[i]
                        : $"{currentPath}/{pathParts[i]}";

                    if (!addedGroups.Contains(currentPath))
                    {
                        tree.Add(new SearchTreeGroupEntry(new GUIContent(pathParts[i]), level));
                        addedGroups.Add(currentPath);
                    }

                    level++;
                }

                tree.Add(new SearchTreeEntry(new GUIContent(pathParts[^1]))
                {
                    level = level,
                    userData = type
                });
            }

            return tree;
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            var node = (NodeData)CreateInstance(((Type)searchTreeEntry.userData).FullName);
            node.ID = Guid.NewGuid();

            var worldMousePosition = _editor.rootVisualElement.ChangeCoordinatesTo(
                _editor.rootVisualElement.parent,
                context.screenMousePosition - _editor.position.position);
            var localMousePosition = _view.contentViewContainer.WorldToLocal(worldMousePosition);

            node.nodePosition = localMousePosition;
            _triggerGraph.nodes ??= new List<NodeData>();
            _view.CreateNode(node);
            _triggerGraph.nodes.Add(node);
            return true;
        }
    }
}