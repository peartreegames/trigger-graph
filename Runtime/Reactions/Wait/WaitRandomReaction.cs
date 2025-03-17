using System.Collections;
using UnityEngine;

namespace PeartreeGames.TriggerGraph.Reactions
{
    [SearchTree("Reaction/Wait/Wait Between Seconds")]
    public class WaitRandomReaction : ReactionNode
    {
        [SerializeField] private float minDelay;
        [SerializeField] private float maxDelay;
        [SerializeField] private bool realtime;
        public override IEnumerator React(TriggerContext ctx, NodeData caller)
        {
            if (realtime) yield return new WaitForSecondsRealtime(Random.Range(minDelay, maxDelay));
            else yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
        }
    }
}