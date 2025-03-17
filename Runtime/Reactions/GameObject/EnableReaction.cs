using System;
using System.Collections;
using UnityEngine;

namespace PeartreeGames.TriggerGraph.Reactions
{
    [Serializable, SearchTree("Reaction/GameObject/Enable Component Reaction")]
    public class EnableReaction : ReactionNode
    {
        [SerializeField] private MonoBehaviour behaviour;
        [SerializeField] private bool active;
        public override IEnumerator React(TriggerContext ctx, NodeData caller)
        {
            behaviour.enabled = active;
            yield break;
        }
    }
}