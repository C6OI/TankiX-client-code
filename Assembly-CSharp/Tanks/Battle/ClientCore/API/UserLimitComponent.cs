using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(3911401339075883957L)]
    public class UserLimitComponent : Component {
        public int UserLimit { get; set; }

        public int TeamLimit { get; set; }
    }
}