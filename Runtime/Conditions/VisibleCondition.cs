using System;
using UnityEngine;

namespace PeartreeGames.TriggerGraph.Conditions
{
    [Serializable, SearchTree("Condition/Renderer/Is Visible")]
    public class VisibleCondition : ConditionNode
    {
        [SerializeField] private Renderer renderer;
        [SerializeField, BoolDropdown] private bool target;

        public override bool CheckIsSatisfied(TriggerContext ctx) =>
            target switch
            {
                true => renderer.isVisible,
                false => !renderer.isVisible
            };
    }
}