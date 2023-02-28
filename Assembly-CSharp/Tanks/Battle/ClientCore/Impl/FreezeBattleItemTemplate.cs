using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(525358843506658817L)]
    public interface FreezeBattleItemTemplate : StreamWeaponTemplate, WeaponTemplate, Template {
        FreezeComponent freeze();
    }
}