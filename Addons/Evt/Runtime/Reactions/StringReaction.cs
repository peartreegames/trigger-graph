using System;
using System.Collections;
using PeartreeGames.Evt.Variables;
using UnityEngine;

namespace PeartreeGames.TriggerGraph.Evt
{
    [Serializable]
    [SearchTree("Reaction/Evt/String Reaction")]
    public class StringReaction : ReactionNode
    {
        [SerializeField] private EvtString variable;
        [SerializeField] private string value;
        public override IEnumerator React(TriggerContext ctx, NodeData caller)
        {
            variable.Value = value;
            yield break;
        }
    }
}