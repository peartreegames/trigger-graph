using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PeartreeGames.TriggerGraph.Utils;

namespace PeartreeGames.TriggerGraph.Reactions
{
    [Serializable, SearchTree("Reaction/Wait/Wait For All")]
    public class WaitForAllReaction : ReactionNode
    {
        public class InputConnections : IContextData
        {
            public readonly HashSet<Guid> Ids = new();
        }
        
        public override IEnumerator React(TriggerContext ctx, NodeData caller)
        {
            yield break;
        }
        
        public override IEnumerator Execute(TriggerContext ctx, NodeData caller)
        {
            IsActive = true;
            var data = ctx.Data.CreateOrGet<InputConnections>();
            data.Ids.Add(caller.ID);
            var prev = ctx.Graph.GetPrevNodes(this, InputPort);
            foreach (var conn in prev)
            {
                if (!data.Ids.Contains(conn.ID)) yield break;
            }

            data.Ids.Clear();
            var connections = ctx.Graph.GetNextNodes(this, OnCompletePort);
            yield return Coroutines.YieldAll(connections.Select(c =>
                ctx.Graph.StartCoroutine(c.Execute(ctx, this))));
            IsActive = false;
        }
    }
}