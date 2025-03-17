using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PeartreeGames.TriggerGraph.Reactions
{
    [Serializable, SearchTree("Reaction/Scene/Unload Scene Reaction")]
    public class UnloadSceneReaction : ReactionNode
    {
        [SerializeField] private string scene;
        public override IEnumerator React(TriggerContext ctx, NodeData caller)
        {
            yield return SceneManager.UnloadSceneAsync(scene);
        }
    }
}