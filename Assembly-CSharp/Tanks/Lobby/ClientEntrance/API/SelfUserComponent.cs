using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientEntrance.API {
    public class SelfUserComponent : Component, AttachToEntityListener {
        public static Entity SelfUser { get; set; }

        public void AttachedToEntity(Entity entity) {
            SelfUser = entity;
        }
    }
}