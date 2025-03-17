using System;
using System.Collections;
using UnityEngine;

namespace PeartreeGames.TriggerGraph.Reactions
{
    [Serializable, SearchTree("Reaction/Particle/Particle Reaction")]
    public class ParticleControlReaction : ReactionNode
    {
        public enum State
        {
            Play,
            Pause,
            Stop,
        }

        [SerializeField] private ParticleSystem particles;
        [SerializeField] private State action;

        [SerializeField]
        private bool delayUntilComplete;
        
        public override IEnumerator React(TriggerContext ctx, NodeData caller)
        {
            switch (action)
            {
                case State.Play:
                    particles.Play();
                    if (delayUntilComplete)
                        yield return new WaitUntil(() => particles.isPlaying); 
                    break;
                case State.Pause:
                    particles.Pause();
                    break;
                case State.Stop:
                    particles.Stop();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}