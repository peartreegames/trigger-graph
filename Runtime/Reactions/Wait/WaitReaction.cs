using System.Collections;
using UnityEngine;

namespace PeartreeGames.TriggerGraph.Reactions
{
    [SearchTree("Reaction/Wait/Wait For Seconds")]
    public class WaitReaction : ReactionNode
    {
        [SerializeField] private float delay;
        [SerializeField] private bool realtime;
        public override IEnumerator React(TriggerContext ctx, NodeData caller)
        {
            if (realtime) yield return new WaitForSecondsRealtime(delay);
            else yield return new WaitForSeconds(delay);
        }
    }
}