using System;
using UnityEngine;

namespace PeartreeGames.TriggerGraph
{
    [Serializable]
    public class TargetContext
    {
        public enum Target
        {
            SceneObject,
            Self,
            Invoker,
        }

        [SerializeField] private Target target;
        [SerializeField] private GameObject gameObject;

        public GameObject Get(TriggerContext ctx) =>
            target switch
            {
                Target.SceneObject => gameObject,
                Target.Self => ctx.Graph.gameObject,
                Target.Invoker => ctx.Invoker,
                _ => throw new ArgumentOutOfRangeException()
            };
    }
}