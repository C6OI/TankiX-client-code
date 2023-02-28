using System.Collections.Generic;
using Tanks.Lobby.ClientBattleSelect.Impl;
using Tanks.Lobby.ClientControls.API;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class LeagueSpecialOfferComponent : ItemContainerComponent {
        public LocalizedField worthItText;

        public void ShowOfferItems(List<SpecialOfferItem> items, int worthIt) {
            InstantiateItems(items);
            string saleText = string.Format(worthItText.Value, worthIt);
            GetComponent<SpecialOfferContent>().SetSaleText(saleText);
        }
    }
}