using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(3413384256910001471L)]
    public interface IsisBattleItemTemplate : Template, StreamWeaponTemplate, WeaponTemplate {
        IsisComponent isis();

        [PersistentConfig]
        DistanceAndAngleTargetEvaluatorComponent distanceAndAngleTargetEvaluator();
    }
}