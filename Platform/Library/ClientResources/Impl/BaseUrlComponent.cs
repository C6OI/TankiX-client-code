using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Library.ClientResources.Impl {
    public class BaseUrlComponent : Component {
        public BaseUrlComponent() { }

        public BaseUrlComponent(string url) => Url = url;

        public string Url { get; set; }
    }
}