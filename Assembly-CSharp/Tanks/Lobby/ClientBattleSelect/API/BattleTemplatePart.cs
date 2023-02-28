using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientBattleSelect.Impl;
using Tanks.Lobby.ClientControls.API;

namespace Tanks.Lobby.ClientBattleSelect.API {
    [TemplatePart]
    public interface BattleTemplatePart : BattleTemplate, Template {
        BattleStartTimeComponent battleTime();

        UserCountComponent userCount();

        SelectedListItemComponent selectedListItem();

        SearchDataComponent searchData();

        VisibleItemComponent visibleItem();

        NotFullBattleComponent notFullBattle();

        BattleLevelRangeComponent battleLevelRange();
    }
}