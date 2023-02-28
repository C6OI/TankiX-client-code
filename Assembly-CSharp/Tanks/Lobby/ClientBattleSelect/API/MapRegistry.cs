using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientBattleSelect.API {
    public interface MapRegistry {
        Map GetMap(Entity mapEntity);
    }
}