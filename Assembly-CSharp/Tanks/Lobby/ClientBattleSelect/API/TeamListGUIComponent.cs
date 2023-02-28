using System.Collections.Generic;
using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientBattleSelect.API {
    public class TeamListGUIComponent : MonoBehaviour {
        [SerializeField] GameObject joinButton;

        [SerializeField] RectTransform joinButtonContainer;

        [SerializeField] LobbyUserListItemComponent userListItemPrefab;

        [SerializeField] LobbyUserListItemComponent customLobbyuserListItemPrefab;

        [SerializeField] RectTransform scrollViewRect;

        [SerializeField] TextMeshProUGUI membersCount;

        int maxCount = 20;

        bool showSearchingText = true;

        public bool ShowSearchingText {
            get => showSearchingText;
            set {
                showSearchingText = value;
                LobbyUserListItemComponent[] componentsInChildren = scrollViewRect.GetComponentsInChildren<LobbyUserListItemComponent>();

                for (int i = 0; i < componentsInChildren.Length; i++) {
                    componentsInChildren[i].ShowSearchingText = showSearchingText;
                }
            }
        }

        public int MaxCount {
            get => maxCount;
            set {
                maxCount = value;
                UpdateList();
            }
        }

        public bool ShowJoinButton {
            set => joinButton.SetActive(value);
        }

        void OnEnable() {
            UpdateList();
            GetComponentInChildren<ScrollRect>().verticalNormalizedPosition = 1f;
        }

        public void AddUser(Entity userEntity, bool selfUser, bool customLobby) {
            GameObject itemByUserEntity = GetItemByUserEntity(userEntity);

            if (!(itemByUserEntity != null)) {
                InitNewItem(userEntity, selfUser, customLobby).transform.SetSiblingIndex(GetFirstEmptyIndex());
                UpdateList();
            }
        }

        public void RemoveUser(Entity userEntity) {
            GameObject itemByUserEntity = GetItemByUserEntity(userEntity);

            if (!(itemByUserEntity == null)) {
                itemByUserEntity.transform.SetParent(transform);
                Destroy(itemByUserEntity);
                UpdateList();
            }
        }

        void UpdateList() {
            RemoveExcessItems();
            FillEmptyCells();
            UpdateBounds();
            UpdateMembersCount();
            MoveJoinButton(HasEmptyCells());
            SortBySquadGroup();
        }

        void MoveJoinButton(bool interactable) {
            float num = Mathf.Min(scrollViewRect.rect.height, GetComponent<RectTransform>().rect.height) + 20f;
            joinButtonContainer.anchoredPosition = new Vector2(joinButtonContainer.anchoredPosition.x, 0f - num);
            joinButton.GetComponent<Button>().interactable = interactable;
        }

        void SortBySquadGroup() {
            LobbyUserListItemComponent[] componentsInChildren = scrollViewRect.GetComponentsInChildren<LobbyUserListItemComponent>();
            List<long> list = new();

            for (int i = 0; i < componentsInChildren.Length; i++) {
                if (!componentsInChildren[i].Empty && componentsInChildren[i].userEntity.HasComponent<SquadGroupComponent>() &&
                    !list.Contains(componentsInChildren[i].userEntity.GetComponent<SquadGroupComponent>().Key)) {
                    list.Add(componentsInChildren[i].userEntity.GetComponent<SquadGroupComponent>().Key);
                }
            }

            for (int j = 0; j < list.Count; j++) {
                for (int k = 0; k < componentsInChildren.Length; k++) {
                    if (!componentsInChildren[k].Empty && componentsInChildren[k].userEntity.HasComponent<SquadGroupComponent>() &&
                        componentsInChildren[k].userEntity.GetComponent<SquadGroupComponent>().Key == list[j]) {
                        componentsInChildren[k].transform.SetSiblingIndex(0);
                    }
                }
            }
        }

        public void Clean() {
            LobbyUserListItemComponent[] componentsInChildren = scrollViewRect.GetComponentsInChildren<LobbyUserListItemComponent>();

            for (int i = 0; i < componentsInChildren.Length; i++) {
                componentsInChildren[i].transform.SetParent(transform);
                Destroy(componentsInChildren[i].gameObject);
            }
        }

        GameObject InitNewItem(Entity userEntity, bool selfUser, bool customLobby) {
            LobbyUserListItemComponent component = Instantiate(!customLobby && !selfUser ? userListItemPrefab : customLobbyuserListItemPrefab).GetComponent<LobbyUserListItemComponent>();
            component.transform.SetParent(scrollViewRect, false);
            component.userEntity = userEntity;
            component.selfUser = selfUser;
            component.ShowSearchingText = ShowSearchingText;
            component.gameObject.SetActive(true);
            return component.gameObject;
        }

        public GameObject GetItemByUserEntity(Entity userEntity) {
            LobbyUserListItemComponent[] componentsInChildren = scrollViewRect.GetComponentsInChildren<LobbyUserListItemComponent>();

            for (int i = 0; i < componentsInChildren.Length; i++) {
                if (!componentsInChildren[i].Empty && componentsInChildren[i].userEntity.Equals(userEntity)) {
                    return componentsInChildren[i].gameObject;
                }
            }

            return null;
        }

        public bool HasEmptyCells() {
            int num = scrollViewRect.GetComponentsInChildren<LobbyUserListItemComponent>().Count(uli => uli.userInfo.activeSelf);
            return maxCount > num;
        }

        int GetFirstEmptyIndex() {
            LobbyUserListItemComponent[] componentsInChildren = scrollViewRect.GetComponentsInChildren<LobbyUserListItemComponent>();

            for (int i = 0; i < componentsInChildren.Length; i++) {
                if (componentsInChildren[i].Empty) {
                    return i;
                }
            }

            return componentsInChildren.Length;
        }

        void FillEmptyCells() {
            LobbyUserListItemComponent[] componentsInChildren = scrollViewRect.GetComponentsInChildren<LobbyUserListItemComponent>();
            int num = maxCount - componentsInChildren.Length;

            for (int i = 0; i < num; i++) {
                InitNewItem(null, false, false);
            }
        }

        void RemoveExcessItems() {
            LobbyUserListItemComponent[] componentsInChildren = scrollViewRect.GetComponentsInChildren<LobbyUserListItemComponent>();

            for (int i = maxCount; i < componentsInChildren.Length; i++) {
                componentsInChildren[i].transform.SetParent(transform);
                Destroy(componentsInChildren[i].gameObject);
            }
        }

        int GetUsersCount() {
            LobbyUserListItemComponent[] componentsInChildren = scrollViewRect.GetComponentsInChildren<LobbyUserListItemComponent>();

            for (int i = 0; i < componentsInChildren.Length; i++) {
                if (componentsInChildren[i].Empty) {
                    return i;
                }
            }

            return componentsInChildren.Length;
        }

        void UpdateMembersCount() {
            membersCount.text = GetUsersCount() + "/" + maxCount;
        }

        void UpdateBounds() {
            float y = (userListItemPrefab.GetComponent<LayoutElement>().preferredHeight + scrollViewRect.GetComponent<VerticalLayoutGroup>().spacing) * scrollViewRect.childCount;
            scrollViewRect.sizeDelta = new Vector2(scrollViewRect.sizeDelta.x, y);
        }
    }
}