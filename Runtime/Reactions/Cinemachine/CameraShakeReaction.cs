using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

namespace PeartreeGames.TriggerGraph.Reactions
{
    [Serializable, SearchTree("Reaction/Cinemachine/Camera Shake Reaction")]
    public class CameraShakeReaction : ReactionNode
    {
        [SerializeField] private Transform source;
        [SerializeField] private Vector3 velocity = Vector3.down * 0.1f;


        [SerializeField] private CinemachineImpulseDefinition impulse = new()
        {
            ImpulseChannel = 1,
            ImpulseShape = CinemachineImpulseDefinition.ImpulseShapes.Bump,
            CustomImpulseShape = new AnimationCurve(),
            ImpulseDuration = 0.2f,
            ImpulseType = CinemachineImpulseDefinition.ImpulseTypes.Uniform,
            DissipationDistance = 100,
            DissipationRate = 0.25f,
            PropagationSpeed = 343
        };

        public override IEnumerator React(TriggerContext ctx, NodeData caller)
        {
            impulse.CreateEvent(source.position, velocity);
            yield break;
        }
    }
}