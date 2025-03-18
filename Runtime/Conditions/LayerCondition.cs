using System;
using UnityEngine;

namespace PeartreeGames.TriggerGraph.Conditions
{
    [Serializable, SearchTree("Condition/GameObject/Is Layer")]
    public class LayerCondition : ConditionNode
    {
        [SerializeField] private TargetContext gameObject;
        [SerializeField] private LayerMask layer;

        public override bool CheckIsSatisfied(TriggerContext ctx)
        {
            if (layer.value > 0)
            {
                var go = gameObject.Get(ctx);
                if (go == null || (layer & go.layer) != 0) return false;
            }

            return true;
        }
    }

}