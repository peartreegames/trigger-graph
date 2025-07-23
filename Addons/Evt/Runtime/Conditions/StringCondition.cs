using PeartreeGames.Evt.Variables;
using UnityEngine;

namespace PeartreeGames.TriggerGraph.Evt
{
    [SearchTree("Condition/Evt/String Condition")]
    public class StringCondition : ConditionNode
    {
        [SerializeField] private EvtVariable<string> variable;
        [SerializeField] private string target;
        public override bool CheckIsSatisfied(TriggerContext ctx)
        {
            if (string.IsNullOrEmpty(variable?.Value) && string.IsNullOrEmpty(target)) return true;
            return variable?.Value == target;
        }
    }
}