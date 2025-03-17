using System;
using System.Collections;
using System.Linq;
using PeartreeGames.TriggerGraph.Utils;

namespace PeartreeGames.TriggerGraph
{
    [Serializable]
    public abstract class ConditionNode : NodeData
    {
        [Input] public static string InputPort => "Input";
        [Output(color: PortColor.Green)] public static string TruePort => "True";
        [Output(color: PortColor.Red)] public static string FalsePort => "False";

        public abstract bool CheckIsSatisfied(TriggerContext ctx);

        public override IEnumerator Execute(TriggerContext ctx, NodeData caller)
        {
            var connections =
                ctx.Graph.GetNextNodes(this, CheckIsSatisfied(ctx) ? TruePort : FalsePort);
            yield return Coroutines.YieldAll(connections.Select(c =>
                ctx.Graph.StartCoroutine(c.Execute(ctx, this))));
        }
    }
}