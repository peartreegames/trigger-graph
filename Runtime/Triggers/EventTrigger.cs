using UnityEngine;

namespace PeartreeGames.TriggerGraph.Triggers
{
    [SearchTree("Trigger/Event Trigger")]
    public class EventTrigger : TriggerNode
    {
        [field: SerializeField] public override string Tag { get; set; }
    }
}