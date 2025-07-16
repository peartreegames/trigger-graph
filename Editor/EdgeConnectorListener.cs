using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace PeartreeGames.TriggerGraph.Editor
{
    public class EdgeConnectorListener : IEdgeConnectorListener
    {
        private readonly TriggerGraphView _graphView;

        public EdgeConnectorListener(TriggerGraphView graphView)
        {
            _graphView = graphView;
        }

        public void OnDropOutsidePort(Edge edge, Vector2 position)
        {
            var sourcePort = edge.output ?? edge.input;
            if (sourcePort != null)
            {
                var worldPosition = _graphView.LocalToWorld(position);
                var screenPosition = GUIUtility.GUIToScreenPoint(worldPosition);

                _graphView.SearchWindow.SetAutoConnectPort(sourcePort);
                var searchContext = new SearchWindowContext(screenPosition);
                SearchWindow.Open(searchContext, _graphView.SearchWindow);
            }
        }

        public void OnDrop(GraphView graphView, Edge edge)
        {
            graphView.AddElement(edge);
        }
    }
}