using System.Collections.Generic;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class VisualItemsDropDownList : DropDownListComponent {
        public void UpdateList(List<VisualItem> items) {
            if (items.Count == 0) {
                return;
            }

            dataProvider.ClearItems();
            VisualItem visualItem = null;

            foreach (VisualItem item in items) {
                if (item.IsSelected) {
                    visualItem = item;
                }
            }

            listTitle.text = visualItem == null ? "None" : visualItem.Name;
            dataProvider.Init(items, visualItem);
        }

        protected override void OnItemSelect(ListItem item) {
            base.OnItemSelect(item);
            VisualItem visualItem = (VisualItem)item.Data;
            listTitle.text = visualItem.Name;
        }
    }
}