using System;
using PeartreeGames.Evt.Variables;
using UnityEngine;

namespace PeartreeGames.TriggerGraph.Evt
{
    [Serializable, SearchTree("Trigger/Evt Trigger")]
    public class EvtTrigger : TriggerNode
    {
        [SerializeField] private EvtGameObject evt;
        private TriggerGraph _graph;
        
        public override string Tag
        {
            get => evt.name;
            set => throw new NotSupportedException();
        }

        public override void OnEnable(TriggerGraph graph)
        {
            _graph = graph;
            evt.OnEvt += Invoke;
        }

        public override void OnDisable(TriggerGraph graph)
        {
            _graph = graph;
            evt.OnEvt -= Invoke;
        }

        private void Invoke(GameObject go)
        {
            if (_graph == null) return;
             _graph.StartCoroutine(Execute(new TriggerContext(_graph, go), null));
        }
    }
}