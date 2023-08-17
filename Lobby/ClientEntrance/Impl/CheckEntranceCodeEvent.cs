using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientEntrance.Impl {
    [Shared]
    [SerialVersionUID(636071966771169354L)]
    public class CheckEntranceCodeEvent : Event {
        public CheckEntranceCodeEvent() { }

        public CheckEntranceCodeEvent(string code) => Code = code;

        public string Code { get; set; }
    }
}