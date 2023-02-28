using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(-943942723589794079L)]
    public interface BattleTemplate : Template {
        BattleComponent battle();

        ScoreLimitComponent scoreLimit();

        TimeLimitComponent timeLimit();

        UserLimitComponent userLimit();

        BattleGroupComponent battleJoin();

        [PersistentConfig]
        [AutoAdded]
        BonusClientConfigPrefabComponent bonusClientConfigPrefab();
    }
}