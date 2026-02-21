
using System;
using System.Collections;
using PeartreeGames.Evt.Variables;
using UnityEngine;

namespace PeartreeGames.TriggerGraph.Evt
{
    [Serializable]
    [SearchTree("Reaction/Evt/Wait Until")]
    public class WaitUntilReaction : ReactionNode
    {
        [SerializeField] private bool target = true;
        [SerializeField] private EvtVariable<bool> value;

        public override IEnumerator React(TriggerContext context,
            NodeData caller)
        {
            while (value.Value != target) yield return null;
        }
    }
}