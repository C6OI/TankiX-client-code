using Platform.Library.ClientProtocol.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientHUD.API {
    [SerialVersionUID(635824351578525226L)]
    public class BattleScreenComponent : MonoBehaviour, Component {
        public TimeProgressBar timeProgressBar;
    }
}