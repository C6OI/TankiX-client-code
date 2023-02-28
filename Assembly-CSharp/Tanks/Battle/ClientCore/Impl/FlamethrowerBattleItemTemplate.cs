using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(4652768934679402653L)]
    public interface FlamethrowerBattleItemTemplate : StreamWeaponTemplate, WeaponTemplate, Template {
        FlamethrowerComponent flamethrower();
    }
}