using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PeartreeGames.TriggerGraph.Utils
{
    public static class Coroutines
    {
        public static IEnumerator YieldAll(IEnumerable<Coroutine> coroutines)
        {
            var arr = coroutines.ToArray();
            while (arr.Any(x => x != null)) yield return null;
        }
    }
}