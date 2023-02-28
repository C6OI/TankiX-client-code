using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientEntrance.Impl {
    [Shared]
    [SerialVersionUID(1438070264777L)]
    public class SaveAutoLoginTokenEvent : Event {
        public string Uid { get; set; }

        public byte[] Token { get; set; }
    }
}