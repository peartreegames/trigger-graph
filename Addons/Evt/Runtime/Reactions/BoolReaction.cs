using System.Collections;
using UnityEngine;
using PeartreeGames.Evt.Variables;

namespace PeartreeGames.TriggerGraph.Evt
{
    [SearchTree("Reaction/Evt/Bool")]
    public class BoolReaction : ReactionNode
    {
        [SerializeField] private EvtBool variable;
        [SerializeField] private bool value;
        public override IEnumerator React(TriggerContext context,
            NodeData caller)
        {
            variable.Value = value;
            yield break;
        }
    }
}