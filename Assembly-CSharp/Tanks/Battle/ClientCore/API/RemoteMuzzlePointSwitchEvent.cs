using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(-8312866616397669979L)]
    public class RemoteMuzzlePointSwitchEvent : Event {
        public int Index { get; set; }
    }
}