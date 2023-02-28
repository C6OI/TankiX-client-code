using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(1485337553359L)]
    public interface SpiderEffectTemplate : EffectBaseTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        PreloadingMineKeyComponent preloadingMineKey();

        [AutoAdded]
        SpiderMineEffectComponent spiderMineEffect();

        [AutoAdded]
        DirectionEvaluatorComponent directionEvaluator();

        [AutoAdded]
        EffectInstanceRemovableComponent effectInstanceRemovable();
    }
}