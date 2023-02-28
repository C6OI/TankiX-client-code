using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class CheckUserForSpectatorEvent : Event {
        public bool UserIsSpectator { get; set; }
    }
}