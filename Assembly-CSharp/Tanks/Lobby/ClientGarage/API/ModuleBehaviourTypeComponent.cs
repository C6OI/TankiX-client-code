using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [Shared]
    [SerialVersionUID(636341573884178402L)]
    public class ModuleBehaviourTypeComponent : Component {
        public ModuleBehaviourType Type { get; set; }
    }
}