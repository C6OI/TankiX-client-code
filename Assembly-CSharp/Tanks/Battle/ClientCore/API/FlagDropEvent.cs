using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(2921314315544889042L)]
    public class FlagDropEvent : FlagEvent {
        public bool IsUserAction { get; set; }
    }
}