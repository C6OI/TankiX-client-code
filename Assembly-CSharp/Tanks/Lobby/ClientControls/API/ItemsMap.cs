using System.Collections;
using System.Collections.Generic;

namespace Tanks.Lobby.ClientControls.API {
    public class ItemsMap : IEnumerable<ListItem>, IEnumerable {
        readonly List<ListItem> items = new();

        readonly Dictionary<object, ListItem> map = new();

        public int Count => items.Count;

        public ListItem this[object entity] => map[entity];

        public IEnumerator<ListItem> GetEnumerator() => items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => items.GetEnumerator();

        public void Add(ListItem item) {
            items.Add(item);
            map.Add(item.Data, item);
        }

        public void Clear() {
            items.Clear();
            map.Clear();
        }

        public bool Contains(object entity) => map.ContainsKey(entity);

        public bool Remove(object entity) {
            if (map.ContainsKey(entity)) {
                ListItem listItem = map[entity];

                if (listItem != null) {
                    items.Remove(listItem);
                }
            }

            return map.Remove(entity);
        }

        public void Sort(IComparer<ListItem> comparer) {
            items.Sort(comparer);
        }
    }
}