using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Platform.Common.ClientECSCommon.API {
    [Shared]
    [SerialVersionUID(1446024502618L)]
    public class NameComponent : Component {
        public NameComponent() { }

        public NameComponent(string name) => Name = name;

        public string Name { get; set; }
    }
}