using System;
using System.Collections.Generic;
using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientBattleSelect.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class MatchLobbyGUIComponent : EventMappingComponent {
        [SerializeField] GameObject teamList1Title;

        [SerializeField] TextMeshProUGUI gameModeTitle;

        [SerializeField] Image mapIcon;

        [SerializeField] TextMeshProUGUI mapTitle;

        [SerializeField] PresetsDropDownList presetsDropDownList;

        [SerializeField] VisualItemsDropDownList hullSkinsDropDownList;

        [SerializeField] VisualItemsDropDownList hullPaintDropDownList;

        [SerializeField] VisualItemsDropDownList turretSkinsDropDownList;

        [SerializeField] VisualItemsDropDownList turretPaintDropDownList;

        [SerializeField] RectTransform blueTeamListWithHeader;

        [SerializeField] RectTransform redTeamListWithHeader;

        [SerializeField] TeamListGUIComponent blueTeamList;

        [SerializeField] TeamListGUIComponent redTeamList;

        [SerializeField] TextMeshProUGUI hullName;

        [SerializeField] TextMeshProUGUI turretName;

        [SerializeField] TextMeshProUGUI hullFeature;

        [SerializeField] TextMeshProUGUI turretFeature;

        [SerializeField] GameObject customGameElements;

        [SerializeField] GameObject openBattleButton;

        [SerializeField] GameObject inviteFriendsButton;

        [SerializeField] GameObject editParamsButton;

        public TextMeshProUGUI paramTimeLimit;

        public TextMeshProUGUI paramFriendlyFire;

        public TextMeshProUGUI paramGravity;

        public TextMeshProUGUI enabledModules;

        public CreateBattleScreenComponent createBattleScreen;

        public GameObject chat;
        bool teamBattleMode;

        [Inject] public static GarageItemsRegistry GarageItemsRegistry { get; set; }

        public TankPartItem Hull { get; set; }

        public TankPartItem Turret { get; set; }

        public bool ShowSearchingText {
            set {
                blueTeamList.ShowSearchingText = value;
                redTeamList.ShowSearchingText = value;
            }
        }

        public string MapName {
            set => mapTitle.text = value;
        }

        public string ModeName {
            set => gameModeTitle.text = value;
        }

        public TeamColor UserTeamColor {
            set {
                blueTeamList.ShowJoinButton = value == TeamColor.RED;
                redTeamList.ShowJoinButton = value == TeamColor.BLUE;
            }
        }

        void OnEnable() {
            PresetsDropDownList obj = presetsDropDownList;
            obj.onDropDownListItemSelected = (OnDropDownListItemSelected)Delegate.Combine(obj.onDropDownListItemSelected, new OnDropDownListItemSelected(OnPresetSelected));
            SendEvent<MatchLobbyShowEvent>();
        }

        void OnDisable() {
            PresetsDropDownList obj = presetsDropDownList;
            obj.onDropDownListItemSelected = (OnDropDownListItemSelected)Delegate.Remove(obj.onDropDownListItemSelected, new OnDropDownListItemSelected(OnPresetSelected));
        }

        public void SetTeamBattleMode(bool teamBattleMode, int teamLimit, int userLimit) {
            this.teamBattleMode = teamBattleMode;
            redTeamListWithHeader.gameObject.SetActive(teamBattleMode);
            teamList1Title.SetActive(teamBattleMode);

            if (teamBattleMode) {
                blueTeamList.MaxCount = teamLimit;
                redTeamList.MaxCount = teamLimit;
            } else {
                blueTeamList.MaxCount = userLimit;
            }
        }

        public void SetMapPreview(Texture2D image) {
            mapIcon.color = Color.white;
            mapIcon.sprite = Sprite.Create(image, new Rect(Vector2.zero, new Vector2(image.width, image.height)), Vector2.one * 0.5f);
        }

        public void ShowCustomLobbyElements(bool show) {
            customGameElements.SetActive(show);
        }

        public void ShowEditParamsButton(bool show, bool interactable) {
            editParamsButton.SetActive(show);
            editParamsButton.GetComponent<Button>().interactable = interactable;
            openBattleButton.SetActive(show);
            inviteFriendsButton.SetActive(show);
        }

        public void ShowChat(bool show) {
            chat.SetActive(show);
        }

        public void InitPresetsDropDown(List<PresetItem> items) {
            presetsDropDownList.UpdateList(items);
        }

        public void InitHullDropDowns() {
            List<VisualItem> items = FilterItemsList(Hull.Skins.ToList());
            List<VisualItem> items2 = FilterItemsList(GarageItemsRegistry.Paints.ToList());
            hullSkinsDropDownList.UpdateList(items);
            hullPaintDropDownList.UpdateList(items2);
        }

        public void InitTurretDropDowns() {
            List<VisualItem> items = FilterItemsList(Turret.Skins.ToList());
            List<VisualItem> items2 = FilterItemsList(GarageItemsRegistry.Coatings.ToList());
            turretSkinsDropDownList.UpdateList(items);
            turretPaintDropDownList.UpdateList(items2);
        }

        List<VisualItem> FilterItemsList(List<VisualItem> items) {
            List<VisualItem> list = new();

            foreach (VisualItem item in items) {
                if (item.UserItem != null && !item.WaitForBuy) {
                    list.Add(item);
                }
            }

            return list;
        }

        public void AddUser(Entity userEntity, bool selfUser, bool customLobby) {
            TeamColor teamColor = userEntity.GetComponent<TeamColorComponent>().TeamColor;

            if (teamBattleMode) {
                if (teamColor == TeamColor.RED) {
                    AddUserToSecondList(userEntity, selfUser, customLobby);
                } else {
                    AddUserToFirstList(userEntity, selfUser, customLobby);
                }
            } else {
                AddUserToFirstList(userEntity, selfUser, customLobby);
            }

            UpdateInviteFriendsButton();
        }

        void AddUserToFirstList(Entity userEntity, bool selfUser, bool customLobby) {
            redTeamList.RemoveUser(userEntity);
            blueTeamList.AddUser(userEntity, selfUser, customLobby);
        }

        void AddUserToSecondList(Entity userEntity, bool selfUser, bool customLobby) {
            blueTeamList.RemoveUser(userEntity);
            redTeamList.AddUser(userEntity, selfUser, customLobby);
        }

        public void RemoveUser(Entity userEntity) {
            blueTeamList.RemoveUser(userEntity);
            redTeamList.RemoveUser(userEntity);
            UpdateInviteFriendsButton();
        }

        public void CleanUsersList() {
            blueTeamList.Clean();
            redTeamList.Clean();
            UpdateInviteFriendsButton();
        }

        void UpdateInviteFriendsButton() {
            bool interactable = !teamBattleMode ? blueTeamList.HasEmptyCells() : blueTeamList.HasEmptyCells() || redTeamList.HasEmptyCells();
            inviteFriendsButton.GetComponent<Button>().interactable = interactable;
        }

        protected override void Subscribe() { }

        public void OnPresetSelected(ListItem item) {
            PresetItem presetItem = (PresetItem)item.Data;
            Mount(presetItem.presetEntity);
        }

        public void OnVisualItemSelected(ListItem item) {
            VisualItem visualItem = (VisualItem)item.Data;
            Mount(visualItem.UserItem);
        }

        void Mount(Entity target) {
            EngineService.Engine.ScheduleEvent<MountPresetEvent>(target);
        }

        public void SetHullLabels() {
            hullName.text = Hull.Name;
            hullFeature.text = Hull.Feature;
        }

        public void SetTurretLabels() {
            turretName.text = Turret.Name;
            turretFeature.text = Turret.Feature;
        }
    }
}