using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientNavigation.API {
    public class HistoryComponent : Component {
        public Stack<ShowScreenData> screens = new();
    }
}