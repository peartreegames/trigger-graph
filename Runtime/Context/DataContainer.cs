using System;
using System.Collections.Generic;

namespace PeartreeGames.TriggerGraph
{
    public class DataContainer
    {
        private readonly Dictionary<Type, IContextData> _data = new();

        public void Add<T>(T data) where T : class, IContextData => _data[typeof(T)] = data;

        public T CreateOrGet<T>() where T : class, IContextData, new()
        {
            if (_data.TryGetValue(typeof(T), out var data)) return data as T;
            var newData = new T();
            Add(newData);
            return newData;
        }
    }
}