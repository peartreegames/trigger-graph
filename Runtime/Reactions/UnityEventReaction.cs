using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace PeartreeGames.TriggerGraph.Reactions
{
    [SearchTree("Reaction/UnityEvent")]
    public class UnityEventReaction : ReactionNode
    {
        [SerializeField] private UnityEvent unityEvent;
        public override IEnumerator React(TriggerContext ctx, NodeData caller)
        {
            unityEvent?.Invoke();
            yield break;
        }
    }
}