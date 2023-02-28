using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientHUD.Impl {
    public class ActivateMultikillNotificationEvent : Event {
        public int Score { get; set; }

        public int Kills { get; set; }

        public string UserName { get; set; }
    }
}