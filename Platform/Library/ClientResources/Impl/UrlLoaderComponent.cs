using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Platform.Library.ClientResources.Impl {
    public class UrlLoaderComponent : Component {
        public UrlLoaderComponent(Loader loader) => Loader = loader;

        public Loader Loader { get; set; }
    }
}