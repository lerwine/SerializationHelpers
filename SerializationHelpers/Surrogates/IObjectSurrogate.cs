using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerializationHelpers.Surrogates
{
    public interface IObjectSurrogate
    {
        object Deserialized_Object { get; }
    }

    public interface IObjectSurrogate<T> : IObjectSurrogate
    {
        T Deserialized_Object { get; }
    }
}
