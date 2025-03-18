
using System;

namespace PeartreeGames.TriggerGraph.Conditions
{
    [Serializable, SearchTree("Condition/True If All")]
    public class AllCondition: ConditionNode
    {
        [Output(PortOrientation.Vertical, PortColor.Yellow)] public static string CheckAllPort => "True If All";

        public override bool CheckIsSatisfied(TriggerContext ctx)
        {
            var connections = ctx.Graph.GetNextNodes(this, CheckAllPort);
            foreach (var connection in connections)
            {
                if (connection is ConditionNode condition && !condition.CheckIsSatisfied(ctx))
                    return false;
            }

            return true;
        }
    }
}