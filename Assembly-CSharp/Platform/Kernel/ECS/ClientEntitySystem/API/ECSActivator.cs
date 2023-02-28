using Platform.Kernel.OSGi.ClientCore.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public interface ECSActivator : Activator {
        void RegisterSystemsAndTemplates();
    }
}