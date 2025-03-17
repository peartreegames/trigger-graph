using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;

namespace PeartreeGames.TriggerGraph.Editor.Attributes
{
    [CustomPropertyDrawer(typeof(BoolDropdownAttribute))]
    public class BoolDropdownDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();
            var tagField = new PopupField<string>(
                label: property.displayName,
                choices: new List<string> { "True", "False" },
                defaultValue: property.boolValue
                    ? "True"
                    : "False"
            );

            tagField.RegisterValueChangedCallback(evt =>
            {
                property.boolValue = evt.newValue == "True";
                property.serializedObject.ApplyModifiedProperties();
            });

            tagField.value = property.boolValue
                ? "True"
                : "False";
            container.Add(tagField);
            return container;
        }
    }
}