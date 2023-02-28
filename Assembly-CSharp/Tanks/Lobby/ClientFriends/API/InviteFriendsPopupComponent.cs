using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientFriends.Impl;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientFriends.API {
    public class InviteFriendsPopupComponent : UIBehaviour, Component, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler {
        [SerializeField] UITableViewCell inviteToLobbyCell;

        [SerializeField] UITableViewCell inviteToSquadCell;

        [SerializeField] InviteMode currentInviteMode;

        [SerializeField] InviteFriendsUIComponent inviteFriends;

        [SerializeField] TextMeshProUGUI inviteHeader;

        [SerializeField] LocalizedField inviteToLobbyLocalizationFiled;

        [SerializeField] LocalizedField inviteToSquadLocalizationFiled;

        bool pointerIn;

        public InviteMode InviteMode {
            set {
                switch (value) {
                    case InviteMode.Lobby:
                        inviteFriends.tableView.CellPrefab = inviteToLobbyCell;
                        break;

                    case InviteMode.Squad:
                        inviteFriends.tableView.CellPrefab = inviteToSquadCell;
                        break;
                }
            }
        }

        void Update() {
            if (!pointerIn && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))) {
                Close();
            }
        }

        new void OnDisable() {
            pointerIn = false;
        }

        public void OnPointerEnter(PointerEventData eventData) {
            pointerIn = true;
        }

        public void OnPointerExit(PointerEventData eventData) {
            pointerIn = false;
        }

        public void ShowInvite(Vector3 popupPosition, Vector2 pivot, InviteMode inviteMode) {
            InviteMode = inviteMode;
            inviteHeader.text = inviteMode != 0 ? inviteToSquadLocalizationFiled.Value : inviteToLobbyLocalizationFiled.Value;
            GetComponent<RectTransform>().pivot = pivot;
            GetComponent<RectTransform>().position = popupPosition;
            inviteFriends.GetComponent<RectTransform>().pivot = pivot;
            inviteFriends.GetComponent<RectTransform>().position = popupPosition;
            inviteFriends.Show();
        }

        public void Close() {
            inviteFriends.Hide();
        }
    }
}