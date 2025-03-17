using System.Collections;
using PeartreeGames.TriggerGraph.Utils;
using UnityEngine;

namespace PeartreeGames.TriggerGraph.Reactions
{
    [SearchTree("Reaction/Material/Set Material Color Reaction")]
    public class MaterialColorReaction : ReactionNode
    {
        [SerializeField] private Renderer renderer;
        [SerializeField] private Ease ease;

        [SerializeField, ColorUsage(true, true)]
        private Color color;

        [SerializeField] private string property;

        [SerializeField,
         Tooltip("Set to true if using instanced Built-in Render Pipeline material")]
        private bool useMaterialPropertyBlock;

        public override IEnumerator React(TriggerContext ctx, NodeData caller)
        {
            var prop = Shader.PropertyToID(property);
            var props = useMaterialPropertyBlock ? new MaterialPropertyBlock() : null;
            var startCol = renderer.material.color;
            yield return ease.Invoke(t =>
            {
                var col = Color.Lerp(startCol, color, t);
                if (useMaterialPropertyBlock)
                {
                    props!.SetColor(prop, col);
                    renderer.SetPropertyBlock(props);
                }
                else renderer.material.SetColor(prop, col);
            });
        }
    }
}