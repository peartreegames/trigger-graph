using System.Collections.Generic;
using System.Linq;
using PeartreeGames.TriggerGraph.Triggers;
using UnityEngine;

namespace PeartreeGames.TriggerGraph
{
    [DisallowMultipleComponent]
    public class TriggerGraph : MonoBehaviour
    {
        public List<NodeData> nodes = new();
        public List<EdgeData> edges = new();
        
        public GameObject NextEventInvoker { get; set; }

        private void Start()
        {
            ExecuteTriggers<LifecycleTrigger>(new TriggerContext(this, gameObject, LifecycleTrigger.Type.Start.ToString()));
        }

        private void OnEnable()
        {
            ExecuteTriggers<LifecycleTrigger>(new TriggerContext(this, gameObject, LifecycleTrigger.Type.OnEnable.ToString()));
        }

        private void OnDisable()
        {
            ExecuteTriggers<LifecycleTrigger>(new TriggerContext(this, gameObject, LifecycleTrigger.Type.OnDisable.ToString()));
        }

        private void ExecuteTriggers<T>(TriggerContext ctx) where T : TriggerNode
        {
            var triggers = GetTriggerNodes<T>();
            foreach (var trigger in triggers)
            {
                if (string.IsNullOrEmpty(ctx.Tag) || ctx.Tag.Equals(trigger.Tag))
                {
                    StartCoroutine(trigger.Execute(ctx, null));
                }
            }
        }

        public void InvokeTrigger(string triggerTag)
        {
            ExecuteTriggers<EventTrigger>(new TriggerContext(this, NextEventInvoker, triggerTag));
            NextEventInvoker = null;
        }

        public void InvokeTrigger(GameObject invoker, string triggerTag)
        {
            ExecuteTriggers<EventTrigger>(new TriggerContext(this, invoker, triggerTag));
            NextEventInvoker = null;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            ExecuteTriggers<CollisionTrigger>(new TriggerContext(this, other.gameObject,
                CollisionTrigger.Type.TriggerEnter.ToString()));
        }

        private void OnTriggerExit(Collider other)
        {
            ExecuteTriggers<CollisionTrigger>(new TriggerContext(this, other.gameObject,
                CollisionTrigger.Type.TriggerExit.ToString()));
        }

        private void OnCollisionEnter(Collision other)
        {
            ExecuteTriggers<CollisionTrigger>(new TriggerContext(this, other.gameObject,
                CollisionTrigger.Type.CollisionEnter.ToString()));
        }

        private void OnCollisionExit(Collision other)
        {
            ExecuteTriggers<CollisionTrigger>(new TriggerContext(this, other.gameObject,
                CollisionTrigger.Type.CollisionExit.ToString()));
        }

        private List<T> GetTriggerNodes<T>() where T : TriggerNode =>
            nodes.Where(n => n is T).Cast<T>().ToList();

        public List<NodeData> GetNextNodes(NodeData node, string outputPortName)
        {
            var edgeData = edges.FindAll(e =>
                e.OutputId == node.ID && e.outputPortName == outputPortName);
            return nodes.FindAll(n => edgeData.Exists(e => e.InputId == n.ID));
        }

        public List<NodeData> GetPrevNodes(NodeData node, string inputPortName)
        {
            var edgeData =
                edges.FindAll(e => e.InputId == node.ID && e.inputPortName == inputPortName);
            return nodes.FindAll(n => edgeData.Exists(e => e.OutputId == n.ID));
        }
    }
}