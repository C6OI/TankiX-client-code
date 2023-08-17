using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientHUD.Impl {
    [SerialVersionUID(1450950140104L)]
    [Shared]
    public class BattleChatMessageReceivedEvent : Event {
        public string Message { get; set; }
    }
}