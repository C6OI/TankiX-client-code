using Platform.Kernel.ECS.ClientEntitySystem.Impl;

namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public interface EngineHandlerRegistrationListener {
        void OnHandlerAdded(Handler handler);
    }
}