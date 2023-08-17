using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientHUD.Impl {
    public class NameplateTeamColorComponent : MonoBehaviour, Component {
        public Color redTeamColor;

        public Color blueTeamColor;

        public Color dmColor;
    }
}