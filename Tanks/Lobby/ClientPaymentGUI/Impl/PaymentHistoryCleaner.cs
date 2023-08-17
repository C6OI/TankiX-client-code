using System.Collections.Generic;
using Lobby.ClientNavigation.API;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class PaymentHistoryCleaner : ScreenHistoryCleaner {
        public override void ClearHistory(Stack<ShowScreenData> history) {
            foreach (ShowScreenData item in history) {
                if (item.ScreenType == typeof(GoodsSelectionScreenComponent)) {
                    ClearTo(item, history);
                    break;
                }
            }
        }

        void ClearTo(ShowScreenData entry, Stack<ShowScreenData> history) {
            while (history.Count > 0) {
                if (history.Peek() == entry) {
                    if (GetComponent<GoodsSelectionScreenComponent>() != null) {
                        history.Pop().FreeContext();
                    }

                    break;
                }

                history.Pop().FreeContext();
            }
        }
    }
}