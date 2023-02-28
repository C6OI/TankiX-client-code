using System.Collections.Generic;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Lobby.ClientMatchMaking.Impl {
    public class UsersEnergyCellsListUIComponent : BehaviourComponent {
        [SerializeField] RectTransform content;

        [SerializeField] UserEnergyCellUIComponent cell;

        readonly List<UserEnergyCellUIComponent> cells = new();

        void OnDisable() {
            content.DestroyChildren();
            cells.Clear();
        }

        public UserEnergyCellUIComponent AddUserCell() {
            UserEnergyCellUIComponent userEnergyCellUIComponent = Instantiate(cell);
            userEnergyCellUIComponent.transform.SetParent(content, false);
            userEnergyCellUIComponent.gameObject.SetActive(true);
            cells.Add(userEnergyCellUIComponent);
            UpdateCells();
            return userEnergyCellUIComponent;
        }

        public void RemoveUserCell(UserEnergyCellUIComponent user) {
            if (cells.Contains(user)) {
                cells.Remove(user);
            }

            Destroy(user.gameObject);
            UpdateCells();
        }

        void UpdateCells() {
            for (int i = 0; i < cells.Count; i++) {
                cells[i].CellIsFirst = i == 0;
            }
        }
    }
}