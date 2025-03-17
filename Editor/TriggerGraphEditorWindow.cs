using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace PeartreeGames.TriggerGraph.Editor
{
    public class TriggerGraphEditorWindow : EditorWindow
    {
        private GlobalObjectId _id;
        private TriggerGraph _graph;
        private Vector2 _graphPosition;
        private float _graphZoom;
        
        public static void Show(TriggerGraph graph)
        {
            var window = GetWindow<TriggerGraphEditorWindow>();
            window.titleContent = new GUIContent(graph.name);
            window._graph = graph;
            window._id = GlobalObjectId.GetGlobalObjectIdSlow(graph);
            window.Show();
        }

        private void Init()
        {
            rootVisualElement.Clear();
            var box = new Box { style = { alignItems = Align.Center } };
            box.StretchToParentSize();
            var label = new Label { style = { top = 50 } };
            if (_graph == null) _graph = GlobalObjectId.GlobalObjectIdentifierToObjectSlow(_id) as TriggerGraph;
            if (_graph == null) label.text = "TriggerGraph Missing";
            rootVisualElement.Clear();
            if (label.text != string.Empty)
            {
                box.Add(label);
                rootVisualElement.Add(box);
                return;
            }

            var view = new TriggerGraphView(this, _graph)
            {
                name = "Graph"
            };
            view.StretchToParentSize();
            rootVisualElement.Add(view);
        }

        private void OnEnable()
        {
            if (_graph == null) _graph = GlobalObjectId.GlobalObjectIdentifierToObjectSlow(_id) as TriggerGraph;
            if (_graph != null) Init();
            else
            {
                EditorApplication.update -= CheckForUserData;
                EditorApplication.update += CheckForUserData;
            }

            EditorApplication.playModeStateChanged += PlayModeChanged;
        }

        private void PlayModeChanged(PlayModeStateChange obj)
        {
            switch(obj)
            {
                case PlayModeStateChange.EnteredEditMode:
                case PlayModeStateChange.EnteredPlayMode:
                    Init();
                    break;
            }

        }

        private void CheckForUserData()
        {
            if (_graph == null) _graph = GlobalObjectId.GlobalObjectIdentifierToObjectSlow(_id) as TriggerGraph;
            if (_graph == null) return;
            EditorApplication.update -= CheckForUserData;
            Init();
        }

        private void OnDisable()
        {
            rootVisualElement.Clear();
            EditorApplication.update -= CheckForUserData;
            EditorApplication.playModeStateChanged -= PlayModeChanged;
        }
    }
}
