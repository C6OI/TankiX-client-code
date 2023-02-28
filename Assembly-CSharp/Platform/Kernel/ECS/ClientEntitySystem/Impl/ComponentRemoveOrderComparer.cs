using System;
using System.Collections.Generic;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class ComponentRemoveOrderComparer : Comparer<Type> {
        public override int Compare(Type x, Type y) => string.Compare(x.Name, y.Name, StringComparison.Ordinal);
    }
}