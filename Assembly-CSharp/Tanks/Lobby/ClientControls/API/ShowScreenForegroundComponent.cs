using Platform.Library.ClientProtocol.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientControls.API {
    [SerialVersionUID(635901816912888930L)]
    public class ShowScreenForegroundComponent : MonoBehaviour, Component {
        [SerializeField] [Range(0f, 1f)] float alpha = 1f;

        public float Alpha => alpha;
    }
}