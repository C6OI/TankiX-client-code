using Lobby.ClientControls.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.Hud.Impl {
    public class ItemButtonBlinkerComponent : MonoBehaviour, Component {
        public float blinkStartPercent = 0.9f;

        public Blinker blinker;
    }
}