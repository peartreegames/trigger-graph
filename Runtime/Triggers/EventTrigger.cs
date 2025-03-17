using System;
using UnityEngine;

namespace PeartreeGames.TriggerGraph.Triggers
{
    [SearchTree("Trigger/Event Trigger")]
    [Serializable]
    public class EventTrigger : TriggerNode
    {
        [field: SerializeField] public override string Tag { get; set; }
    }
}