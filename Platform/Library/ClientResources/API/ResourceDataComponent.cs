using Platform.Library.ClientProtocol.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Platform.Library.ClientResources.API {
    [SerialVersionUID(635824350735025226L)]
    public class ResourceDataComponent : Component {
        public string Name { get; set; }

        public Object Data { get; set; }
    }
}