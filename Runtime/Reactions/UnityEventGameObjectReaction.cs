using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace PeartreeGames.TriggerGraph.Reactions
{
    [Serializable, SearchTree("Reaction/Event/UnityEvent<GameObject>")]
    public class UnityEventGameObjectReaction : ReactionNode
    {
        [SerializeField] private TargetContext context;
        [SerializeField] private UnityEvent<GameObject> unityEvent;
        public override IEnumerator React(TriggerContext ctx, NodeData caller)
        {
            unityEvent?.Invoke(context.Get(ctx));
            yield break;
        }
    }
}