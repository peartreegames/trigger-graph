﻿using System;
using System.Collections;
using System.Linq;
using PeartreeGames.TriggerGraph.Utils;
using UnityEngine;

namespace PeartreeGames.TriggerGraph
{
    [Serializable]
    public abstract class ReactionNode : NodeData
    {
        public bool IsActive { get; protected set; }
        [Input] public static string InputPort => "Input";
        [Output] public static string OnCompletePort => "OnComplete";

        public abstract IEnumerator React(TriggerContext ctx, NodeData caller);

        public override IEnumerator Execute(TriggerContext ctx, NodeData caller)
        {
            IsActive = true;
            yield return React(ctx, this);
            IsActive = false;
            var connections = ctx.Graph.GetNextNodes(this, OnCompletePort);
            yield return Coroutines.YieldAll(connections.Select(c =>
                ctx.Graph.StartCoroutine(c.Execute(ctx, this))));
        }
    }
}