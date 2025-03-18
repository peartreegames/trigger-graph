using System;
using System.Collections;
using PeartreeGames.TriggerGraph.Utils;
using UnityEngine;

namespace PeartreeGames.TriggerGraph.Reactions
{
    [Serializable, SearchTree("Reaction/Transform/Rotate To")]
    public class RotateReaction : ReactionNode
    {
        [SerializeField] private TargetContext gameObject;
        [SerializeField] private TargetContext target;
        [SerializeField] private Ease ease;

        public override IEnumerator React(TriggerContext ctx, NodeData caller)
        {
            var transform = gameObject.Get(ctx).transform;
            var start = transform.rotation;
            var end = target.Get(ctx).transform.rotation;
            yield return ease.Invoke(t =>
                transform.rotation = Quaternion.Lerp(start, end, t));
            transform.rotation = end;
        }
    }
}