using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientEntrance.Impl {
    [Shared]
    [SerialVersionUID(1439531278716L)]
    public class PersonalPasscodeEvent : Event {
        public PersonalPasscodeEvent() { }

        public PersonalPasscodeEvent(string passcode) => Passcode = passcode;

        public string Passcode { get; set; }
    }
}