using System.Collections.Generic;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientResources.API;
using Platform.System.Data.Statics.ClientConfigurator.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientBattleSelect.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientFriends.Impl;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl;
using UnityEngine;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class MatchLobbyGUISystem : ECSSystem {
        [Inject] public static GarageItemsRegistry GarageItemsRegistry { get; set; }

        [Inject] public static ConfigurationService ConfiguratorService { get; set; }

        [OnEventFire]
        public void OnMainScreen(NodeAddedEvent e, SingleNode<MainScreenComponent> screen, [JoinAll] [Context] SelfUserNode selfUser) {
            int needGoodBattles = selfUser.battleLeaveCounter.NeedGoodBattles;

            if (needGoodBattles > 0) {
                MainScreenComponent.Instance.ShowDesertIcon(needGoodBattles);
            } else {
                MainScreenComponent.Instance.HideDeserterIcon();
            }
        }

        [OnEventFire]
        public void OnLobbyScreen(NodeAddedEvent e, SingleNode<MatchLobbyGUIComponent> gameModeSelectScreen, [JoinAll] SelfUserNode selfUser,
            [JoinAll] Optional<SingleNode<CustomBattleLobbyComponent>> customBattleLobby) {
            if (customBattleLobby.IsPresent()) {
                MainScreenComponent.Instance.HideDeserterDesc();
            } else {
                CheckForDeserterDesc(selfUser);
            }
        }

        void CheckForDeserterDesc(SelfUserNode selfUser) {
            int needGoodBattles = selfUser.battleLeaveCounter.NeedGoodBattles;

            if (needGoodBattles > 0) {
                MainScreenComponent.Instance.ShowDeserterDesc(needGoodBattles, true);
            } else {
                MainScreenComponent.Instance.HideDeserterDesc();
            }
        }

        [OnEventFire]
        public void LobbyRemove(NodeRemoveEvent e, SingleNode<MatchLobbyGUIComponent> gameModeSelectScreen) {
            MainScreenComponent.Instance.HideDeserterDesc();
        }

        [OnEventFire]
        public void InitUI(NodeAddedEvent e, SingleNode<MatchLobbyGUIComponent> ui, LobbyNode lobby, [JoinByMap] MapNode map) {
            GameModesDescriptionData gameModesDescriptionData = ConfiguratorService.GetConfig("localization/battle_mode").ConvertTo<GameModesDescriptionData>();
            ui.component.SetTeamBattleMode(lobby.battleMode.BattleMode != BattleMode.DM, lobby.userLimit.TeamLimit, lobby.userLimit.UserLimit);
            ui.component.ModeName = gameModesDescriptionData.battleModeLocalization[lobby.battleMode.BattleMode];
            ui.component.MapName = map.descriptionItem.Name;
            ui.component.ShowSearchingText = !lobby.Entity.HasComponent<CustomBattleLobbyComponent>();

            if (!map.Entity.HasComponent<MapPreviewDataComponent>()) {
                AssetRequestEvent assetRequestEvent = new();
                assetRequestEvent.Init<MapPreviewDataComponent>(map.mapPreview.AssetGuid);
                ScheduleEvent(assetRequestEvent, map);
            } else {
                ui.component.SetMapPreview((Texture2D)map.Entity.GetComponent<MapPreviewDataComponent>().Data);
            }

            if (ui.Entity.HasComponent<BattleLobbyGroupComponent>()) {
                ui.Entity.GetComponent<BattleLobbyGroupComponent>().Detach(ui.Entity);
            }

            lobby.battleLobbyGroup.Attach(ui.Entity);
            ui.component.paramGravity.text = ConfiguratorService.GetConfig("localization/gravity_type").ConvertTo<GravityTypeNames>().Names[lobby.gravity.GravityType];
        }

        [OnEventFire]
        public void DeinitUI(NodeRemoveEvent e, LobbyNode lobby, SingleNode<MatchLobbyGUIComponent> ui) {
            if (lobby.Entity.HasComponent<BattleLobbyGroupComponent>() && ui.Entity.HasComponent<BattleLobbyGroupComponent>()) {
                lobby.battleLobbyGroup.Detach(ui.Entity);
            }
        }

        [OnEventFire]
        public void MatchLobbyGUIAdded(NodeAddedEvent e, LobbyUINode matchLobbyGUI, [Combine] [JoinByBattleLobby] [Context] UserNode userNode,
            [JoinByBattleLobby] Optional<CustomLobbyNode> customLobby, [JoinAll] SelfBattleLobbyUser selfBattleLobbyUser) {
            bool selfUser = userNode.userGroup.Key == selfBattleLobbyUser.userGroup.Key;

            bool flag = userNode.Entity.HasComponent<SquadGroupComponent>() && selfBattleLobbyUser.Entity.HasComponent<SquadGroupComponent>() &&
                        userNode.Entity.GetComponent<SquadGroupComponent>().Key == selfBattleLobbyUser.Entity.GetComponent<SquadGroupComponent>().Key;

            matchLobbyGUI.matchLobbyGUI.AddUser(userNode.Entity, selfUser, flag || customLobby.IsPresent());
        }

        [OnEventFire]
        public void MatchLobbyGUIRemoved(NodeRemoveEvent e, LobbyUINode ui, [Combine] [JoinByBattleLobby] [Context] UserNode userNode) {
            if (userNode.Entity.HasComponent<LobbyUserListItemComponent>()) {
                userNode.Entity.RemoveComponent<LobbyUserListItemComponent>();
            }

            ui.matchLobbyGUI.RemoveUser(userNode.Entity);
        }

        [OnEventFire]
        public void SetPreview(NodeAddedEvent e, MapWithPreviewDataNode map, [JoinAll] SingleNode<MatchLobbyGUIComponent> ui) {
            ui.component.SetMapPreview((Texture2D)map.mapPreviewData.Data);
        }

        [OnEventFire]
        public void InitHullForBattleSelectScreen(NodeAddedEvent e, SingleNode<MatchLobbyGUIComponent> ui, HullNode hull, [Context] [JoinByMarketItem] MarketItemNode marketItem) {
            ui.component.Hull = GarageItemsRegistry.GetItem<TankPartItem>(marketItem.Entity);
            ui.component.SetHullLabels();
        }

        [OnEventFire]
        public void InitTurretForBattleSelectScreen(NodeAddedEvent e, SingleNode<MatchLobbyGUIComponent> ui, TurretNode turret, [Context] [JoinByMarketItem] MarketItemNode marketItem) {
            ui.component.Turret = GarageItemsRegistry.GetItem<TankPartItem>(marketItem.Entity);
            ui.component.SetTurretLabels();
        }

        [OnEventFire]
        public void MatchLobbyGUIShowed(MatchLobbyShowEvent e, SingleNode<MatchLobbyGUIComponent> ui, [JoinAll] SingleNode<SelfUserComponent> selfUser,
            [JoinAll] ICollection<PresetNode> presetsList) {
            List<PresetItem> list = new();

            foreach (PresetNode presets in presetsList) {
                if (presets.presetEquipment.HullId == 0) { }

                if (presets.presetEquipment.WeaponId == 0) { }

                if (presets.presetEquipment.HullId != 0 && presets.presetEquipment.WeaponId != 0) {
                    Entity entityById = GetEntityById(presets.presetEquipment.HullId);
                    Entity entityById2 = GetEntityById(presets.presetEquipment.WeaponId);
                    string name = entityById.GetComponent<DescriptionItemComponent>().Name;
                    string name2 = entityById2.GetComponent<DescriptionItemComponent>().Name;
                    long key = entityById.GetComponent<MarketItemGroupComponent>().Key;
                    long key2 = entityById2.GetComponent<MarketItemGroupComponent>().Key;
                    string presetName = GetPresetName(presets);
                    list.Add(new PresetItem(presetName, 1, name, name2, key, key2, presets.Entity));
                }
            }

            list.Sort(new PresetItemComparer());
            ui.component.InitPresetsDropDown(list);
        }

        string GetPresetName(PresetNode preset) {
            GetPresetNameEvent getPresetNameEvent = new();
            ScheduleEvent(getPresetNameEvent, preset);
            return getPresetNameEvent.Name;
        }

        [OnEventFire]
        public void UserItemAdded(NodeAddedEvent e, SingleNode<LobbyUserListItemComponent> userUI, [JoinByUser] Optional<CustomLobbyNode> lobby) {
            bool master = lobby.IsPresent();
            userUI.component.Master = master;
        }

        [OnEventFire]
        public void LobbyMasterAdded(NodeAddedEvent e, CustomLobbyNode lobbyWithMaster, [JoinByUser] SingleNode<LobbyUserListItemComponent> userUI) {
            userUI.component.Master = true;
        }

        [OnEventFire]
        public void LobbyMasterRemoved(NodeRemoveEvent e, CustomLobbyNode lobbyWithMaster, [JoinByUser] SingleNode<LobbyUserListItemComponent> userUI) {
            userUI.component.Master = false;
        }

        [OnEventFire]
        public void ItemMounted(NodeAddedEvent e, SingleNode<MountedItemComponent> node, [JoinAll] SingleNode<MatchLobbyGUIComponent> ui) {
            ui.component.InitHullDropDowns();
            ui.component.InitTurretDropDowns();
        }

        [OnEventFire]
        public void UserEquipment(NodeAddedEvent e, UserEquipmentNode userEquipment, [JoinAll] SingleNode<MatchLobbyGUIComponent> ui) {
            UpdateEquipment(userEquipment.lobbyUserListItem, userEquipment.userEquipment);
        }

        void UpdateEquipment(LobbyUserListItemComponent uiItem, UserEquipmentComponent userEquipment) {
            Entity entityById = GetEntityById(userEquipment.WeaponId);
            Entity entityById2 = GetEntityById(userEquipment.HullId);
            TankPartItem item = GarageItemsRegistry.GetItem<TankPartItem>(entityById2);
            TankPartItem item2 = GarageItemsRegistry.GetItem<TankPartItem>(entityById);
            uiItem.UpdateEquipment(item.Name, userEquipment.HullId, item2.Name, userEquipment.WeaponId);
        }

        [OnEventFire]
        public void SetSelfUserTeamColor(NodeAddedEvent e, SelfBattleLobbyUser selfUserNode, [JoinByBattleLobby] LobbyNode lobby, [JoinAll] [Context] LobbyUINode ui) {
            if (lobby.Entity.HasComponent<CustomBattleLobbyComponent>()) {
                ui.matchLobbyGUI.UserTeamColor = selfUserNode.teamColor.TeamColor;
            } else {
                ui.matchLobbyGUI.UserTeamColor = TeamColor.NONE;
            }
        }

        [OnEventFire]
        public void CheckForShowSpectatorModeButton(CheckForSpectatorButtonShowEvent e, Node any, [JoinAll] Optional<SelfBattleLobbyUser> selfLobbyUserNode,
            [JoinAll] Optional<SelfSquadUser> selfSquadUser) {
            e.CanGoToSpectatorMode = !selfLobbyUserNode.IsPresent() && !selfSquadUser.IsPresent();
        }

        public class HullNode : Node {
            public HangarItemPreviewComponent hangarItemPreview;

            public MarketItemGroupComponent marketItemGroup;

            public TankItemComponent tankItem;
        }

        public class TurretNode : Node {
            public HangarItemPreviewComponent hangarItemPreview;

            public MarketItemGroupComponent marketItemGroup;

            public WeaponItemComponent weaponItem;
        }

        public class PresetNode : Node {
            public PresetEquipmentComponent presetEquipment;
            public PresetItemComponent presetItem;

            public PresetNameComponent presetName;

            public UserItemComponent userItem;
        }

        public class MarketItemNode : Node {
            public MarketItemComponent marketItem;
            public MarketItemGroupComponent marketItemGroup;
        }

        public class UserNode : Node {
            public BattleLeaveCounterComponent battleLeaveCounter;

            public BattleLobbyGroupComponent battleLobbyGroup;

            public TeamColorComponent teamColor;
            public UserComponent user;

            public UserGroupComponent userGroup;

            public UserUidComponent userUid;
        }

        public class SelfBattleLobbyUser : UserNode {
            public SelfUserComponent selfUser;
        }

        public class SelfSquadUser : Node {
            public SelfUserComponent selfUser;

            public SquadGroupComponent squadGroup;

            public UserGroupComponent userGroup;
        }

        public class SelfUserNode : Node {
            public BattleLeaveCounterComponent battleLeaveCounter;

            public SelfUserComponent selfUser;
            public UserComponent user;
        }

        public class UserEquipmentNode : UserNode {
            public LobbyUserListItemComponent lobbyUserListItem;
            public UserEquipmentComponent userEquipment;
        }

        public class LobbyNode : Node {
            public BattleLobbyComponent battleLobby;

            public BattleLobbyGroupComponent battleLobbyGroup;

            public BattleModeComponent battleMode;

            public GravityComponent gravity;

            public MapGroupComponent mapGroup;

            public UserLimitComponent userLimit;
        }

        public class CustomLobbyNode : Node {
            public CustomBattleLobbyComponent customBattleLobby;

            public UserGroupComponent userGroup;
        }

        public class MapNode : Node {
            public DescriptionItemComponent descriptionItem;
            public MapComponent map;

            public MapGroupComponent mapGroup;

            public MapPreviewComponent mapPreview;
        }

        public class MapWithPreviewDataNode : MapNode {
            public MapPreviewDataComponent mapPreviewData;
        }

        public class LobbyUINode : Node {
            public BattleLobbyGroupComponent battleLobbyGroup;
            public MatchLobbyGUIComponent matchLobbyGUI;
        }

        class PresetItemComparer : IComparer<PresetItem> {
            public int Compare(PresetItem p1, PresetItem p2) => PresetSystem.comparer.Compare(p1.presetEntity, p2.presetEntity);
        }
    }
}