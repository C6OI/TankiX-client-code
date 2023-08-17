using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Platform.Common.ClientECSCommon.API {
    [SerialVersionUID(1446021176912L)]
    public class OwnerComponent : Component {
        Entity Owner { get; set; }
    }
}