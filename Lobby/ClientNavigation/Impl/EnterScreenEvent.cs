using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientNavigation.Impl {
    [SerialVersionUID(1453867134827L)]
    [Shared]
    public class EnterScreenEvent : Event {
        public EnterScreenEvent() { }

        public EnterScreenEvent(string screen) => Screen = screen;

        public string Screen { get; set; }
    }
}