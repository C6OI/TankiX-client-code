using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(1486709084156L)]
    public interface MineEffectTemplate : EffectBaseTemplate, Template {
        [AutoAdded]
        MineEffectComponent mineEffect();

        [AutoAdded]
        [PersistentConfig]
        PreloadingMineKeyComponent preloadingMineKey();

        [AutoAdded]
        EffectInstanceRemovableComponent effectInstanceRemovable();
    }
}