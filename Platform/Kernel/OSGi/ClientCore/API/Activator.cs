using System;

namespace Platform.Kernel.OSGi.ClientCore.API {
    public interface Activator {
        void Launch(Action onComplete = null);
    }
}