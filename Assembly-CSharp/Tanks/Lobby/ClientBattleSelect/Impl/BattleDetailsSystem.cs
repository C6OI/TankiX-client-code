using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientBattleSelect.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientNavigation.Impl;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class BattleDetailsSystem : ECSSystem {
        long selectedBattleId = -1L;

        [OnEventFire]
        public void SelectBattle(NodeAddedEvent e, SelectedBattleNode battle) {
            Log.DebugFormat("SelectBattle {0}", battle);
            ScheduleEvent<SelectBattleEvent>(battle);
            selectedBattleId = battle.Entity.Id;
        }

        [OnEventFire]
        public void UnselectBattle(NodeRemoveEvent e, SelectedBattleNode battle) {
            bool flag = selectedBattleId != battle.Entity.Id;
            Log.DebugFormat("UnselectBattle {0} skip={1}", battle, flag);

            if (!flag) {
                ScheduleEvent<UnselectBattleEvent>(battle);
                selectedBattleId = -1L;
            }
        }

        [OnEventFire]
        public void SetMapNameText(NodeAddedEvent e, SelectedBattleNode battle, [JoinByMap] [Mandatory] MapNode map) {
            SetScreenHeaderEvent setScreenHeaderEvent = new();
            setScreenHeaderEvent.Immediate(map.descriptionItem.Name + " " + battle.battleMode.BattleMode);
            ScheduleEvent(setScreenHeaderEvent, battle);
        }

        [OnEventFire]
        public void DelaySetDefaultText(NodeAddedEvent e, ScreenNode screen) {
            NewEvent<DelayedSetTopPanelTextEvent>().Attach(screen).ScheduleDelayed(screen.battleSelectScreenHeaderText.HeaderTextShowDelaySeconds);
        }

        [OnEventFire]
        public void SetDefaultText(DelayedSetTopPanelTextEvent e, ActiveScreenNode screen, [JoinAll] [Mandatory] TopPanelNode topPanel) {
            if (string.IsNullOrEmpty(topPanel.topPanel.NewHeader)) {
                topPanel.topPanel.SetHeaderTextImmediately(screen.battleSelectScreenHeaderText.HeaderText);
            }
        }

        [OnEventFire]
        public void ShowTimeIndicator(NodeAddedEvent e, BattleDetailsIndicatorsNode battleDetailsIndicators, [Context] [JoinByScreen] ScreenWithBattleGroupNode screen,
            [Context] [JoinByBattle] BattleWithTimeLimitNode battleDMWithTimeLimit) {
            battleDetailsIndicators.battleDetailsIndicators.TimeIndicator.SetActive(true);
        }

        [OnEventFire]
        public void ShowScoreIndicator(NodeAddedEvent e, BattleDetailsIndicatorsNode battleDetailsIndicators, [Context] [JoinByScreen] ScreenWithBattleGroupNode screen,
            [Context] [JoinByBattle] DMBattleWithScoreLimitNode battleDMWithScoreLimit) {
            battleDetailsIndicators.battleDetailsIndicators.ScoreIndicator.SetActive(true);
        }

        [OnEventFire]
        public void ShowScoreIndicator(NodeAddedEvent e, BattleDetailsIndicatorsNode battleDetailsIndicators, [Context] [JoinByScreen] ScreenWithBattleGroupNode screen,
            [Context] [JoinByBattle] TeamBattleNode teamBattle) {
            battleDetailsIndicators.battleDetailsIndicators.ScoreIndicator.SetActive(true);
        }

        [OnEventFire]
        public void ShowLevelWarning(NodeAddedEvent e, SelectedBattleWithInfoNode battle, [Context] [JoinByBattle] ScreenWithBattleGroupNode screen,
            [Context] [JoinByScreen] BattleDetailsIndicatorsNode indicators) {
            PersonalBattleInfo info = battle.personalBattleInfo.Info;
            BattleSelectScreenLocalizationComponent battleSelectScreenLocalization = screen.battleSelectScreenLocalization;
            string text = battleSelectScreenLocalization.BattleLevelsIndicatorText + battle.battleLevelRange.Range.Position;

            if (battle.battleLevelRange.Range.Position != battle.battleLevelRange.Range.EndPosition) {
                text = text + "-" + battle.battleLevelRange.Range.EndPosition;
            }

            indicators.battleDetailsIndicators.BattleLevelsIndicator.ShowText(text);

            if (!battle.Entity.HasComponent<ArchivedBattleComponent>()) {
                if (!info.CanEnter) {
                    indicators.battleDetailsIndicators.LevelWarning.ShowText(battleSelectScreenLocalization.LevelErrorText);
                } else if (!info.InLevelRange) {
                    indicators.battleDetailsIndicators.LevelWarning.ShowText(battleSelectScreenLocalization.LevelWarningEquipDowngradedText);
                }
            }
        }

        [OnEventFire]
        public void ShowArchivedBattleIndicator(NodeAddedEvent e, ArchivedBattleNode battle, [Context] [JoinByBattle] ScreenWithBattleGroupNode screen,
            [Context] [JoinByScreen] BattleDetailsIndicatorsNode indicators) {
            indicators.battleDetailsIndicators.LevelWarning.Hide();
            indicators.battleDetailsIndicators.ArchivedBattleIndicator.SetActive(true);
        }

        [OnEventFire]
        public void HideIndicators(NodeRemoveEvent e, BattleDetailsIndicatorsNode battleDetailsIndicators) {
            battleDetailsIndicators.battleDetailsIndicators.ScoreIndicator.SetActive(false);
            battleDetailsIndicators.battleDetailsIndicators.TimeIndicator.SetActive(false);
            battleDetailsIndicators.battleDetailsIndicators.LevelWarning.Hide();
            battleDetailsIndicators.battleDetailsIndicators.BattleLevelsIndicator.Hide();
            battleDetailsIndicators.battleDetailsIndicators.ArchivedBattleIndicator.SetActive(false);
        }

        public class SelectedBattleNode : Node {
            public BattleComponent battle;

            public BattleConfiguredComponent battleConfigured;

            public BattleGroupComponent battleGroup;

            public BattleLevelRangeComponent battleLevelRange;

            public BattleModeComponent battleMode;

            public MapGroupComponent mapGroup;

            public SelectedListItemComponent selectedListItem;
        }

        public class SelectedBattleWithInfoNode : SelectedBattleNode {
            public PersonalBattleInfoComponent personalBattleInfo;
        }

        public class BattleWithTimeLimitNode : SelectedBattleNode {
            public TimeLimitComponent timeLimit;
        }

        public class DMBattleWithScoreLimitNode : SelectedBattleNode {
            public DMComponent dm;

            public ScoreLimitComponent scoreLimit;
        }

        public class TeamBattleNode : SelectedBattleNode {
            public TeamBattleComponent teamBattle;
        }

        public class ArchivedBattleNode : SelectedBattleNode {
            public ArchivedBattleComponent archivedBattle;
        }

        public class MapNode : Node {
            public DescriptionItemComponent descriptionItem;
            public MapComponent map;
        }

        public class TopPanelNode : Node {
            public TopPanelComponent topPanel;
        }

        public class ScreenNode : Node {
            public BattleSelectScreenComponent battleSelectScreen;

            public BattleSelectScreenHeaderTextComponent battleSelectScreenHeaderText;

            public ScreenGroupComponent screenGroup;
        }

        public class ScreenWithBattleGroupNode : ScreenNode {
            public BattleGroupComponent battleGroup;

            public BattleSelectScreenLocalizationComponent battleSelectScreenLocalization;
        }

        public class BattleDetailsIndicatorsNode : Node {
            public BattleDetailsIndicatorsComponent battleDetailsIndicators;

            public ScreenGroupComponent screenGroup;
        }

        public class DelayedSetTopPanelTextEvent : Event { }

        public class ActiveScreenNode : ScreenNode {
            public ActiveScreenComponent activeScreen;
        }
    }
}