using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(1485335125183L)]
    public interface DroneWeaponTemplate : Template {
        [AutoAdded]
        WeaponUnblockedComponent weaponUnblocked();

        [AutoAdded]
        ShotIdComponent shotIdComponent();

        [AutoAdded]
        [PersistentConfig]
        VerticalTargetingComponent verticalTargeting();

        [AutoAdded]
        DirectionEvaluatorComponent directionEvaluator();
    }
}