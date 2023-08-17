using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.ClientLauncher.Impl {
    public class LauncherDownloadScreenComponent : MonoBehaviour, Component {
        public Text loadingInfo;
    }
}