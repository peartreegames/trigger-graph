using System;
using UnityEngine;

namespace PeartreeGames.TriggerGraph.Triggers
{
    [SearchTree("Trigger/Lifecycle Trigger")]
    [Serializable]
    public class LifecycleTrigger : TriggerNode 
    {
        public enum Type
        {
            Start,
            OnEnable,
            OnDisable,
        }
        
        [field: SerializeField] public Type Lifecycle { get; private set; }
        public override string Tag
        {
            get => Lifecycle.ToString();
            set => throw new System.NotImplementedException();
        }
    }
}