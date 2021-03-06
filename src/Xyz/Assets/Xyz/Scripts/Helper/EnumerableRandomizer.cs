﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class EnumerableRandomizer
{
    public static IEnumerable<T> AsRandom<T>(this IList<T> list)
    {
        int[] indexes = Enumerable.Range(0, list.Count).ToArray();
        //Random generator = new Random();
        for (int i = 0; i < list.Count; ++i)
        {
            //int position = generator.Next(i, list.Count);
            int position = Random.Range(i, list.Count);

            yield return list[indexes[position]];

            indexes[position] = indexes[i];
        }
    }

    public static T GetRandom<T>(this IList<T> list)
    {
        int index = Random.Range(0, list.Count);
        return list[index];
    }
}
