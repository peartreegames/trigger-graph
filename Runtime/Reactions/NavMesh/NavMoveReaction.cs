using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace PeartreeGames.TriggerGraph.Reactions
{
    [SearchTree("Reaction/NavMeshAgent/Navigate To Destination Reaction")]
    public class NavMoveReaction : ReactionNode
    {
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private TargetContext destinationTarget;
        [SerializeField] private float distanceThreshold = 0.1f;
        public override IEnumerator React(TriggerContext ctx, NodeData caller)
        {
            var pos = destinationTarget.Get(ctx).transform.position;
            agent.destination = pos;
            while (Vector3.Distance(agent.transform.position, pos) > distanceThreshold)
                yield return null;
        }
    }
}