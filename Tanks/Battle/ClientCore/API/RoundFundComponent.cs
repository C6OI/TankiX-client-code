using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(-1439124697382389571L)]
    [Shared]
    public class RoundFundComponent : Component {
        public float Fund { get; set; }
    }
}