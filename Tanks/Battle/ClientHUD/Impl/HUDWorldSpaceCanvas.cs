using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientHUD.Impl {
    public class HUDWorldSpaceCanvas : MonoBehaviour, Component {
        public Canvas canvas;

        public GameObject nameplatePrefab;
    }
}