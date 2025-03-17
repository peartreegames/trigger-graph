using System;
using UnityEngine;
using PeartreeGames.Evt.Variables;
using PeartreeGames.TriggerGraph.Utils;

namespace PeartreeGames.TriggerGraph.Evt
{
    [SearchTree("Condition/Evt/Int")]
    public class IntCondition : ConditionNode
    {
        [SerializeField] private EvtVariable<int> variable;
        [SerializeField] private ComparisonOperator compOp;
        [SerializeField] private int target;

        public override bool CheckIsSatisfied(TriggerContext ctx) => compOp switch
        {
            ComparisonOperator.Equal => variable.Value == target,
            ComparisonOperator.NotEqual => variable.Value != target,
            ComparisonOperator.LessThan => variable.Value < target,
            ComparisonOperator.GreaterThan => variable.Value > target,
            ComparisonOperator.LessThanOrEqual => variable.Value <= target,
            ComparisonOperator.GreaterThanOrEqual => variable.Value >= target,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}