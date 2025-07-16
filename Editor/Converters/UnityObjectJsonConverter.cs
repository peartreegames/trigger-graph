using System;
using Newtonsoft.Json;
using UnityEditor;

namespace PeartreeGames.TriggerGraph.Editor
{
    public class UnityObjectJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(UnityEngine.Object).IsAssignableFrom(objectType);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            var unityObject = (UnityEngine.Object)value;
            var globalId = GlobalObjectId.GetGlobalObjectIdSlow(unityObject);
            
            writer.WriteStartObject();
            writer.WritePropertyName("globalId");
            writer.WriteValue(globalId.ToString());
            writer.WritePropertyName("typeName");
            writer.WriteValue(unityObject.GetType().AssemblyQualifiedName);
            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            var obj = serializer.Deserialize<UnityObjectReference>(reader);
            if (obj == null || string.IsNullOrEmpty(obj.globalId))
                return null;

            if (GlobalObjectId.TryParse(obj.globalId, out var globalId))
            {
                var unityObject = GlobalObjectId.GlobalObjectIdentifierToObjectSlow(globalId);
                return unityObject;
            }

            return null;
        }

        [Serializable]
        private class UnityObjectReference
        {
            public string globalId;
            public string typeName;
        }
    }
}
