using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PeartreeGames.TriggerGraph.Reactions
{
    [Serializable, SearchTree("Reaction/GameObject/Instantiate")]
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
                Object.Instantiate(prefab, spawn.position, spawn.rotation, parent);
                yield return new WaitForSeconds(delayPerSpawn);
            }
        }
    }
}