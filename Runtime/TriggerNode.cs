using System;
using System.Collections;
using System.Linq;
using PeartreeGames.TriggerGraph.Utils;

namespace PeartreeGames.TriggerGraph
{
    [Serializable]
    public abstract class TriggerNode : NodeData
    {
        public abstract string Tag { get; set; }
        [Output] public static string OnTriggerPort => "OnTrigger";

        public override IEnumerator Execute(TriggerContext ctx, NodeData caller)
        {
            var connections = ctx.Graph.GetNextNodes(this, OnTriggerPort);
            yield return Coroutines.YieldAll(connections.Select(c =>
                ctx.Graph.StartCoroutine(c.Execute(ctx, this))));
        }
    }
}