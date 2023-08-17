using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientEntrance.Impl {
    [Shared]
    [SerialVersionUID(1453881244963L)]
    public class IncrementRegistrationNicksEvent : Event {
        public IncrementRegistrationNicksEvent() { }

        public IncrementRegistrationNicksEvent(string nick) => Nick = nick;

        public string Nick { get; set; }
    }
}