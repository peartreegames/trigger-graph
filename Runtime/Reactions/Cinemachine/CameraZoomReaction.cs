using System;
using System.Collections;
using PeartreeGames.TriggerGraph.Utils;
using Unity.Cinemachine;
using UnityEngine;

namespace PeartreeGames.TriggerGraph.Reactions
{
    [Serializable, SearchTree("Reaction/Cinemachine/Camera Zoom Reaction")]
    public class CameraZoomReaction : ReactionNode
    {
        public enum Relativity
        {
            Relative,
            Absolute
        }
        
        [SerializeField] private float zoomScale;
        [SerializeField] private Relativity relativity;
        [SerializeField] private Ease ease;

        public override IEnumerator React(TriggerContext ctx, NodeData caller)
        {
            var recomposer = CinemachineCore.GetVirtualCamera(0)
                .GetComponentInChildren<CinemachineRecomposer>();
            var start = recomposer.ZoomScale;
            var end = relativity == Relativity.Absolute ? zoomScale : start * zoomScale;
            yield return ease.Invoke(t =>
            {
                if (recomposer == null) return;
                recomposer.ZoomScale = Mathf.Lerp(start, end, t);
            });
        }
    }
}