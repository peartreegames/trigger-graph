using System;
using System.Collections;
using UnityEngine;

namespace PeartreeGames.TriggerGraph.Reactions
{
    [Serializable, SearchTree("Reaction/GameObject/Set Active Reaction")]
    public class SetActiveReaction : ReactionNode
    {
        [SerializeField] private TargetContext gameObject;
        [SerializeField] private bool active;
        public override IEnumerator React(TriggerContext ctx, NodeData caller)
        {
            gameObject.Get(ctx).SetActive(active);
            yield break;
        }
    }
}