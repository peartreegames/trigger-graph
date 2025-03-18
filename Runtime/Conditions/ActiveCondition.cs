using System;
using UnityEngine;

namespace PeartreeGames.TriggerGraph.Conditions
{
    [Serializable, SearchTree("Condition/GameObject/Is Active")]
    public class ActiveCondition : ConditionNode
    {
        [SerializeField] private TargetContext gameObject;
        [SerializeField, BoolDropdown] private bool isActive;
        public override bool CheckIsSatisfied(TriggerContext ctx) =>
            isActive switch
            {
                true => gameObject.Get(ctx).activeInHierarchy,
                false => !gameObject.Get(ctx).activeInHierarchy
            };
    }
}