using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(-8312866616397669979L)]
    [Shared]
    public class RemoteMuzzlePointSwitchEvent : Event {
        public int Index { get; set; }
    }
}