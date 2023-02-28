using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientUserProfile.API;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ExitDialogSystem : ECSSystem {
        [OnEventFire]
        public void ExitDialogTimer(UpdateEvent e, DailyBonusReturnDialogNode dialog, [JoinAll] UserDailyBonusNode user, [JoinAll] DailyBonusConfig dailyBonusConfig) {
            float num = user.userDailyBonusNextReceivingDate.Date - Date.Now;
            string arg = Mathf.Floor(num / 60f % 60f).ToString("00");
            string arg2 = (num % 60f).ToString("00");
            string arg3 = Mathf.Floor(num / 60f / 60f).ToString("00");

            if (num <= 0f) {
                for (int i = 0; i < dialog.exitGameDialog.textNotReady.Length; i++) {
                    dialog.exitGameDialog.textNotReady[i].SetActive(false);
                }

                dialog.exitGameDialog.textReady.SetActive(true);
                return;
            }

            for (int j = 0; j < dialog.exitGameDialog.textNotReady.Length; j++) {
                dialog.exitGameDialog.textNotReady[j].SetActive(true);
            }

            dialog.exitGameDialog.textReady.SetActive(false);
            dialog.exitGameDialog.timer.text = string.Format("{0:0}:{1:00}:{2:00}", arg3, arg, arg2);
        }

        [OnEventFire]
        public void ExitDialogRewards(NodeAddedEvent e, DailyBonusReturnDialogNode dialog, [JoinAll] UserDailyBonusNode user, [JoinAll] DailyBonusConfig dailyBonusConfig) {
            bool flag = false;

            if (user.userStatistics.Statistics.ContainsKey("BATTLES_PARTICIPATED")) {
                flag = user.userStatistics.Statistics["BATTLES_PARTICIPATED"] >= dailyBonusConfig.dailyBonusCommonConfig.BattleCountToUnlockDailyBonuses;
            }

            if (!flag) {
                dialog.exitGameDialog.content.SetActive(false);
                return;
            }

            dialog.exitGameDialog.content.SetActive(true);
            DailyBonusFirstCycleComponent dailyBonusFirstCycle = dailyBonusConfig.dailyBonusFirstCycle;
            DailyBonusEndlessCycleComponent dailyBonusEndlessCycle = dailyBonusConfig.dailyBonusEndlessCycle;
            DailyBonusData dailyBonusData = dailyBonusConfig.dailyBonusFirstCycle.DailyBonuses[0];
            bool flag2 = true;
            dialog.exitGameDialog.ReceivedRewards.AddRange(user.userDailyBonusReceivedRewards.ReceivedRewards);
            int num;

            if (user.userDailyBonusCycle.CycleNumber == 0L) {
                num = dailyBonusFirstCycle.Zones[(int)(IntPtr)user.userDailyBonusZone.ZoneNumber];

                if (user.userDailyBonusReceivedRewards.ReceivedRewards.Count == num + 1) {
                    if (user.userDailyBonusZone.ZoneNumber + 1 >= dailyBonusFirstCycle.Zones.Length) {
                        flag2 = false;
                        num = dailyBonusEndlessCycle.Zones[0];
                        dialog.exitGameDialog.ReceivedRewards.Clear();
                    } else {
                        num = dailyBonusFirstCycle.Zones[(int)(IntPtr)(user.userDailyBonusZone.ZoneNumber + 1)];
                    }
                }
            } else {
                num = dailyBonusEndlessCycle.Zones[(int)(IntPtr)user.userDailyBonusZone.ZoneNumber];
                flag2 = false;

                if (user.userDailyBonusReceivedRewards.ReceivedRewards.Count == num + 1) {
                    if (user.userDailyBonusZone.ZoneNumber + 1 >= dailyBonusEndlessCycle.Zones.Length) {
                        num = dailyBonusEndlessCycle.Zones[0];
                        dialog.exitGameDialog.ReceivedRewards.Clear();
                    } else {
                        num = dailyBonusEndlessCycle.Zones[(int)(IntPtr)(user.userDailyBonusZone.ZoneNumber + 1)];
                    }
                }
            }

            for (int i = 0; i <= num; i++) {
                DailyBonusData dailyBonusData2 = !flag2 ? dailyBonusConfig.dailyBonusEndlessCycle.DailyBonuses[i] : dailyBonusConfig.dailyBonusFirstCycle.DailyBonuses[i];

                if (!dialog.exitGameDialog.ReceivedRewards.Contains(dailyBonusData2.Code)) {
                    if (dailyBonusData2.DailyBonusType == DailyBonusType.CONTAINER) {
                        dailyBonusData = dailyBonusData2;
                    }

                    if (dailyBonusData.DailyBonusType != DailyBonusType.CONTAINER && dailyBonusData2.DailyBonusType == DailyBonusType.DETAIL) {
                        dailyBonusData = dailyBonusData2;
                    }

                    if (dailyBonusData.DailyBonusType != DailyBonusType.CONTAINER && dailyBonusData.DailyBonusType != DailyBonusType.DETAIL &&
                        dailyBonusData2.DailyBonusType == DailyBonusType.XCRY) {
                        dailyBonusData = dailyBonusData2;
                    }

                    if (dailyBonusData.DailyBonusType != DailyBonusType.CONTAINER && dailyBonusData.DailyBonusType != DailyBonusType.DETAIL &&
                        dailyBonusData.DailyBonusType != DailyBonusType.XCRY && dailyBonusData2.DailyBonusType == DailyBonusType.ENERGY) {
                        dailyBonusData = dailyBonusData2;
                    }

                    if (dailyBonusData.DailyBonusType != DailyBonusType.CONTAINER && dailyBonusData.DailyBonusType != DailyBonusType.DETAIL &&
                        dailyBonusData.DailyBonusType != DailyBonusType.XCRY && dailyBonusData.DailyBonusType != DailyBonusType.ENERGY &&
                        dailyBonusData2.DailyBonusType == DailyBonusType.CRY && dailyBonusData2.CryAmount >= dailyBonusData.CryAmount) {
                        dailyBonusData = dailyBonusData2;
                    }
                }
            }

            InstantiateBonus(dialog, dailyBonusData);
        }

        void InstantiateBonus(DailyBonusReturnDialogNode dialog, DailyBonusData bonus) {
            DailyBonusType dailyBonusType = bonus.DailyBonusType;

            switch (dailyBonusType) {
                case DailyBonusType.CRY:
                    dialog.exitGameDialog.InstantiateCryBonus(bonus.CryAmount);
                    return;

                case DailyBonusType.XCRY:
                    dialog.exitGameDialog.InstantiateXCryBonus(bonus.XcryAmount);
                    return;

                case DailyBonusType.ENERGY:
                    dialog.exitGameDialog.InstantiateEnergyBonus(bonus.EnergyAmount);
                    return;

                case DailyBonusType.CONTAINER:
                    dialog.exitGameDialog.InstantiateContainerBonus(bonus.ContainerReward.MarketItemId);
                    return;
            }

            if (dailyBonusType == DailyBonusType.DETAIL) {
                dialog.exitGameDialog.InstantiateDetailBonus(bonus.DetailReward.MarketItemId);
            }
        }

        public class UserDailyBonusNode : Node {
            public SelfUserComponent selfUser;

            public UserDailyBonusCycleComponent userDailyBonusCycle;
            public UserDailyBonusInitializedComponent userDailyBonusInitialized;

            public UserDailyBonusNextReceivingDateComponent userDailyBonusNextReceivingDate;

            public UserDailyBonusReceivedRewardsComponent userDailyBonusReceivedRewards;

            public UserDailyBonusZoneComponent userDailyBonusZone;

            public UserStatisticsComponent userStatistics;
        }

        public class DailyBonusConfig : Node {
            public DailyBonusCommonConfigComponent dailyBonusCommonConfig;

            public DailyBonusEndlessCycleComponent dailyBonusEndlessCycle;

            public DailyBonusFirstCycleComponent dailyBonusFirstCycle;
        }

        public class DailyBonusReturnDialogNode : Node {
            public ExitGameDialogComponent exitGameDialog;
        }
    }
}