using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(-2650671245931951659L)]
    [Shared]
    public class MuzzlePointSwitchEvent : Event {
        public MuzzlePointSwitchEvent() { }

        public MuzzlePointSwitchEvent(int index) => Index = index;

        public int Index { get; set; }
    }
}