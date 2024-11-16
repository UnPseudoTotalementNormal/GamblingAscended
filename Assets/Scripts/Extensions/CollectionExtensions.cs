using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CollectionExtensions
{
    public static T PickRandom<T>(this ICollection<T> collection)
    {
        int index = Random.Range(0, collection.Count);
        return collection.ElementAt(index);
    }
}