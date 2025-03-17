using System;
using UnityEngine;

namespace PeartreeGames.TriggerGraph.Triggers
{
    [Serializable, SearchTree("Trigger/Collision Trigger")]
    public class CollisionTrigger : TriggerNode 
    {
        public enum Type
        {
            TriggerEnter,
            TriggerExit,
            CollisionEnter,
            CollisionExit
        }
        
        [field: SerializeField] public Type Collision { get; private set; }
        public override string Tag
        {
            get => Collision.ToString();
            set => throw new System.NotImplementedException();
        }
    }
}