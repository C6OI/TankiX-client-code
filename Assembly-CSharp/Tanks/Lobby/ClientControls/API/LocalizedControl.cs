using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Lobby.ClientControls.API {
    public abstract class LocalizedControl : FromConfigBehaviour {
        [SerializeField] string path = "/ui/element";

        protected override string GetRelativeConfigPath() => path;
    }
}