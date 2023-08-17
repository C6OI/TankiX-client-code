using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientBattleSelect.Impl;

namespace Tanks.Lobby.ClientBattleSelect.API {
    [SerialVersionUID(635754216736135597L)]
    public interface BattleSelectScreenTemplate : ScreenTemplate, Template {
        BattleSelectScreenComponent battleSelectScreen();

        BattleSelectLoadedComponent battleSelectLoaded();

        [AutoAdded]
        VisibleItemsRangeComponent visibleItemsRange();

        [PersistentConfig]
        [AutoAdded]
        BattleSelectScreenHeaderTextComponent battleSelectScreenHeaderText();

        [AutoAdded]
        [PersistentConfig]
        ScoreTableEmptyRowTextComponent scoreTableEmptyRowText();

        [PersistentConfig]
        BattleSelectScreenLocalizationComponent battleSelectScreenLocalization();

        [PersistentConfig]
        [AutoAdded]
        InviteFriendsConfigComponent inviteFriendsConfig();

        [PersistentConfig]
        InviteFriendsPanelLocalizationComponent inviteFriendsPanelLocalization();
    }
}