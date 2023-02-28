using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(-3596341255560623830L)]
    public class TimeLimitComponent : Component {
        public long TimeLimitSec { get; set; }

        public long WarmingUpTimeLimitSec { get; set; }
    }
}