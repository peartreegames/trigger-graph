using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace PeartreeGames.TriggerGraph.Editor
{
    public class UnitySerializeFieldContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member,
            MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            if (member is not FieldInfo field) return property;
            if (typeof(UnityEngine.Object).IsAssignableFrom(field.FieldType))
            {
                property.Converter = new UnityObjectJsonConverter();
            }

            if (!field.IsPrivate) return property;
            var hasSerializeField = field.GetCustomAttribute<SerializeField>() != null;
            if (!hasSerializeField) return property;
            property.Writable = true;
            property.Readable = true;

            return property;
        }

        protected override IList<JsonProperty> CreateProperties(Type type,
            MemberSerialization memberSerialization)
        {
            var properties = new List<JsonProperty>();

            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public |
                                        BindingFlags.NonPublic);

            foreach (var field in fields)
            {
                if (!field.IsPublic && field.GetCustomAttribute<SerializeField>() == null) continue;
                var property = CreateProperty(field, memberSerialization);
                properties.Add(property);
            }

            return properties;
        }
    }
}
