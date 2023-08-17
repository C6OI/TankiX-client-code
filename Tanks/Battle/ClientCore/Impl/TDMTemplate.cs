using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(8215935014037697786L)]
    public interface TDMTemplate : Template, BattleTemplate, TeamBattleTemplate {
        TDMComponent tdmComponent();
    }
}