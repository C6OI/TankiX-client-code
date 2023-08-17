using Platform.Library.ClientUnityIntegration.API;

namespace Lobby.ClientControls.API {
    public abstract class LocalizedControl : FromConfigBehaviour {
        protected override string GetRelativeConfigPath() => "/ui/element";
    }
}