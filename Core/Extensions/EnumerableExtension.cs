using System.Collections.Generic;
using System.Collections.Immutable;
using Core.Interfaces;
using Core.Utilities;

namespace Core.Extensions
{
    public static class EnumerableExtension
    {
        public static IValueCollection<T> AsValueSemantics<T>(this IEnumerable<T> list)
        {
            return new ImmutableListWithValueSemantics<T>(list.ToImmutableList());
        }
    }
}