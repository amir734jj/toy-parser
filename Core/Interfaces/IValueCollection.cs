using System;
using System.Collections.Immutable;

namespace Core.Interfaces
{
    public interface IValueCollection<T> : IImmutableList<T>, IEquatable<IValueCollection<T>>
    {
    }
}