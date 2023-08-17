using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientHUD.API {
    public class VisibilityIntervalComponent : MonoBehaviour, Component {
        public int intervalInSec = 2;
    }
}