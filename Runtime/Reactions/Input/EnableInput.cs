using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PeartreeGames.TriggerGraph.Reactions
{
    [Serializable, SearchTree("Reaction/Input/Enable Input")]
    public class EnableInputReaction : ReactionNode
    {
        [SerializeField, BoolDropdown] private bool enable;
        [SerializeField] private InputActionReference[] references;
        public override IEnumerator React(TriggerContext ctx, NodeData caller)
        {
            foreach (var input in references)
            {
                if (enable) input.action.Enable();
                else input.action.Disable();
            }
            yield break;
        }
    }
}