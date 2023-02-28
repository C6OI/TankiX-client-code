using System.Collections.Generic;
using UnityEngine;

namespace Tanks.Lobby.ClientControls.API {
    public class GroupSelector : MonoBehaviour {
        readonly List<SimpleSelectableComponent> items = new();

        public void Add(SimpleSelectableComponent item) {
            items.Add(item);
            item.AddHandler(Select);
            item.AddDestroyHandler(Remove);
        }

        public void Remove(SimpleSelectableComponent item) {
            items.Remove(item);
        }

        public void Select(SimpleSelectableComponent item, bool selected) {
            if (!selected) {
                return;
            }

            items.ForEach(delegate(SimpleSelectableComponent x) {
                if (x != item && x != null && x.gameObject != null) {
                    x.Select(false);
                }
            });
        }
    }
}