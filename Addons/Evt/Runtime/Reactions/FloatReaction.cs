using System;
using System.Collections;
using UnityEngine;
using PeartreeGames.Evt.Variables;
using PeartreeGames.TriggerGraph.Utils;

namespace PeartreeGames.TriggerGraph.Evt
{
    [SearchTree("Reaction/Evt/Float Reaction")]
    public class FloatReaction : ReactionNode
    {
        [SerializeField] private EvtFloat variable;
        [SerializeField] private ArithmeticOperator arithOp;
        [SerializeField] private float value;

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
                ArithmeticOperator.Set => value,
                _ => throw new ArgumentOutOfRangeException()
            };
            variable.Value = result;
            yield break;
        }
    }
}