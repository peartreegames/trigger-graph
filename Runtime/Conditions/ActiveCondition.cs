using System;
using UnityEngine;

namespace PeartreeGames.TriggerGraph.Conditions
{
    [SearchTree("Condition/GameObject/Is Active Condition")]
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