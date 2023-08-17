using System;
using System.Collections.Generic;
using Lobby.ClientControls.API.List;
using UnityEngine;

namespace Lobby.ClientControls.API {
    public class DefaultListDataProvider : MonoBehaviour, IUIList, ListDataProvider {
        protected readonly List<object> dataStorage = new();

        void OnDisable() => ClearItems();

        public void AddItem(object data) {
            dataStorage.Add(data);
            SendChanged();
        }

        public void RemoveItem(object data) {
            dataStorage.Remove(data);
            SendChanged();
        }

        public virtual void ClearItems() {
            dataStorage.Clear();
            SendChanged();
        }

        public virtual IList<object> Data => dataStorage;

        public event Action<ListDataProvider> DataChanged;

        protected void SendChanged() {
            if (DataChanged != null) {
                DataChanged(this);
            }
        }
    }
}