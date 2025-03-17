using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PeartreeGames.TriggerGraph.Reactions
{
    [SearchTree("Reaction/Wait/Wait For Input")]
    public class WaitForInputReaction : ReactionNode
    {
        [SerializeField] private InputActionReference[] references;

        public override IEnumerator React(TriggerContext ctx, NodeData caller)
        {
            while (true)
            {
                foreach (var reference in references)
                {
                    if (reference.action.phase != InputActionPhase.Started) continue;
                    yield break;
                }

                yield return null;
            }
        }
    }
}