using System;
using System.Collections;
using PeartreeGames.TriggerGraph.Utils;
using UnityEngine;

namespace PeartreeGames.TriggerGraph.Reactions
{
    [Serializable, SearchTree("Reaction/Transform/Move To Reaction")]
    public class MoveToReaction : ReactionNode
    {
        [SerializeField] private TargetContext gameObject;
        [SerializeField] private TargetContext target;
        [SerializeField] private Ease ease;

        public override IEnumerator React(TriggerContext ctx, NodeData caller)
        {
            var transform = gameObject.Get(ctx).transform;
            var start = transform.position;
            var end = target.Get(ctx).transform.position;
            yield return ease.Invoke(t => { transform.position = Vector3.Lerp(start, end, t); });
            transform.position = end;
        }
    }
}