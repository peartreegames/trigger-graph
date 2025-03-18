using System;
using System.Collections;
using UnityEngine;

namespace PeartreeGames.TriggerGraph.Reactions
{
    [Serializable, SearchTree("Reaction/Debug/Debug Log")]
    public class DebugLogReaction : ReactionNode
    {
        [SerializeField] private string message;
        public override IEnumerator React(TriggerContext ctx, NodeData caller)
        {
            Debug.Log(message);
            yield break;
        }
    }
}