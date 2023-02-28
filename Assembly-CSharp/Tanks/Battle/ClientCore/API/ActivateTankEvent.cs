using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(-5086569348607290080L)]
    public class ActivateTankEvent : Event {
        public ActivateTankEvent() { }

        public ActivateTankEvent(long phase) => Phase = phase;

        public long Phase { get; set; }
    }
}