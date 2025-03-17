using UnityEngine;

namespace PeartreeGames.TriggerGraph.Conditions
{
    [SearchTree("Condition/GameObject/Tag Condition")]
    public class TagCondition : ConditionNode
    {
        [SerializeField] private TargetContext gameObject;
        [SerializeField, Tag] private string tag;

        public override bool CheckIsSatisfied(TriggerContext ctx)
        {
            if (!string.IsNullOrEmpty(tag))
            {
                var go = gameObject.Get(ctx);
                if (go == null || !go.tag.Equals(tag)) return false;
            }
            return true;
        }
    }

}