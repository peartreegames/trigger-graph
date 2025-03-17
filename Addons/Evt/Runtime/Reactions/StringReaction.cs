using System.Collections;
using PeartreeGames.Evt.Variables;
using UnityEngine;

namespace PeartreeGames.TriggerGraph.Evt
{
    [SearchTree("Reaction/Evt/String")]
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