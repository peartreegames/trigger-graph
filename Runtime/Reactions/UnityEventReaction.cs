using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace PeartreeGames.TriggerGraph.Reactions
{
    [Serializable, SearchTree("Reaction/Event/UnityEvent")]
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