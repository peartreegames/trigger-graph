using System;
using PeartreeGames.TriggerGraph.Utils;
using UnityEngine;

namespace PeartreeGames.TriggerGraph.Conditions
{
    [Serializable, SearchTree("Condition/Is Distance Check")]
    public class DistanceCondition : ConditionNode
    {
        [SerializeField] private TargetContext fromGameObject;
        [SerializeField] private TargetContext toGameObject;
        [SerializeField] private ComparisonOperator compOp;
        [SerializeField] private float distance;
        public override bool CheckIsSatisfied(TriggerContext ctx)
        {
            var fromPosition = fromGameObject.Get(ctx).transform.position;
            var toPosition = toGameObject.Get(ctx).transform.position;
            return compOp switch
            {
                ComparisonOperator.Equal => Vector3.Distance(fromPosition, toPosition) - distance < Mathf.Epsilon,
                ComparisonOperator.NotEqual => Vector3.Distance(fromPosition, toPosition) - distance > Mathf.Epsilon,
                ComparisonOperator.LessThan => Vector3.Distance(fromPosition, toPosition) < distance,
                ComparisonOperator.GreaterThan => Vector3.Distance(fromPosition, toPosition) > distance,
                ComparisonOperator.LessThanOrEqual => Vector3.Distance(fromPosition, toPosition) <= distance,
                ComparisonOperator.GreaterThanOrEqual => Vector3.Distance(fromPosition, toPosition) >= distance,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}