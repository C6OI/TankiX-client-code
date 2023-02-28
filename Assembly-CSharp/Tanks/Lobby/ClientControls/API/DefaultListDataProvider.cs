using System;
using System.Collections.Generic;
using Tanks.Lobby.ClientControls.API.List;
using UnityEngine;

namespace Tanks.Lobby.ClientControls.API {
    public class DefaultListDataProvider : MonoBehaviour, ListDataProvider, IUIList {
        [SerializeField] bool clearOnDisable = true;

        protected readonly List<object> dataStorage = new();

        object selected;

        protected virtual void OnDisable() {
            if (clearOnDisable) {
                ClearItems();
            }
        }

        public virtual void AddItem(object data) {
            dataStorage.Add(data);
            SendChanged();
        }

        public virtual void RemoveItem(object data) {
            dataStorage.Remove(data);
            SendChanged();
        }

        public virtual void ClearItems() {
            selected = null;
            dataStorage.Clear();
            SendChanged();
        }

        public virtual IList<object> Data => dataStorage;

        public object Selected {
            get => selected;
            set {
                if (selected != value) {
                    selected = value;
                    SendChanged();
                }
            }
        }

        public event Action<ListDataProvider> DataChanged;

        public void SelectPrev() {
            if (dataStorage.Count != 0) {
                int num = dataStorage.IndexOf(selected);

                if (num >= 0) {
                    Selected = dataStorage[Mathf.Max(num - 1, 0)];
                } else {
                    Selected = dataStorage[0];
                }
            }
        }

        public void SelectNext() {
            if (dataStorage.Count != 0) {
                int num = dataStorage.IndexOf(selected);

                if (num >= 0) {
                    Selected = dataStorage[Mathf.Min(num + 1, dataStorage.Count - 1)];
                } else {
                    Selected = dataStorage[0];
                }
            }
        }

        public void Init<T>(ICollection<T> data) {
            foreach (T datum in data) {
                dataStorage.Add(datum);
            }

            SendChanged();
        }

        public void Init<T>(ICollection<T> data, T selected) {
            foreach (T datum in data) {
                dataStorage.Add(datum);
            }

            this.selected = selected;
            SendChanged();
        }

        void OnItemSelect(ListItem listItem) {
            selected = listItem.Data;
        }

        public void SendChanged() {
            if (DataChanged != null) {
                DataChanged(this);
            }
        }
    }
}