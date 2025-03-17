using System;

namespace PeartreeGames.TriggerGraph.Conditions
{
    [Serializable, SearchTree("Condition/True If Any Condition")]
    public class AnyCondition: ConditionNode
    {
        [Output(PortOrientation.Vertical, PortColor.Yellow)] public static string CheckAnyPort => "True If Any";

        public override bool CheckIsSatisfied(TriggerContext ctx)
        {
            var connections = ctx.Graph.GetNextNodes(this, CheckAnyPort);
            foreach (var connection in connections)
            {
                if (connection is ConditionNode condition && condition.CheckIsSatisfied(ctx))
                    return true;
            }
            return false;
        }
    }
}