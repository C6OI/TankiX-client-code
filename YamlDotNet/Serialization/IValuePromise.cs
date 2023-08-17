using System;
using System.Diagnostics.CodeAnalysis;

namespace YamlDotNet.Serialization {
    public interface IValuePromise {
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        event Action<object> ValueAvailable;
    }
}