using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PeartreeGames.TriggerGraph.Reactions
{
    [Serializable, SearchTree("Reaction/Scene/Load Scene")]
    public class LoadSceneReaction : ReactionNode
    {
        [SerializeField] private LoadSceneMode mode;
        [SerializeField] private string sceneName;
        public override IEnumerator React(TriggerContext ctx, NodeData caller)
        {
            yield return SceneManager.LoadSceneAsync(sceneName, mode);
        }
    }
}