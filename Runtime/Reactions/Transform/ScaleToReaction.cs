using System;
using System.Collections;
using PeartreeGames.TriggerGraph.Utils;
using UnityEngine;

namespace PeartreeGames.TriggerGraph.Reactions
{
    [Serializable, SearchTree("Reaction/Transform/Scale To Reaction")]
    public class ScaleReaction : ReactionNode
    {
        [SerializeField] private TargetContext gameObject;
        [SerializeField] private Vector3 target;
        [SerializeField] private Ease ease;

        public override IEnumerator React(TriggerContext ctx, NodeData caller)
        {
            var transform = gameObject.Get(ctx).transform;
            var start = transform.localScale;
            yield return ease.Invoke(t =>
                transform.localScale = Vector3.Lerp(start, target, t));
            transform.localScale = target;
        }
    }
}