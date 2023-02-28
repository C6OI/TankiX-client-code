using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Lobby.ClientQuests.API {
    [Shared]
    [SerialVersionUID(1476707093577L)]
    public class QuestExpireDateComponent : Component {
        public Date Date { get; set; }
    }
}