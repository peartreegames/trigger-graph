using System;
using UnityEngine;

namespace PeartreeGames.TriggerGraph
{
    public class TriggerContext
    {
        public readonly TriggerGraph Graph;
        public readonly GameObject Invoker; 
        public readonly string Tag; 
        [NonSerialized]
        public readonly DataContainer Data = new();
        
        public TriggerContext(TriggerGraph graph, GameObject invoker, string tag = null)
        {
            Graph = graph;
            Invoker = invoker;
            Tag = tag;
        }
    }
}