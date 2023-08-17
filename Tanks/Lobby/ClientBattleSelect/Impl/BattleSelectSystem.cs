using System.Collections.Generic;
using System.Linq;
using Lobby.ClientControls.API;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientBattleSelect.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class BattleSelectSystem : ECSSystem {
        [OnEventFire]
        public void Init(NodeAddedEvent e, ScreenInitNode screen, BattleSelectNode battleSelect) =>
            screen.Entity.AddComponent<BattleSelectLoadedComponent>();

        [OnEventFire]
        public void RequestOnServer(ScreenRangeChangedEvent e, BattleSelectScreenNode screen,
            [JoinAll] BattleSelectNode battleSelect) => ScheduleEvent(new SearchRequestChangedEvent(e.Range), battleSelect);

        [OnEventFire]
        public void Deinit(NodeRemoveEvent e, ScreenInitNode screen, BattleSelectNode battleSelect) {
            Log.Info("Deinit");
            ScheduleEvent<ResetSearchEvent>(battleSelect);
            screen.Entity.RemoveComponent<BattleSelectLoadedComponent>();
        }

        [OnEventFire]
        public void ShowBattleInfo(ShowBattleEvent e, Node any, [JoinAll] NotBattleSelectScreenNode screen) {
            Entity entity = CreateEntity("BattleSelectScreenContext");
            entity.AddComponent(new BattleSelectScreenContextComponent(e.BattleId));
            ShowScreenLeftEvent<BattleSelectScreenComponent> showScreenLeftEvent = new();
            showScreenLeftEvent.SetContext(entity, true);
            ScheduleEvent(showScreenLeftEvent, entity);
        }

        [OnEventFire]
        public void ShowBattleInfo(ShowBattleEvent e, Node any, [JoinAll] BattleSelectScreenNode screen,
            [JoinAll] BattleSelectNode battleSelect) {
            IndexRange visibleItemsRange = screen.lazyList.VisibleItemsRange;
            screen.lazyList.ClearItems();
            ScheduleEvent<ResetSearchEvent>(battleSelect);
            ScheduleEvent(new RequestBattleEvent(e.BattleId), battleSelect);
            ScheduleEvent(new SearchRequestChangedEvent(visibleItemsRange), battleSelect);
        }

        [OnEventFire]
        public void ShowBattleWithContext(NodeAddedEvent e, ActiveContextNode activeContext,
            [JoinAll] BattleSelectNode battleSelect) {
            long? battleId = activeContext.battleSelectScreenContext.BattleId;

            if (battleId.HasValue) {
                ScheduleEvent(new RequestBattleEvent(battleId.Value), battleSelect);
            }
        }

        [OnEventFire]
        public void SetIdToContext(NodeAddedEvent e, SelectedBattleNode battle, [JoinAll] BattleSelectScreenNode screen,
            [JoinByScreen] ActiveContextNode context) => context.battleSelectScreenContext.BattleId = battle.Entity.Id;

        [OnEventFire]
        public void ClearContext(NodeRemoveEvent e, SelectedBattleNode battle, [JoinAll] BattleSelectScreenNode screen,
            [JoinByScreen] ActiveContextNode context) => context.battleSelectScreenContext.BattleId = null;

        [OnEventFire]
        public void UpdateScrollButtonsVisibility(ScrollLimitEvent e, BattleSelectScreenNode screen) {
            screen.battleSelectScreen.PrevBattlesButton.SetActive(!screen.lazyList.AtLimitLow);
            screen.battleSelectScreen.NextBattlesButton.SetActive(!screen.lazyList.AtLimitHigh);
        }

        [OnEventFire]
        public void PrevBattles(ButtonClickEvent e, SingleNode<PrevBattlesButtonComponent> prevBattlesButton,
            [JoinByScreen] BattleSelectScreenNode screen) => screen.lazyList.Scroll(-1);

        [OnEventFire]
        public void NextBattles(ButtonClickEvent e, SingleNode<NextBattlesButtonComponent> nextBattlesButton,
            [JoinByScreen] BattleSelectScreenNode screen) => screen.lazyList.Scroll(1);

        [OnEventFire]
        public void SetBattlesCount(SearchResultEvent e, BattleSelectNode battleSelect,
            [JoinAll] BattleSelectScreenNode screen) => screen.lazyList.ItemsCount = e.BattlesCount;

        [OnEventComplete]
        [Mandatory]
        public void AddSearchResult(SearchResultEvent e, BattleSelectNode battleSelect,
            [JoinAll] ICollection<BattleNode> battles) {
            if (Log.IsInfoEnabled) {
                Log.InfoFormat("AddSearchResult Battles={0}",
                    string.Join(", ", e.NewBattleEntries.Select(data => data.Id.ToString()).ToArray()));
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

        [Mandatory]
        [OnEventFire]
        public void ClearSearchResult(ResetSearchCallbackEvent e, BattleSelectNode battleSelect,
            [JoinAll] ICollection<BattleNode> battles) {
            foreach (BattleNode battle in battles) {
                TryRemoveSearchDataFromBattle(battle.Entity, battleSelect);
            }

            battleSelect.searchResult.PinnedBattles.Clear();
            battleSelect.searchResult.PersonalInfos.Clear();
            battleSelect.searchResult.BattlesCount = 0;
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

        public class ForegroundNode : Node {
            public ScreenForegroundComponent screenForeground;

            public ScreenForegroundAnimationComponent screenForegroundAnimation;
        }

        public class ActiveContextNode : Node {
            public BattleSelectScreenContextComponent battleSelectScreenContext;

            public ScreenGroupComponent screenGroup;
        }

        public class SelectedBattleNode : Node {
            public BattleComponent battle;

            public SelectedListItemComponent selectedListItem;
        }
    }
}