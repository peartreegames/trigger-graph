using System;
using System.Collections;
using UnityEngine;

namespace PeartreeGames.TriggerGraph
{
    public abstract class NodeData : ScriptableObject, ISerializationCallbackReceiver, IEquatable<NodeData>
    {
        public Guid ID;
        public string nodeIdString;
        public Vector2 nodePosition;

#if UNITY_EDITOR
        #pragma warning disable CS0414
        [HideInInspector] [SerializeField] private bool isExpanded = true;
        #pragma warning restore CS0414
#endif
        public abstract IEnumerator Execute(TriggerContext ctx, NodeData caller);

        public void OnBeforeSerialize() => nodeIdString = ID.ToString();
        public void OnAfterDeserialize() => Guid.TryParse(nodeIdString, out ID);

        public bool Equals(NodeData other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && ID.Equals(other.ID);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((NodeData)obj);
        }

        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), ID);
    }

}