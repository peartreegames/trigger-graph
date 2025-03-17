using System.Collections;
using UnityEngine;

namespace PeartreeGames.TriggerGraph.Reactions
{

    [SearchTree("Reaction/Animation/Set Animation Bool Reaction")]
    public class AnimationBoolReaction : ReactionNode
    {
        [SerializeField] private Animator animator;
        [SerializeField] private string parameterName;
        [SerializeField] private bool active;
        public override IEnumerator React(TriggerContext ctx, NodeData caller)
        {
            animator.SetBool(parameterName, active);
            yield break;
        }
    }
}