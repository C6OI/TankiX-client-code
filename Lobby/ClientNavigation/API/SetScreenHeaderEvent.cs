using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientNavigation.API {
    public class SetScreenHeaderEvent : Event {
        string header;

        public string Header {
            get => header ?? string.Empty;
            set => header = value;
        }

        public bool Animate { get; set; } = true;

        public void Animated(string header) {
            Animate = true;
            Header = header;
        }

        public void Immediate(string header) {
            Animate = false;
            Header = header;
        }
    }
}