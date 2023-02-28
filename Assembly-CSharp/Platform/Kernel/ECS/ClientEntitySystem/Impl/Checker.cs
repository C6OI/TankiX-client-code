using System.Collections;
using System.Collections.Generic;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class Checker {
        public static void RequireNotEmpty(ICollection c) {
            if (c.Count != 0) {
                throw new EmptyCollectionNotSupportedException();
            }
        }

        public static void RequireOneOnly<T>(ICollection<T> c) {
            if (c.Count != 1) {
                throw new RequiredOneElementOnlyException();
            }
        }
    }
}