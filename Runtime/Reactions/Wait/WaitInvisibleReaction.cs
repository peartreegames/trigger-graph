using System.Collections;
using UnityEngine;

namespace PeartreeGames.TriggerGraph.Reactions
{
    [SearchTree("Reaction/Wait/Wait Until Invisible")]
    public class WaitInvisibleReaction : ReactionNode
    {
        [SerializeField] private Renderer target;
        public override IEnumerator React(TriggerContext ctx, NodeData caller)
        {
            while (target.isVisible) yield return null;
        }
    }
}