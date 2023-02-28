using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(140335313420508312L)]
    public interface RoundUserTemplate : Template {
        RoundUserComponent roundUserComponent();

        UserGroupComponent userJoinComponent();

        BattleGroupComponent battleJoinComponent();

        RoundUserStatisticsComponent roundUserStatisticsComponent();
    }
}