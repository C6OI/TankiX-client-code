using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(6490819016194539750L)]
    public interface TeamBattleTemplate : Template, BattleTemplate {
        TeamBattleComponent teamBattle();
    }
}