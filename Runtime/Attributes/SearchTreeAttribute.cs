using System;

namespace PeartreeGames.TriggerGraph
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SearchTreeAttribute : Attribute
    {
        public string Name;

        public SearchTreeAttribute(string name)
        {
            Name = name;
        }
    }
}