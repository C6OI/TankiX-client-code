using Platform.Library.ClientProtocol.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(635824353088195226L)]
    public class FlagInstanceComponent : Component {
        public GameObject FlagInstance { get; set; }

        public GameObject FlagBeam { get; set; }
    }
}