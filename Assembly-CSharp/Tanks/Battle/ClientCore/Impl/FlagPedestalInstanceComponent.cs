using Platform.Library.ClientProtocol.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(635824353034785226L)]
    public class FlagPedestalInstanceComponent : Component {
        public GameObject FlagPedestalInstance { get; set; }
    }
}