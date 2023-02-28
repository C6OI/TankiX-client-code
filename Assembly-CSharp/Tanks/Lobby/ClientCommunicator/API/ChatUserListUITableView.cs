using System.Collections.Generic;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientUserProfile.API;

namespace Tanks.Lobby.ClientCommunicator.API {
    public class ChatUserListUITableView : UITableView {
        List<UserCellData> filteredItems;

        string filterString = string.Empty;
        List<UserCellData> items;

        public List<UserCellData> Items {
            get => items ?? (items = new List<UserCellData>());
            set => items = value;
        }

        public List<UserCellData> FilteredItems {
            get => filteredItems ?? (filteredItems = new List<UserCellData>());
            set => filteredItems = value;
        }

        public string FilterString {
            get => filterString;
            set {
                filterString = value;
                FilteredItems = new List<UserCellData>();

                foreach (UserCellData item in Items) {
                    if (string.IsNullOrEmpty(value) || item.uid.ToLower().Contains(filterString.ToLower())) {
                        FilteredItems.Add(item);
                    }
                }

                UpdateTable();
            }
        }

        protected override void OnDisable() {
            base.OnDisable();
            Items.Clear();
            FilterString = string.Empty;
        }

        protected override int NumberOfRows() => FilteredItems.Count;

        protected override UITableViewCell CellForRowAtIndex(int index) {
            UITableViewCell uITableViewCell = base.CellForRowAtIndex(index);

            if (uITableViewCell != null) {
                FriendsUITableViewCell friendsUITableViewCell = (FriendsUITableViewCell)uITableViewCell;
                friendsUITableViewCell.Init(FilteredItems[index].id, Items.Count > 50);
            }

            return uITableViewCell;
        }

        public void RemoveUser(long userId, bool toRight) {
            for (int i = 0; i < Items.Count; i++) {
                if (Items[i].id == userId) {
                    UserCellData item = Items[i];
                    Items.Remove(item);

                    if (FilteredItems.Contains(item)) {
                        int index = FilteredItems.IndexOf(item);
                        FilteredItems.RemoveAt(index);
                        RemoveCell(index, toRight);
                    }
                }
            }
        }
    }
}