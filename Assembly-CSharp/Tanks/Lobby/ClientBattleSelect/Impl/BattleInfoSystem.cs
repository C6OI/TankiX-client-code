using System.Collections.Generic;
using System.Diagnostics;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientBattleSelect.API;
using Tanks.Lobby.ClientControls.API;
using UnityEngine;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class BattleInfoSystem : ECSSystem {
        [OnEventFire]
        [Mandatory]
        public void AddInfo(PersonalBattleInfoEvent e, SingleNode<BattleComponent> battle) {
            if (battle.Entity.HasComponent<PersonalBattleInfoComponent>()) {
                battle.Entity.GetComponent<PersonalBattleInfoComponent>().Info = e.Info;
                return;
            }

            battle.Entity.AddComponent(new PersonalBattleInfoComponent {
                Info = e.Info
            });
        }

        [OnEventFire]
        public void SetVisibleItems(ItemsVisibilityChangedEvent e, ScreenNode screen, [JoinAll] ICollection<BattleNode> battles) {
            screen.Entity.RemoveComponent<VisibleItemsRangeComponent>();
            screen.Entity.AddComponent(new VisibleItemsRangeComponent(e.Range));

            foreach (BattleNode battle in battles) {
                int indexInSearchResult = battle.searchData.IndexInSearchResult;

                if (!e.Range.Contains(indexInSearchResult) && e.PrevRange.Contains(indexInSearchResult)) {
                    Log.InfoFormat("RemoveVisibleItem {0}", battle.Entity.Id);
                    battle.Entity.RemoveComponent<VisibleItemComponent>();
                } else if (e.Range.Contains(indexInSearchResult) && !e.PrevRange.Contains(indexInSearchResult)) {
                    Log.InfoFormat("AddVisibleItem {0}", battle.Entity.Id);
                    battle.Entity.AddComponent<VisibleItemComponent>();
                }
            }
        }

        [OnEventFire]
        public void AddVisibleItem(NodeAddedEvent e, BattleNode battle, [JoinAll] ScreenNode screen) {
            if (screen.visibleItemsRange.Range.Contains(battle.searchData.IndexInSearchResult)) {
                Log.InfoFormat("AddVisibleItem {0}", battle.Entity.Id);
                battle.Entity.AddComponent<VisibleItemComponent>();
            }
        }

        [OnEventFire]
        public void RequestImage(NodeAddedEvent e, VisibleBattleNode battle, [JoinByMap] SingleNode<MapPreviewComponent> map, [JoinAll] ScreenNode screen) {
            Log.InfoFormat("RequestImage {0}", battle);
            RectTransform item = screen.lazyList.GetItem(battle.searchData.IndexInSearchResult);
            EntityBehaviour entityBehaviour = Object.Instantiate(screen.battleSelectScreen.ItemContentPrefab);
            screen.lazyList.SetItemContent(battle.searchData.IndexInSearchResult, entityBehaviour.GetComponent<RectTransform>());
            entityBehaviour.BuildEntity(battle.Entity);
            screen.lazyList.UpdateSelection(battle.searchData.IndexInSearchResult);
            AssetRequestEvent assetRequestEvent = new();
            assetRequestEvent.Init<MapPreviewDataComponent>(map.component.AssetGuid);
            ScheduleEvent(assetRequestEvent, battle);
        }

        [OnEventFire]
        public void ReplaceIcon(NodeAddedEvent e, CTFBattleNode battle) {
            battle.battleItemContent.SetFlagAsScoreIcon();
        }

        [OnEventFire]
        public void SetImage(NodeAddedEvent e, BattleWithPreviewDataNode battle, [JoinAll] ScreenNode screen) {
            Log.InfoFormat("SetImage {0}", battle.mapPreviewData.Data);
            battle.battleItemContent.SetPreview((Texture2D)battle.mapPreviewData.Data);
        }

        [OnEventFire]
        public void Update(UpdateEvent e, BattleWithViewNode battle) {
            battle.battleItemContent.SetModeField(battle.battleMode.BattleMode.ToString());
            battle.battleItemContent.SetUserCountField(battle.userCount.UserCount + " / " + battle.userLimit.UserLimit);
        }

        [OnEventFire]
        public void SetArchived(NodeAddedEvent e, ArchivedBattleNode battle) {
            battle.battleItemContent.EntranceLocked = true;
        }

        [OnEventFire]
        public void UpdateTime(UpdateEvent e, BattleTimeWithLimitNode battle, [JoinByBattle] RoundNode round) {
            long timeLimitSec = battle.timeLimit.TimeLimitSec;
            float num = timeLimitSec;

            if (battle.Entity.HasComponent<BattleStartTimeComponent>() && !round.Entity.HasComponent<RoundWarmingUpStateComponent>()) {
                float num2 = Date.Now - battle.Entity.GetComponent<BattleStartTimeComponent>().RoundStartTime;
                num -= num2;
            }

            string timerText = TimerUtils.GetTimerText(num);
            battle.battleItemContent.SetTimeField(timerText);
        }

        [OnEventFire]
        public void HideTime(NodeAddedEvent e, BattleTimeWithoutLimitNode battle) {
            battle.battleItemContent.HideTime();
        }

        [OnEventFire]
        public void HideScore(NodeAddedEvent e, BattleScoreWithoutLimitNode battle) {
            battle.battleItemContent.HideScore();
        }

        [OnEventFire]
        [Conditional("DEBUG")]
        public void Update(UpdateEvent e, ScreenNode screen, [JoinAll] ICollection<BattleWithViewNode> battles) {
            if (Input.GetKeyDown(KeyCode.F8)) {
                screen.battleSelectScreen.DebugEnabled = !screen.battleSelectScreen.DebugEnabled;
            }

            foreach (BattleWithViewNode battle in battles) {
                string debugField = string.Empty;

                if (screen.battleSelectScreen.DebugEnabled) {
                    SearchDataComponent searchData = battle.searchData;
                    BattleEntry battleEntry = searchData.BattleEntry;

                    debugField = string.Format("id={0}\nindex={1}\nrelevance={2}\nfriends={3}\nserver={4}\nlobby={5}", battle.Entity.Id, searchData.IndexInSearchResult,
                        battleEntry.Relevance, battleEntry.FriendsInBattle, battleEntry.Server, battleEntry.LobbyServer);
                }

                battle.battleItemContent.SetDebugField(debugField);
            }
        }

        public class BattleNode : Node {
            public BattleComponent battle;

            public BattleConfiguredComponent battleConfigured;

            public BattleModeComponent battleMode;
            public MapGroupComponent mapGroup;

            public SearchDataComponent searchData;

            public UserCountComponent userCount;

            public UserLimitComponent userLimit;
        }

        public class VisibleBattleNode : BattleNode {
            public VisibleItemComponent visibleItem;
        }

        public class BattleWithViewNode : VisibleBattleNode {
            public BattleItemContentComponent battleItemContent;
        }

        [Not(typeof(TimeLimitComponent))]
        public class BattleTimeWithoutLimitNode : BattleWithViewNode { }

        public class BattleTimeWithLimitNode : BattleWithViewNode {
            public TimeLimitComponent timeLimit;
        }

        [Not(typeof(ScoreLimitComponent))]
        public class BattleScoreWithoutLimitNode : BattleWithViewNode {
            public BattleScoreComponent battleScore;
        }

        public class ScreenNode : Node {
            public BattleSelectScreenComponent battleSelectScreen;

            public LazyListComponent lazyList;

            public VisibleItemsRangeComponent visibleItemsRange;
        }

        public class BattleWithPreviewDataNode : VisibleBattleNode {
            public BattleItemContentComponent battleItemContent;
            public MapPreviewDataComponent mapPreviewData;
        }

        public class ArchivedBattleNode : BattleWithViewNode {
            public ArchivedBattleComponent archivedBattle;
        }

        public class CTFBattleNode : VisibleBattleNode {
            public BattleItemContentComponent battleItemContent;
            public CTFComponent ctf;
        }

        public class RoundNode : Node {
            public BattleGroupComponent battleGroup;

            public RoundComponent round;
        }
    }
}