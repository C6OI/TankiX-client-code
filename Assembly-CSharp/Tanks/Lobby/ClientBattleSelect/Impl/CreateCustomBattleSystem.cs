using System;
using System.Collections.Generic;
using System.Linq;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Exchange.ClientNetwork.API;
using Platform.System.Data.Statics.ClientConfigurator.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientBattleSelect.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientNavigation.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class CreateCustomBattleSystem : ECSSystem {
        static Entity screen;

        [Inject] public static ConfigurationService ConfiguratorService { get; set; }

        [OnEventFire]
        public void InitScreen(NodeAddedEvent e, ScreenNode screen, [JoinAll] ICollection<MapNode> mapNodes, [JoinAll] Optional<LobbyNode> lobby,
            [JoinAll] SingleNode<SelfUserComponent> user) {
            CreateBattleScreenComponent createBattleScreen = screen.createBattleScreen;
            bool flag = lobby.IsPresent();
            bool flag2 = !flag || lobby.Get().Entity.HasComponent<SelfComponent>();
            bool flag3 = flag && lobby.Get().Entity.HasComponent<BattleGroupComponent>();
            string text = screen.createBattleScreen.offText;
            string text2 = screen.createBattleScreen.onText;

            if (CreateCustomBattleSystem.screen == null) {
                DefaultDropDownList battleModeDropdown = createBattleScreen.battleModeDropdown;

                battleModeDropdown.onDropDownListItemSelected =
                    (OnDropDownListItemSelected)Delegate.Combine(battleModeDropdown.onDropDownListItemSelected, new OnDropDownListItemSelected(OnBattleModeSelected));

                DefaultDropDownList mapDropdown = createBattleScreen.mapDropdown;

                mapDropdown.onDropDownListItemSelected =
                    (OnDropDownListItemSelected)Delegate.Combine(mapDropdown.onDropDownListItemSelected, new OnDropDownListItemSelected(OnMapSelected));

                DefaultDropDownList timeLimitDropdown = createBattleScreen.timeLimitDropdown;

                timeLimitDropdown.onDropDownListItemSelected =
                    (OnDropDownListItemSelected)Delegate.Combine(timeLimitDropdown.onDropDownListItemSelected, new OnDropDownListItemSelected(OnTimeLimitSelected));
            }

            CreateCustomBattleSystem.screen = screen.Entity;
            ClientBattleParams p = !flag ? LoadParams() : lobby.Get().clientBattleParams.Params;
            List<BattleMode> list = ((BattleMode[])Enum.GetValues(typeof(BattleMode))).Where(m => m != BattleMode.CP).ToList();
            Dictionary<BattleMode, string> modeNames = BattleModeLocalizationUtil.GetModeToNameDict();

            foreach (BattleMode item in list) {
                Dictionary<BattleMode, List<string>> modesToMapsDict = createBattleScreen.ModesToMapsDict;

                if (!modesToMapsDict.ContainsKey(item)) {
                    createBattleScreen.ModesToMapsDict.Add(item, new List<string>());
                }

                modesToMapsDict[item].Clear();
            }

            int index = Array.IndexOf(Enum.GetValues(typeof(BattleMode)), p.BattleMode);
            createBattleScreen.battleModeDropdown.UpdateList(list.Select(en => modeNames[en]).ToList(), index);
            List<MapNode> list2 = new();

            foreach (MapNode mapNode2 in mapNodes) {
                if (!user.Entity.HasComponent<UserAdminComponent>() && !user.Entity.HasComponent<UserTesterComponent>() &&
                    !mapNode2.Entity.HasComponent<MapEnabledInCustomGameComponent>()) {
                    continue;
                }

                list2.Add(mapNode2);
                Dictionary<BattleMode, List<string>> modesToMapsDict2 = createBattleScreen.ModesToMapsDict;

                foreach (BattleMode availableMode in mapNode2.mapModeRestriction.AvailableModes) {
                    if (modesToMapsDict2.ContainsKey(availableMode)) {
                        modesToMapsDict2[availableMode].Add(mapNode2.descriptionItem.Name);
                    }
                }
            }

            List<string> list3 = createBattleScreen.ModesToMapsDict[p.BattleMode];
            string text3 = list3.First();
            MapNode mapNode = list2.FirstOrDefault(n => n.Entity.Id == p.MapId);

            if (mapNode != null) {
                text3 = mapNode.descriptionItem.Name;
            }

            int index2 = list3.IndexOf(text3);
            createBattleScreen.mapDropdown.UpdateList(list3, index2);
            int[] array = new int[7] { 1, 5, 10, 15, 30, 60, 90 };
            int num = Array.IndexOf(array, p.TimeLimit);

            if (num < 0) {
                num = 2;
            }

            string minutesText = " " + screen.createBattleScreen.minutesText.Value;
            createBattleScreen.timeLimitDropdown.UpdateList(array.Select(n => n + minutesText).ToList(), num);
            UpdateScoreLimit(createBattleScreen, p.ScoreLimit);
            UpdatePlayerLimit(createBattleScreen, p.ScoreLimit);
            createBattleScreen.maxPlayersDropdown.GetComponent<Button>().interactable = !flag;
            createBattleScreen.friendlyFireDropdown.UpdateList(new string[2] { text, text2 }.Select(n => n).ToList(), p.FriendlyFire ? 1 : 0);
            UpdateFriendlyFire(createBattleScreen);
            Dictionary<GravityType, string> gravityNames = ConfiguratorService.GetConfig("localization/gravity_type").ConvertTo<GravityTypeNames>().Names;
            createBattleScreen.gravityDropdown.UpdateList(((GravityType[])Enum.GetValues(typeof(GravityType))).Select(g => gravityNames[g]).ToList(), (int)p.Gravity);
            createBattleScreen.killZoneDropdown.UpdateList(new string[2] { text, text2 }.Select(n => n).ToList(), p.KillZoneEnabled ? 1 : 0);
            createBattleScreen.enabledModulesDropdown.UpdateList(new string[2] { text, text2 }.Select(n => n).ToList(), !p.DisabledModules ? 1 : 0);
            GroupMapWithScreen(text3);
            createBattleScreen.canvasGroup.interactable = flag2 && !flag3;
            EnableButton(createBattleScreen.updateBattleParamsButton, flag);
            EnableButton(createBattleScreen.createBattleButton, !flag);
        }

        [OnEventFire]
        public void DeinitSelfLobby(NodeRemoveEvent e, LobbyWithUserGroupNode lobby) {
            if (lobby.Entity.HasComponent<SelfComponent>()) {
                lobby.Entity.RemoveComponent<SelfComponent>();
            }
        }

        [OnEventFire]
        public void JoinTeam(ButtonClickEvent e, SingleNode<SwitchTeamButtonComponent> button, [JoinAll] LobbyNode lobby) {
            ScheduleEvent<SwitchTeamEvent>(lobby);
        }

        static void UpdateScoreLimit(CreateBattleScreenComponent component, int savedScoreLimit = 0) {
            BattleMode battleMode = GetBattleMode(component);
            string text = component.noLimitText;
            string[] array = new string[0];

            switch (battleMode) {
                case BattleMode.DM:
                    array = new string[5] { text, "10", "20", "30", "50" };
                    break;

                case BattleMode.TDM:
                    array = new string[5] { text, "10", "20", "30", "50" };
                    break;

                case BattleMode.CTF:
                    array = new string[5] { text, "1", "3", "5", "10" };
                    break;
            }

            int num = component.scoreLimitDropdown.SelectionIndex;

            if (num < 0) {
                num = Array.IndexOf(array, savedScoreLimit.ToString());

                if (num < 0) {
                    num = 0;
                }
            }

            component.scoreLimitDropdown.UpdateList(array.ToList(), num);
        }

        static void UpdatePlayerLimit(CreateBattleScreenComponent component, int savedPlayerLimit = -1) {
            BattleMode battleMode = GetBattleMode(component);

            string[] array = battleMode == BattleMode.DM ? new string[10] { "2", "4", "6", "8", "10", "12", "14", "16", "18", "20" }
                                 : new string[10] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };

            component.maxPlayerText.text = battleMode != 0 ? component.maxTeamPlayerText : component.maxDmPlayerText;
            int num = component.maxPlayersDropdown.SelectionIndex;

            if (num < 0) {
                num = Array.IndexOf(array, savedPlayerLimit.ToString());

                if (num < 0) {
                    num = array.Length - 1;
                }
            }

            component.maxPlayersDropdown.UpdateList(array.ToList(), num);
        }

        static void UpdateMaps(CreateBattleScreenComponent component) {
            BattleMode battleMode = GetBattleMode(component);
            List<string> list = component.ModesToMapsDict[battleMode];
            string text = list.First();
            int index = list.IndexOf(text);
            component.mapDropdown.UpdateList(list, index);
            GroupMapWithScreen(text);
        }

        static void UpdateFriendlyFire(CreateBattleScreenComponent component) {
            BattleMode battleMode = GetBattleMode(component);
            DefaultDropDownList friendlyFireDropdown = component.friendlyFireDropdown;
            Button component2 = friendlyFireDropdown.GetComponent<Button>();
            component2.interactable = battleMode != BattleMode.DM;

            if (!component2.interactable) {
                friendlyFireDropdown.SelectionIndex = 0;
            }
        }

        void EnableButton(Button button, bool enabled) {
            button.interactable = enabled;
            button.gameObject.SetActive(enabled);
        }

        static void OnBattleModeSelected(ListItem item) {
            CreateBattleScreenComponent component = screen.GetComponent<CreateBattleScreenComponent>();
            UpdatePlayerLimit(component);
            UpdateScoreLimit(component);
            UpdateFriendlyFire(component);
            UpdateMaps(component);
        }

        static void OnMapSelected(ListItem item) {
            GroupMapWithScreen((string)item.Data);
        }

        static void OnTimeLimitSelected(ListItem item) {
            UpdateScoreLimit(screen.GetComponent<CreateBattleScreenComponent>());
        }

        static void GroupMapWithScreen(string mapName) {
            if (screen.HasComponent<MapGroupComponent>()) {
                screen.RemoveComponent<MapGroupComponent>();
            }

            NodeClassInstanceDescription orCreateNodeClassDescription = NodeDescriptionRegistry.GetOrCreateNodeClassDescription(typeof(MapNode));
            ICollection<Entity> entities = Flow.Current.NodeCollector.GetEntities(orCreateNodeClassDescription.NodeDescription);
            Entity entity = entities.First(m => m.GetComponent<DescriptionItemComponent>().Name.Equals(mapName));
            screen.AddComponent(new MapGroupComponent(entity.Id));
        }

        [OnEventFire]
        public void RequestMapPreview(NodeAddedEvent e, ScreenWithMapGroupNode screen, [JoinByMap] MapNode map) {
            if (!map.Entity.HasComponent<MapPreviewDataComponent>()) {
                AssetRequestEvent assetRequestEvent = new();
                assetRequestEvent.Init<MapPreviewDataComponent>(map.mapPreview.AssetGuid);
                ScheduleEvent(assetRequestEvent, map);
            } else {
                SetPreviewImage(screen, map);
            }

            screen.createBattleScreen.mapName.text = map.descriptionItem.Name;
        }

        [OnEventFire]
        public void ShowMapPreviewOnLoad(NodeAddedEvent e, MapWithPreviewDataNode map, [JoinByMap] ScreenNode screen) {
            SetPreviewImage(screen, map);
        }

        [OnEventFire]
        public void SendCreateBattle(ButtonClickEvent e, CreateBattleButtonNode button, [JoinAll] SingleNode<ClientSessionComponent> session, [JoinAll] ScreenNode screen,
            [JoinAll] ICollection<MapNode> maps, [JoinAll] SingleNode<Dialogs60Component> dialogs, [JoinAll] SingleNode<SelfUserComponent> selfUser) {
            if (selfUser.Entity.HasComponent<SquadGroupComponent>() && !selfUser.Entity.HasComponent<SquadLeaderComponent>()) {
                CantStartGameInSquadDialogComponent cantStartGameInSquadDialogComponent = dialogs.component.Get<CantStartGameInSquadDialogComponent>();
                cantStartGameInSquadDialogComponent.CantSearch = false;
                cantStartGameInSquadDialogComponent.Show();
            } else {
                button.buttonMapping.Button.interactable = false;

                NewEvent(new CreateCustomBattleLobbyEvent {
                    Params = CollectBattleParams(screen, maps)
                }).Attach(session).ScheduleDelayed(0.3f);
            }
        }

        [OnEventFire]
        public void EditBattleParams(ButtonClickEvent e, EditBattleParamsButtonNode button) {
            MainScreenComponent.Instance.ShowScreen(MainScreenComponent.MainScreens.CreateBattle, false);
        }

        [OnEventFire]
        public void SendUpdateBattleParams(ButtonClickEvent e, UpdateBattleParamsButtonNode button, [JoinAll] SingleNode<ClientSessionComponent> session, [JoinAll] LobbyNode lobby,
            [JoinAll] ScreenNode screen, [JoinAll] ICollection<MapNode> maps) {
            button.buttonMapping.Button.interactable = false;

            ScheduleEvent(new UpdateBattleParamsEvent {
                Params = CollectBattleParams(screen, maps)
            }, lobby);
        }

        [OnEventFire]
        public void OnUpdateResponse(NodeAddedEvent e, SingleNode<ClientBattleParamsComponent> lobby, [JoinAll] ScreenNode screen) {
            MainScreenComponent.Instance.ShowScreen(MainScreenComponent.MainScreens.MatchLobby);
        }

        [OnEventFire]
        public void RegisterMapEnabledInCustomComponent(NodeAddedEvent e, SingleNode<MapEnabledInCustomGameComponent> map) { }

        ClientBattleParams CollectBattleParams(ScreenNode screen, ICollection<MapNode> maps) {
            CreateBattleScreenComponent component = screen.createBattleScreen;
            ClientBattleParams clientBattleParams = new();
            clientBattleParams.BattleMode = GetBattleMode(component);
            clientBattleParams.MapId = maps.First(map => map.descriptionItem.Name.Equals(component.mapDropdown.Selected)).Entity.Id;
            clientBattleParams.MaxPlayers = GetPlayerLimit(component);
            clientBattleParams.TimeLimit = GetTimeLimit(component);
            clientBattleParams.ScoreLimit = 0;
            clientBattleParams.FriendlyFire = component.friendlyFireDropdown.SelectionIndex == 1;
            clientBattleParams.Gravity = GetGravityType(component);
            clientBattleParams.KillZoneEnabled = component.killZoneDropdown.SelectionIndex == 1;
            clientBattleParams.DisabledModules = component.enabledModulesDropdown.SelectionIndex == 0;
            ClientBattleParams clientBattleParams2 = clientBattleParams;
            SaveParams(component, clientBattleParams2);
            return clientBattleParams2;
        }

        static void SaveParams(CreateBattleScreenComponent component, ClientBattleParams battleParams) {
            PlayerPrefs.SetInt("BattleParams.BattleMode", component.battleModeDropdown.SelectionIndex);
            PlayerPrefs.SetInt("BattleParams.MapId", (int)battleParams.MapId);
            PlayerPrefs.SetInt("BattleParams.MaxPlayers", battleParams.MaxPlayers);
            PlayerPrefs.SetInt("BattleParams.TimeLimit", battleParams.TimeLimit);
            PlayerPrefs.SetInt("BattleParams.ScoreLimit", battleParams.ScoreLimit);
            PlayerPrefs.SetInt("BattleParams.FriendlyFire", battleParams.FriendlyFire ? 1 : 0);
            PlayerPrefs.SetInt("BattleParams.Gravity", component.gravityDropdown.SelectionIndex);
            PlayerPrefs.SetInt("BattleParams.KillZoneDisabled", !battleParams.KillZoneEnabled ? 1 : 0);
            PlayerPrefs.SetInt("BattleParams.EnabledModules", !battleParams.DisabledModules ? 1 : 0);
            PlayerPrefs.Save();
        }

        static ClientBattleParams LoadParams() {
            ClientBattleParams clientBattleParams = new();
            clientBattleParams.BattleMode = (BattleMode)Enum.GetValues(typeof(BattleMode)).GetValue(PlayerPrefs.GetInt("BattleParams.BattleMode"));
            clientBattleParams.MapId = PlayerPrefs.GetInt("BattleParams.MapId");
            clientBattleParams.MaxPlayers = PlayerPrefs.GetInt("BattleParams.MaxPlayers");
            clientBattleParams.TimeLimit = PlayerPrefs.GetInt("BattleParams.TimeLimit");
            clientBattleParams.ScoreLimit = PlayerPrefs.GetInt("BattleParams.ScoreLimit");
            clientBattleParams.FriendlyFire = PlayerPrefs.GetInt("BattleParams.FriendlyFire") != 0;
            clientBattleParams.Gravity = (GravityType)Enum.GetValues(typeof(GravityType)).GetValue(PlayerPrefs.GetInt("BattleParams.Gravity"));
            clientBattleParams.KillZoneEnabled = PlayerPrefs.GetInt("BattleParams.KillZoneDisabled") == 0;
            clientBattleParams.DisabledModules = PlayerPrefs.GetInt("BattleParams.EnabledModules") == 0;
            return clientBattleParams;
        }

        static BattleMode GetBattleMode(CreateBattleScreenComponent component) => ((BattleMode[])Enum.GetValues(typeof(BattleMode)))[component.battleModeDropdown.SelectionIndex];

        static int GetTimeLimit(CreateBattleScreenComponent component) {
            int result = 0;
            int.TryParse(((string)component.timeLimitDropdown.Selected).Split(' ')[0], out result);
            return result;
        }

        static int GetPlayerLimit(CreateBattleScreenComponent component) {
            int result = 20;
            string text = (string)component.maxPlayersDropdown.Selected;

            if (text == null) {
                return result;
            }

            int.TryParse(text, out result);

            if (GetBattleMode(component) != 0) {
                result *= 2;
            }

            return result;
        }

        static int GetScoreLimit(CreateBattleScreenComponent component) {
            int result = 0;
            string text = (string)component.scoreLimitDropdown.Selected;

            if (text != null) {
                int.TryParse(text, out result);
            }

            return result;
        }

        static GravityType GetGravityType(CreateBattleScreenComponent component) {
            int selectionIndex = component.gravityDropdown.SelectionIndex;
            return (GravityType)Enum.GetValues(typeof(GravityType)).GetValue(selectionIndex);
        }

        static void SetPreviewImage(ScreenNode screen, MapNode map) {
            screen.createBattleScreen.mapPreviewRawImage.texture = (Texture2D)map.Entity.GetComponent<MapPreviewDataComponent>().Data;
        }

        public class ScreenNode : Node {
            public CreateBattleScreenComponent createBattleScreen;
        }

        public class ScreenWithMapGroupNode : ScreenNode {
            public MapGroupComponent mapGroup;
        }

        public class CreateBattleButtonNode : Node {
            public ButtonMappingComponent buttonMapping;
            public CreateBattleButtonComponent createBattleButton;
        }

        public class EditBattleParamsButtonNode : Node {
            public ButtonMappingComponent buttonMapping;
            public EditCustomBattleParamsButtonComponent editCustomBattleParamsButton;
        }

        public class UpdateBattleParamsButtonNode : Node {
            public ButtonMappingComponent buttonMapping;
            public UpdateBattleParamsComponent updateBattleParams;
        }

        public class MapNode : Node {
            public DescriptionItemComponent descriptionItem;
            public MapComponent map;

            public MapModeRestrictionComponent mapModeRestriction;

            public MapPreviewComponent mapPreview;
        }

        public class MapWithPreviewDataNode : MapNode {
            public MapPreviewDataComponent mapPreviewData;
        }

        public class LobbyNode : Node {
            public ClientBattleParamsComponent clientBattleParams;
            public CustomBattleLobbyComponent customBattleLobby;

            public UserGroupComponent userGroup;
        }

        public class LobbyWithUserGroupNode : Node {
            public BattleLobbyComponent battleLobby;

            public UserGroupComponent userGroup;
        }
    }
}