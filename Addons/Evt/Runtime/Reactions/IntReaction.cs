using System;
using System.Collections;
using UnityEngine;
using PeartreeGames.Evt.Variables;
using PeartreeGames.TriggerGraph.Utils;

namespace PeartreeGames.TriggerGraph.Evt
{
    [SearchTree("Reaction/Evt/Int Reaction")]
    public class IntReaction : ReactionNode
    {
        [SerializeField] private EvtInt variable;
        [SerializeField] private ArithmeticOperator arithOp;
        [SerializeField] private int value;

        public override IEnumerator React(TriggerContext context,
            NodeData caller)
        {
            var result = arithOp switch
            {
                ArithmeticOperator.Sum => variable.Value + value,
                ArithmeticOperator.Subtract => variable.Value - value,
                ArithmeticOperator.Multiply => variable.Value * value,
                ArithmeticOperator.Divide => variable.Value / value,
                ArithmeticOperator.Modulo => variable.Value % value,
                _ => throw new ArgumentOutOfRangeException()
            };
            variable.Value = result;
            yield break;
        }
    }
}