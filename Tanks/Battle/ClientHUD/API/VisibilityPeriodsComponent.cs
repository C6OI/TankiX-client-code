using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientHUD.API {
    public class VisibilityPeriodsComponent : MonoBehaviour, Component {
        public int firstIntervalInSec = 30;

        public int lastIntervalInSec = 30;

        public int spaceIntervalInSec = 5;

        public int lastBlinkingIntervalInSec = 10;
    }
}