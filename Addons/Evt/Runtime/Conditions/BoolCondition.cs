using UnityEngine;
using PeartreeGames.Evt.Variables;

namespace PeartreeGames.TriggerGraph.Evt
{
    [SearchTree("Condition/Evt/Bool")]
    public class BoolCondition : ConditionNode
    {
        [SerializeField] private EvtVariable<bool> variable;
        [SerializeField] private bool target;
        public override bool CheckIsSatisfied(TriggerContext ctx) => variable.Value == target;
    }
}