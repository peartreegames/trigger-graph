
using System;
using UnityEngine;
using PeartreeGames.Evt.Variables;
using PeartreeGames.TriggerGraph.Utils;

namespace PeartreeGames.TriggerGraph.Evt
{
    [Serializable]
    [SearchTree("Condition/Evt/Float Condition")]
    public class FloatCondition : ConditionNode
    {
        [SerializeField] private EvtVariable<float> variable;
        [SerializeField] private ComparisonOperator compOp;
        [SerializeField] private float target;

        public override bool CheckIsSatisfied(TriggerContext ctx) => compOp switch
        {
            ComparisonOperator.Equal => Math.Abs(variable.Value - target) < Mathf.Epsilon,
            ComparisonOperator.NotEqual => Math.Abs(variable.Value - target) > Mathf.Epsilon,
            ComparisonOperator.LessThan => variable.Value < target,
            ComparisonOperator.GreaterThan => variable.Value > target,
            ComparisonOperator.LessThanOrEqual => variable.Value <= target,
            ComparisonOperator.GreaterThanOrEqual => variable.Value >= target,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}