using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Platform.Common.ClientECSCommon.API {
    [Shared]
    [SerialVersionUID(1446020883951L)]
    public class CreatedTimestampComponent : Component {
        public long Timestamp { get; set; }
    }
}