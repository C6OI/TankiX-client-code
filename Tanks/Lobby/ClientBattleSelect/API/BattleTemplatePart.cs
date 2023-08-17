using Lobby.ClientControls.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientBattleSelect.Impl;

namespace Tanks.Lobby.ClientBattleSelect.API {
    [TemplatePart]
    public interface BattleTemplatePart : Template, BattleTemplate {
        BattleStartTimeComponent battleTime();

        UserCountComponent userCount();

        SelectedListItemComponent selectedListItem();

        SearchDataComponent searchData();

        VisibleItemComponent visibleItem();

        NotFullBattleComponent notFullBattle();

        BattleLevelRangeComponent battleLevelRange();
    }
}