using System;
using System.Collections;
using PeartreeGames.TriggerGraph.Utils;
using UnityEngine;

namespace PeartreeGames.TriggerGraph.Reactions
{
    [Serializable, SearchTree("Reaction/Material/Set Material Int")]
    public class MaterialIntReaction : ReactionNode
    {
        [SerializeField] private Renderer renderer;
        [SerializeField] private Ease ease;
        [SerializeField] private string property;

        [SerializeField,
         Tooltip("Set to true if using instanced Built-in Render Pipeline material")]
        private bool useMaterialPropertyBlock;

        public override IEnumerator React(TriggerContext ctx, NodeData caller)
        {
            var prop = Shader.PropertyToID(property);
            var props = useMaterialPropertyBlock ? new MaterialPropertyBlock() : null;
            yield return ease.Invoke(t =>
            {
                var i = Mathf.RoundToInt(t);
                if (useMaterialPropertyBlock)
                {
                    props!.SetInteger(prop, i);
                    renderer.SetPropertyBlock(props);
                }
                else renderer.material.SetInteger(prop, i);
            });
        }
    }
}