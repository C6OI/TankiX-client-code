using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Library.ClientUnityIntegration.API {
    public class ApplicationFocusEvent : Event {
        public bool IsFocused { get; set; }
    }
}