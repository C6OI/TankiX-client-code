using Tanks.Lobby.ClientControls.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ArmsRaceRewardNumberComponent : MonoBehaviour, Component {
        public AnimatedLong quantity;

        public int initialVal;

        void Start() {
            quantity.SetImmediate(-1L);
            quantity.Value = initialVal;
        }
    }
}