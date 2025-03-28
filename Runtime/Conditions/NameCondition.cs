using System;
using UnityEngine;

namespace PeartreeGames.TriggerGraph.Conditions
{
    [Serializable, SearchTree("Condition/GameObject/Is Name")]
    public class NameCondition : ConditionNode
    {
        [SerializeField] private TargetContext gameObject;
        [SerializeField] private string objectName;

        public override bool CheckIsSatisfied(TriggerContext ctx)
        {
            if (!string.IsNullOrEmpty(objectName))
            {
                var go = gameObject.Get(ctx);
                if (go == null || !go.name.Equals(objectName)) return false;
            }
            return true;
        }
    }

}