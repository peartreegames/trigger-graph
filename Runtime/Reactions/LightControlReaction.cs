using System.Collections;
using PeartreeGames.TriggerGraph.Utils;
using UnityEngine;

namespace PeartreeGames.TriggerGraph.Reactions
{
    [SearchTree("Reaction/Light Reaction")]
    public class LightControlReaction : ReactionNode
    {
        [SerializeField] private Light light;
        [SerializeField] private Color color = Color.white;
        [SerializeField] private float intensity = 1f;
        [SerializeField] private Ease ease;
        public override IEnumerator React(TriggerContext ctx, NodeData caller)
        {
            var startColor = light.color;
            var startInt = light.intensity;
            yield return ease.Invoke(t =>
            {
                light.color = Color.Lerp(startColor, color, t);
                light.intensity = Mathf.Lerp(startInt, intensity, t);
            });
        }
    }
}