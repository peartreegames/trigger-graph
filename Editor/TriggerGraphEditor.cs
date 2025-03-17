using UnityEditor;
using UnityEngine.UIElements;

namespace PeartreeGames.TriggerGraph.Editor
{
    [CustomEditor(typeof(TriggerGraph))]
    public class TriggerGraphEditor : UnityEditor.Editor
    {

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            root.Add(new IMGUIContainer(() => DrawDefaultInspector()));
            root.Add(new Button(() => TriggerGraphEditorWindow.Show(target as TriggerGraph)){ text = "Edit Graph" });
            return root;
        }
    }
}