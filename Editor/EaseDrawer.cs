using PeartreeGames.TriggerGraph.Utils;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace PeartreeGames.TriggerGraph.Editor
{
    [CustomPropertyDrawer(typeof(Ease))]
    public class EaseDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(
            SerializedProperty property)
        {
            var container = new VisualElement();

            var durationProp = property.FindPropertyRelative("duration");
            var easingProp = property.FindPropertyRelative("easing");
            var curve = property.FindPropertyRelative("curve");

            var durationField = new PropertyField(durationProp);
            durationField.Bind(property.serializedObject);
            container.Add(durationField); 

            var easingField = new PropertyField(easingProp);
            easingField.Bind(property.serializedObject);
            container.Add(easingField);

            var curveContainer = new VisualElement();
            container.Add(curveContainer);

            UpdateCurveField(easingProp.enumValueIndex);
            easingField.RegisterValueChangeCallback(v => UpdateCurveField(v.changedProperty.enumValueIndex));
            
            return container;
            void UpdateCurveField(int i)
            {
                curveContainer.Clear();
                if (i != (int)Easing.Custom) return;
                var curveField = new PropertyField(curve);
                curveField.Bind(property.serializedObject);
                curveContainer.Add(curveField);
            }
        }
    }
}