using System.Collections.Generic;

namespace Tanks.Lobby.ClientNavigation.API {
    public class HomeScreenHistoryCleaner : ScreenHistoryCleaner {
        public override void ClearHistory(Stack<ShowScreenData> history) {
            foreach (ShowScreenData item in history) {
                item.FreeContext();
            }

            history.Clear();
        }
    }
}