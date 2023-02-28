using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientEntrance.API {
    public static class UserABTestByID {
        public static int GetExperimentId(Entity user) => (int)(user.Id % 2);
    }
}