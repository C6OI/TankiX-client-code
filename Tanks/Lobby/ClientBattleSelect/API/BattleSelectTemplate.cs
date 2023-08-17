using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.API {
    [SerialVersionUID(1436248526471L)]
    public interface BattleSelectTemplate : Template {
        BattleSelectComponent battleSelect();

        [AutoAdded]
        SearchResultComponent searchResult();
    }
}