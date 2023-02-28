using System;
using System.Collections.Generic;
using System.Linq;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientBattleSelect.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientNavigation.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class BattleSelectSystem : ECSSystem {
        public static int TRAIN_BATTLE_MAXIMAL_RANK = 1;

        [OnEventFire]
        public void Init(NodeAddedEvent e, SingleNode<BattlesButtonComponent> button) {
            button.component.gameObject.SetInteractable(true);
        }

        [OnEventFire]
        public void ShowBattlesScreen(ButtonClickEvent e, SingleNode<BattlesButtonComponent> node, [JoinAll] SelfUserNode user1, [JoinByUser] Optional<MountedWeaponNode> weapon,
            [JoinAll] SelfUserNode user2, [JoinByUser] Optional<MountedHullNode> hull) {
            if (TryEnterTrainBattle(user1, GetEffectiveLevel(weapon, hull))) {
                node.component.gameObject.SetInteractable(false);
            } else {
                ShowBattleSelect(null);
            }
        }

        int GetEffectiveLevel(Optional<MountedWeaponNode> weapon, Optional<MountedHullNode> hull) {
            if (weapon.IsPresent() && hull.IsPresent()) {
                return Math.Max(weapon.Get().upgradeLevelItem.Level, hull.Get().upgradeLevelItem.Level);
            }

            return TRAIN_BATTLE_MAXIMAL_RANK;
        }

        bool TryEnterTrainBattle(SelfUserNode selfUser, int effectiveLevel) {
            if (effectiveLevel < TRAIN_BATTLE_MAXIMAL_RANK) {
                EnterRelevantBattleEvent enterRelevantBattleEvent = new();
                enterRelevantBattleEvent.PreferredTeam = TeamColor.BLUE;
                enterRelevantBattleEvent.PreferredBattle = 0L;
                enterRelevantBattleEvent.Source = "Train";
                EnterRelevantBattleEvent eventInstance = enterRelevantBattleEvent;
                ScheduleEvent(eventInstance, selfUser);
                return true;
            }

            return false;
        }

        [OnEventFire]
        public void EnableBattleScreen(EnterRelevantBattleFailedEvent e, Node any, [JoinAll] SingleNode<BattlesButtonComponent> button) {
            button.component.gameObject.SetInteractable(true);
        }

        [OnEventFire]
        public void Init(NodeAddedEvent e, ScreenInitNode screen, BattleSelectNode battleSelect) {
            screen.Entity.AddComponent<BattleSelectLoadedComponent>();
        }

        [OnEventFire]
        public void RequestOnServer(ScreenRangeChangedEvent e, BattleSelectScreenNode screen, [JoinAll] BattleSelectNode battleSelect) {
            ScheduleEvent(new SearchRequestChangedEvent(e.Range), battleSelect);
        }

        [OnEventFire]
        public void Deinit(NodeRemoveEvent e, ScreenInitNode screen, BattleSelectNode battleSelect) {
            Log.Info("Deinit");
            ScheduleEvent<ResetSearchEvent>(battleSelect);
            screen.Entity.RemoveComponent<BattleSelectLoadedComponent>();
        }

        [OnEventFire]
        public void ShowBattleInfo(ShowBattleEvent e, Node any, [JoinAll] NotBattleSelectScreenNode screen) {
            ShowBattleSelect(e.BattleId);
        }

        [OnEventFire]
        public void ShowBattleInfo(ShowBattleEvent e, Node any, [JoinAll] BattleSelectScreenNode screen, [JoinAll] BattleSelectNode battleSelect) {
            IndexRange visibleItemsRange = screen.lazyList.VisibleItemsRange;
            screen.lazyList.ClearItems();
            ScheduleEvent<ResetSearchEvent>(battleSelect);
            ScheduleEvent(new RequestBattleEvent(e.BattleId), battleSelect);
            ScheduleEvent(new SearchRequestChangedEvent(visibleItemsRange), battleSelect);
        }

        [OnEventFire]
        public void ShowBattleWithContext(NodeAddedEvent e, ActiveContextNode activeContext, [JoinAll] BattleSelectNode battleSelect) {
            long? battleId = activeContext.battleSelectScreenContext.BattleId;

            if (battleId.HasValue) {
                ScheduleEvent(new RequestBattleEvent(battleId.Value), battleSelect);
            }
        }

        [OnEventFire]
        public void SetIdToContext(NodeAddedEvent e, SelectedBattleNode battle, [JoinAll] BattleSelectScreenNode screen, [JoinByScreen] ActiveContextNode context) {
            context.battleSelectScreenContext.BattleId = battle.Entity.Id;
        }

        [OnEventFire]
        public void ClearContext(NodeRemoveEvent e, SelectedBattleNode battle, [JoinAll] BattleSelectScreenNode screen, [JoinByScreen] ActiveContextNode context) {
            context.battleSelectScreenContext.BattleId = null;
        }

        [OnEventFire]
        public void UpdateScrollButtonsVisibility(ScrollLimitEvent e, BattleSelectScreenNode screen) {
            screen.battleSelectScreen.PrevBattlesButton.SetActive(!screen.lazyList.AtLimitLow);
            screen.battleSelectScreen.NextBattlesButton.SetActive(!screen.lazyList.AtLimitHigh);
        }

        [OnEventFire]
        public void PrevBattles(ButtonClickEvent e, SingleNode<PrevBattlesButtonComponent> prevBattlesButton, [JoinByScreen] BattleSelectScreenNode screen) {
            screen.lazyList.Scroll(-1);
        }

        [OnEventFire]
        public void NextBattles(ButtonClickEvent e, SingleNode<NextBattlesButtonComponent> nextBattlesButton, [JoinByScreen] BattleSelectScreenNode screen) {
            screen.lazyList.Scroll(1);
        }

        [OnEventFire]
        public void SetBattlesCount(SearchResultEvent e, BattleSelectNode battleSelect, [JoinAll] BattleSelectScreenNode screen) {
            screen.lazyList.ItemsCount = e.BattlesCount;
        }

        [OnEventComplete]
        [Mandatory]
        public void AddSearchResult(SearchResultEvent e, BattleSelectNode battleSelect, [JoinAll] ICollection<BattleNode> battles) {
            if (Log.IsInfoEnabled) {
                Log.InfoFormat("AddSearchResult Battles={0}", string.Join(", ", e.NewBattleEntries.Select(data => data.Id.ToString()).ToArray()));
            }

            battleSelect.searchResult.PinnedBattles.AddRange(e.NewBattleEntries);
            battleSelect.searchResult.PersonalInfos.AddRange(e.NewPersonalBattleInfos);
            battleSelect.searchResult.BattlesCount = e.BattlesCount;

            foreach (BattleNode battle in battles) {
                Log.InfoFormat("AddSearchResult(EVENT) call TryAddSearchDataToBattle " + battle.Entity);
                TryAddSearchDataToBattle(battle.Entity, battleSelect);
            }
        }

        [OnEventFire]
        public void AddSearchResult(NodeAddedEvent e, BattleNode battle, [JoinAll] BattleSelectNode battleSelect) {
            Log.InfoFormat("AddSearchResult(NA) call TryAddSearchDataToBattle " + battle.Entity);
            TryAddSearchDataToBattle(battle.Entity, battleSelect);
        }

        [OnEventFire]
        [Mandatory]
        public void ClearSearchResult(ResetSearchCallbackEvent e, BattleSelectNode battleSelect, [JoinAll] ICollection<BattleNode> battles) {
            foreach (BattleNode battle in battles) {
                TryRemoveSearchDataFromBattle(battle.Entity, battleSelect);
            }

            battleSelect.searchResult.PinnedBattles.Clear();
            battleSelect.searchResult.PersonalInfos.Clear();
            battleSelect.searchResult.BattlesCount = 0;
        }

        [OnEventFire]
        public void ParseLink(ParseLinkEvent e, Node node) {
            long result;

            if (e.Link.StartsWith("battleselect")) {
                e.CustomNavigationEvent = CreateShowEvent(null);
            } else if (e.Link.StartsWith("battle/") && long.TryParse(e.Link.Substring("battle/".Length), out result)) {
                e.CustomNavigationEvent = NewEvent(new ShowBattleEvent(result)).Attach(node);
            }
        }

        void ShowBattleSelect(long? battleId) {
            CreateShowEvent(battleId).Schedule();
        }

        EventBuilder CreateShowEvent(long? battleId) {
            Entity entity = CreateEntity("BattleSelectScreenContext");
            entity.AddComponent(new BattleSelectScreenContextComponent(battleId));
            ShowScreenLeftEvent<BattleSelectScreenComponent> showScreenLeftEvent = new();
            showScreenLeftEvent.SetContext(entity, true);
            return NewEvent(showScreenLeftEvent).Attach(entity);
        }

        void TryAddSearchDataToBattle(Entity battle, BattleSelectNode battleSelect) {
            Log.InfoFormat("TryAddSearchDataToBattle {0}", battle.Id);

            if (battle.HasComponent<SearchDataComponent>()) {
                return;
            }

            for (int i = 0; i < battleSelect.searchResult.PinnedBattles.Count; i++) {
                BattleEntry battleEntry = battleSelect.searchResult.PinnedBattles[i];

                if (battleEntry.Id == battle.Id) {
                    Log.InfoFormat("AddSearchDataToBattle {0}", battle.Id);
                    battle.AddComponent(new SearchDataComponent(battleEntry, i));

                    battle.AddComponent(new PersonalBattleInfoComponent {
                        Info = battleSelect.searchResult.PersonalInfos[i]
                    });

                    break;
                }
            }
        }

        void TryRemoveSearchDataFromBattle(Entity battle, BattleSelectNode battleSelect) {
            Log.InfoFormat("TryRemoveSearchDataFromBattle {0}", battle.Id);

            if (!battle.HasComponent<SearchDataComponent>()) {
                return;
            }

            for (int i = 0; i < battleSelect.searchResult.PinnedBattles.Count; i++) {
                if (battleSelect.searchResult.PinnedBattles[i].Id == battle.Id) {
                    Log.InfoFormat("RemoveSearchDataFromBattle {0}", battle.Id);
                    battle.RemoveComponent<SearchDataComponent>();
                    battle.RemoveComponent<PersonalBattleInfoComponent>();
                    break;
                }
            }
        }

        public class ScreenInitNode : Node {
            public BattleSelectScreenComponent battleSelectScreen;

            public LazyListComponent lazyList;

            public ScreenGroupComponent screenGroup;
        }

        public class BattleSelectScreenNode : Node {
            public ActiveScreenComponent activeScreen;

            public BattleSelectLoadedComponent battleSelectLoaded;
            public BattleSelectScreenComponent battleSelectScreen;

            public LazyListComponent lazyList;

            public ScreenGroupComponent screenGroup;
        }

        [Not(typeof(BattleSelectScreenComponent))]
        public class NotBattleSelectScreenNode : Node {
            public ActiveScreenComponent activeScreen;
        }

        public class BattleSelectNode : Node {
            public BattleSelectComponent battleSelect;

            public SearchResultComponent searchResult;
        }

        public class BattleNode : Node {
            public BattleComponent battle;

            public BattleGroupComponent battleGroup;
        }

        public class ActiveContextNode : Node {
            public BattleSelectScreenContextComponent battleSelectScreenContext;

            public ScreenGroupComponent screenGroup;
        }

        public class SelectedBattleNode : Node {
            public BattleComponent battle;

            public SelectedListItemComponent selectedListItem;
        }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;

            public UserRankComponent userRank;
        }

        public class MountedWeaponNode : Node {
            public MountedItemComponent mountedItem;

            public UpgradeLevelItemComponent upgradeLevelItem;
            public WeaponItemComponent weaponItem;
        }

        public class MountedHullNode : Node {
            public MountedItemComponent mountedItem;
            public TankItemComponent tankItem;

            public UpgradeLevelItemComponent upgradeLevelItem;
        }
    }
}