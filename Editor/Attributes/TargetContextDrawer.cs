using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace PeartreeGames.TriggerGraph.Editor.Attributes
{
    [CustomPropertyDrawer(typeof(TargetContext))]
    public class TargetContextDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();
            var targetProp = property.FindPropertyRelative("target");
            var targetField = new PropertyField(targetProp)
            {
                label = property.displayName
            };
            targetField.Bind(targetProp.serializedObject);
            container.Add(targetField);

            var gameObjectContainer = new VisualElement
            {
                style = { marginLeft = 20 } // Adjust this value as needed for the desired indentation
            };
              
            container.Add(gameObjectContainer);

            UpdateGameObjectField(targetProp.enumValueIndex);
            targetField.RegisterValueChangeCallback(v =>
                UpdateGameObjectField(v.changedProperty.enumValueIndex));

            return container;

            void UpdateGameObjectField(int i)
            {
                gameObjectContainer.Clear();
                if (i != (int)TargetContext.Target.SceneObject) return;
                var gameObjectField =
                    new PropertyField(property.FindPropertyRelative("gameObject"));
                gameObjectField.Bind(targetProp.serializedObject);
                gameObjectContainer.Add(gameObjectField);
            }
        }
    }
}