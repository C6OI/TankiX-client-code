using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(6448759673059273851L)]
    public interface EffectTemplate : Template {
        EffectComponent effectComponent();

        EffectInventoryComponent effectInventory();

        DurationComponent durationComponent();

        TankGroupComponent tankJoinComponent();

        UserGroupComponent userJoinComponent();
    }
}