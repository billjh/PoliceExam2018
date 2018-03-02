using System;
using System.Collections.Generic;
using System.Linq;

namespace PoliceExam2018
{
    internal static class Extensions
    {
        public static bool AllSame<T>(this IEnumerable<T> items)
        {
            var array = items.ToArray();
            return array.Length != 0 && array.All(a => a.Equals(array.First()));
        }

        public static bool IsAdjacentTo(this AnswerOption option, AnswerOption other)
        {
            return Math.Abs((int) option - (int) other) == 1;
        }
    }
}