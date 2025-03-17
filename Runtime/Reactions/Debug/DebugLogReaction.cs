using System.Collections;
using UnityEngine;

namespace PeartreeGames.TriggerGraph.Reactions
{
    [SearchTree("Reaction/Debug/Debug Log Reaction")]
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