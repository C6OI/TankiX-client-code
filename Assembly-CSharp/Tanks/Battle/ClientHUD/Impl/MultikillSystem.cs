using System.Collections.Generic;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientHUD.API;
using Tanks.Lobby.ClientSettings.API;
using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    public class MultikillSystem : ECSSystem {
        readonly Dictionary<int, MultikillUIComponent> multikillNotifications = new();

        [OnEventFire]
        public void FillDictionary(NodeAddedEvent e, SingleNode<MultikillListComponent> multikillList) {
            multikillNotifications.Clear();
            MultikillElement[] multikillElements = multikillList.component.multikillElements;

            foreach (MultikillElement multikillElement in multikillElements) {
                multikillNotifications.Add(multikillElement.killNumber, multikillElement.multikillUiComponent);
            }
        }

        [OnEventFire]
        public void LoadMultikillSound(NodeAddedEvent e, SingleNode<MultikillUIComponent> node) {
            if (node.component.VoiceReference != null && !string.IsNullOrEmpty(node.component.VoiceReference.AssetGuid)) {
                AssetReferenceComponent component = new(node.component.VoiceReference);
                node.Entity.AddComponent(component);
                node.Entity.AddComponent<AssetRequestComponent>();
            }
        }

        [OnEventFire]
        public void InitializeSound(NodeAddedEvent e, MultikillDataNode node) {
            node.multikillUi.Voice = (GameObject)node.resourceData.Data;
        }

        [OnEventFire]
        public void DisableOldNotifications(ActivateMultikillNotificationEvent e, SingleNode<MultikillUIComponent> node,
            [JoinAll] [Combine] SingleNode<MultikillUIComponent> multikillUi) {
            if (multikillUi.Entity != node.Entity) {
                multikillUi.component.DeactivateEffect();
            }
        }

        [OnEventFire]
        public void DisableOldNotifications(DisableOldMultikillNotificationsEvent e, Node any, [JoinAll] [Combine] SingleNode<MultikillUIComponent> multikillUi) {
            if (!multikillUi.Entity.Id.Equals(any.Entity.Id)) {
                multikillUi.component.DeactivateEffect();
            }
        }

        [OnEventComplete]
        public void ActivateNewNotification(ActivateMultikillNotificationEvent e, SingleNode<MultikillUIComponent> node,
            [JoinAll] SingleNode<PostProcessingQualityVariantComponent> settings) {
            if (!settings.component.DisableBattleNotifications) {
                node.component.ActivateEffect(e.Score, e.Kills, e.UserName);
            }
        }

        [OnEventFire]
        public void GoldDrop(GoldScheduledNotificationEvent e, Node node, [JoinAll] SingleNode<MultikillListComponent> multikillList) {
            ActivateEffect(multikillList.component.goldBoxElement, 0, 0, e.Sender);
        }

        [OnEventFire]
        public void DoubleKill(KillStreakEvent e, SingleNode<TankIncarnationKillStatisticsComponent> node, [JoinByTank] SingleNode<SelfTankComponent> selfTank,
            [JoinAll] SingleNode<MultikillListComponent> multikillList) {
            int kills = node.component.Kills;

            if (multikillNotifications.ContainsKey(kills)) {
                ActivateEffect(multikillNotifications[kills], e.Score, kills, string.Empty);
            } else if (kills > 40) {
                ActivateEffect(multikillList.component.finalKillStreak, e.Score, kills, string.Empty);
            }
        }

        [OnEventFire]
        public void StreakTermination(StreakTerminationEvent e, SingleNode<SelfBattleUserComponent> battleUser, [JoinAll] StreakTerminationNode streakTermination) {
            StreakTerminationUIComponent streakTerminationUi = streakTermination.streakTerminationUi;
            string text = string.Format(streakTerminationUi.streakTerminationLocalization.Value, e.VictimUid);
            streakTermination.streakTerminationUi.streakTerminationText.text = text;
            ScheduleEvent<ActivateMultikillNotificationEvent>(streakTermination);
        }

        void ActivateEffect(MultikillUIComponent multikillUiComponent, int score, int kills, string userName = "") {
            Entity entity = multikillUiComponent.GetComponent<EntityBehaviour>().Entity;
            ActivateMultikillNotificationEvent activateMultikillNotificationEvent = new();
            activateMultikillNotificationEvent.Score = score;
            activateMultikillNotificationEvent.Kills = kills;
            activateMultikillNotificationEvent.UserName = userName;
            ActivateMultikillNotificationEvent eventInstance = activateMultikillNotificationEvent;
            ScheduleEvent(eventInstance, entity);
        }

        public class CombatEventLogNode : Node {
            public ActiveCombatLogComponent activeCombatLog;

            public CombatEventLogComponent combatEventLog;
            public CombatLogCommonMessagesComponent combatLogCommonMessages;

            public UILogComponent uiLog;
        }

        public class UserNode : Node {
            public UserRankComponent userRank;

            public UserUidComponent userUid;
        }

        public class MultikillDataNode : Node {
            public MultikillUIComponent multikillUi;

            public ResourceDataComponent resourceData;
        }

        public class StreakTerminationNode : Node {
            public MultikillUIComponent multikillUi;

            public StreakTerminationUIComponent streakTerminationUi;
        }
    }
}