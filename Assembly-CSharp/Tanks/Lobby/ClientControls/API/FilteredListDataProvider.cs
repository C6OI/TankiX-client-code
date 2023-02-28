using System;
using System.Collections.Generic;

namespace Tanks.Lobby.ClientControls.API {
    public class FilteredListDataProvider : DefaultListDataProvider {
        List<object> filteredData;

        public override IList<object> Data {
            get {
                if (filteredData == null) {
                    return dataStorage;
                }

                return filteredData;
            }
        }

        public void ApplyFilter(Func<object, bool> IsFiltered) {
            if (filteredData == null) {
                filteredData = new List<object>();
            }

            filteredData.Clear();

            foreach (object item in dataStorage) {
                if (!IsFiltered(item)) {
                    filteredData.Add(item);
                }
            }

            SendChanged();
        }

        public override void ClearItems() {
            base.ClearItems();

            if (filteredData != null) {
                filteredData.Clear();
                filteredData = null;
            }
        }
    }
}