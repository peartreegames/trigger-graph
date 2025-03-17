using System.Collections;
using UnityEngine;

namespace PeartreeGames.TriggerGraph.Reactions
{
    [SearchTree("Reaction/GameObject/Instantiate Reaction")]
    public class InstantiateReaction : ReactionNode
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private Transform spawn;
        [SerializeField] private Transform parent;
        [SerializeField] private int quantity = 1;
        [SerializeField] private float delayPerSpawn;
        public override IEnumerator React(TriggerContext ctx, NodeData caller)
        {
            for (var i = 0; i < quantity; i++)
            {
                Instantiate(prefab, spawn.position, spawn.rotation, parent);
                yield return new WaitForSeconds(delayPerSpawn);
            }
        }
    }
}