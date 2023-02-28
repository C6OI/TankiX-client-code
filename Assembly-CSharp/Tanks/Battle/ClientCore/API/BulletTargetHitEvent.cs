using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.API {
    public class BulletTargetHitEvent : BulletHitEvent {
        public Entity Target { get; set; }
    }
}