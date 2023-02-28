using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(1485335642293L)]
    public interface DroneEffectTemplate : EffectBaseTemplate, Template {
        [AutoAdded]
        DirectionEvaluatorComponent directionEvaluator();

        [AutoAdded]
        UnitTargetingComponent unitTargeting();

        [AutoAdded]
        EffectInstanceRemovableComponent effectInstanceRemovable();
    }
}