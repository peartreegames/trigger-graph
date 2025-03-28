﻿using System;
using System.Collections;
using UnityEngine;

namespace PeartreeGames.TriggerGraph.Reactions
{

    [Serializable, SearchTree("Reaction/Animation/Set Animation Trigger")]
    public class AnimationTriggerReaction : ReactionNode
    {
        [SerializeField] private Animator animator;
        [SerializeField] private string parameterName;
        public override IEnumerator React(TriggerContext ctx, NodeData caller)
        {
            animator.SetTrigger(parameterName);
            yield break;
        }
    }
}