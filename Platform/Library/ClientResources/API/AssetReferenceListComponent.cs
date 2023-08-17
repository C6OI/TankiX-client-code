using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Library.ClientResources.API {
    public class AssetReferenceListComponent : Component {
        public AssetReferenceListComponent() { }

        public AssetReferenceListComponent(List<AssetReference> references) => References = references;

        public List<AssetReference> References { get; set; }
    }
}