﻿using System.Collections;
using System.Collections.Generic;

namespace RevitLookup.Extensions
{
    public static class EnumerableExtensions
    {
        public static List<T> ToList<T>(this IEnumerable set)
        {
            var list = new List<T>();

            foreach (T element in set) list.Add(element);

            return list;
        }
    }
}