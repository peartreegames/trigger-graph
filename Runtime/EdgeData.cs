using System;
using UnityEngine;

namespace PeartreeGames.TriggerGraph
{
    [Serializable]
    public class EdgeData : ISerializationCallbackReceiver
    {
        public Guid OutputId;
        public Guid InputId;
        public string outputPortName;
        public string inputPortName;

        [SerializeField] private string outputIdString;
        [SerializeField] private string inputIdString;
        public void OnBeforeSerialize()
        {
            outputIdString = OutputId.ToString();
            inputIdString = InputId.ToString();
        }

        public void OnAfterDeserialize()
        {
            Guid.TryParse(outputIdString, out OutputId);
            Guid.TryParse(inputIdString, out InputId);
        }
    }
}