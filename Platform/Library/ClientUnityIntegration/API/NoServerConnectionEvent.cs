using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Library.ClientUnityIntegration.API {
    public class NoServerConnectionEvent : Event {
        public string ErrorMessage { get; set; }
    }
}