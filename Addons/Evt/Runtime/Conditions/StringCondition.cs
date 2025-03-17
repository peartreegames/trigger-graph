using PeartreeGames.Evt.Variables;
using UnityEngine;

namespace PeartreeGames.TriggerGraph.Evt
{
    [SearchTree("Condition/Evt/String")]
    public class StringCondition : ConditionNode
    {
        [SerializeField] private EvtVariable<string> variable;
        [SerializeField] private string target;
        public override bool CheckIsSatisfied(TriggerContext ctx) => variable.Value == target;
    }
}