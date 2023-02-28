using System.Collections.Generic;

namespace Tanks.Lobby.ClientControls.API {
    public class DefaultDropDownList : DropDownListComponent {
        public DefaultListDataProvider DataProvider => dataProvider;

        public void UpdateList(List<string> items, int index = 0) {
            dataProvider.ClearItems();
            string text = items[index];
            dataProvider.Init(items, text);
            listTitle.text = text;
        }

        protected override void OnItemSelect(ListItem item) {
            base.OnItemSelect(item);
            listTitle.text = item.Data as string;
        }
    }
}