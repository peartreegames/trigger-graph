using System;
using System.Collections;
using PeartreeGames.TriggerGraph.Utils;
using UnityEngine;

namespace PeartreeGames.TriggerGraph.Reactions
{
    [Serializable, SearchTree("Reaction/Time/Time Scale")]
    public class TimeScaleReaction : ReactionNode
    {
        [SerializeField] private Ease ease;

        public override IEnumerator React(TriggerContext ctx, NodeData caller)
        {
            yield return ease.Invoke(t =>
            {
                Time.timeScale = t;
            }, TimeScale.Real);
        }
    }
}