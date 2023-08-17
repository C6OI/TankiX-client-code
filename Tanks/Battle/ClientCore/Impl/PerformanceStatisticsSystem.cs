using System;
using System.Collections.Generic;
using Lobby.ClientEntrance.API;
using Lobby.ClientSettings.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class PerformanceStatisticsSystem : ECSSystem {
        public const string CONFIG_PATH = "service/performancestatistics";

        [Inject] public static UnityTime UnityTime { get; set; }

        [OnEventFire]
        public void CreatePerformanceStatisticsEntity(NodeAddedEvent e, SingleNode<SelfUserComponent> selfUser) =>
            CreateEntity<PerformanceStatisticsTemplate>("service/performancestatistics");

        [OnEventFire]
        public void InitMeasuringOnRoundStart(NodeAddedEvent e, RoundUserNode roundUser,
            [Context] [JoinByUser] BattleUserNode selfBattleUser, [JoinAll] StatisticsNode statistics) {
            PerformanceStatisticsHelperComponent performanceStatisticsHelper = statistics.performanceStatisticsHelper;
            PerformanceStatisticsSettingsComponent performanceStatisticsSettings = statistics.performanceStatisticsSettings;
            performanceStatisticsHelper.startRoundTimeInSec = UnityTime.realtimeSinceStartup;

            performanceStatisticsHelper.frames = new FramesCollection(performanceStatisticsSettings.HugeFrameDurationInMs,
                performanceStatisticsSettings.MeasuringIntervalInSec);

            performanceStatisticsHelper.tankCount = new StatisticCollection(50);
        }

        [OnEventFire]
        public void Update(TimeUpdateEvent e, BattleUserNode selfBattleUser,
            [JoinByUser] SingleNode<RoundUserComponent> selfRoundUser,
            [JoinByBattle] ICollection<SingleNode<RoundUserComponent>> allRoundUsers, [JoinAll] StatisticsNode statictics) {
            if (!RoundTimeTooShortForMeasuring(statictics)) {
                PerformanceStatisticsHelperComponent performanceStatisticsHelper = statictics.performanceStatisticsHelper;
                int num = (int)(e.DeltaTime * 1000f);
                performanceStatisticsHelper.frames.AddFrame(num);
                performanceStatisticsHelper.tankCount.Add(allRoundUsers.Count, num);
            }
        }

        [OnEventFire]
        public void SendStatisticDataOnRoundStop(NodeRemoveEvent e, SingleNode<RoundUserComponent> roundUser,
            [JoinByUser] BattleUserNode battleUser, [JoinByUser] SelfUserNode selfUser,
            [JoinByUser] SingleNode<RoundUserComponent> node, [JoinByBattle] SingleNode<BattleComponent> battle,
            [JoinByMap] SingleNode<MapComponent> map, [JoinAll] StatisticsNode statistics) {
            if (!RoundTimeTooShortForMeasuring(statistics)) {
                PerformanceStatisticsHelperComponent performanceStatisticsHelper = statistics.performanceStatisticsHelper;
                FramesCollection frames = performanceStatisticsHelper.frames;
                PerformanceStatisticData performanceStatisticData = new();
                performanceStatisticData.UserName = selfUser.userUid.Uid;
                performanceStatisticData.GraphicDeviceName = SystemInfo.graphicsDeviceName;
                performanceStatisticData.GraphicsDeviceType = SystemInfo.graphicsDeviceType.ToString();
                performanceStatisticData.GraphicsMemorySize = SystemInfo.graphicsMemorySize;
                performanceStatisticData.DefaultQuality = GraphicsSettings.INSTANCE.DefaultQuality.Name;
                performanceStatisticData.Quality = QualitySettings.names[QualitySettings.GetQualityLevel()];
                performanceStatisticData.Resolution = GraphicsSettings.INSTANCE.CurrentResolution.ToString();
                performanceStatisticData.MapName = GetMapName(map);

                performanceStatisticData.BattleRoundTimeInMin =
                    (int)((Time.realtimeSinceStartup - performanceStatisticsHelper.startRoundTimeInSec) / 60f);

                performanceStatisticData.TankCountModa = performanceStatisticsHelper.tankCount.Moda;
                performanceStatisticData.Moda = frames.Moda;
                performanceStatisticData.Average = frames.Average;
                performanceStatisticData.StandardDeviationInMs = frames.StandartDevation;
                performanceStatisticData.HugeFrameCount = frames.HugeFrameCount;
                performanceStatisticData.MinAverageForInterval = frames.MinAverageForInterval;
                performanceStatisticData.MaxAverageForInterval = frames.MaxAverageForInterval;

                performanceStatisticData.GraphicDeviceKey = string.Format("DeviceVendorID: {0}; DeviceID: {1}",
                    SystemInfo.graphicsDeviceVendorID,
                    SystemInfo.graphicsDeviceID);

                performanceStatisticData.GraphicsDeviceVersion = SystemInfo.graphicsDeviceVersion;
                PerformanceStatisticData performanceStatisticData2 = performanceStatisticData;
                Log.InfoFormat("{0}\n{1}", "PerformanceStatisticData", performanceStatisticData2);
                ScheduleEvent(new SendPerfomanceStatisticDataEvent(performanceStatisticData2), selfUser);
            }
        }

        static string GetMapName(SingleNode<MapComponent> map) {
            string configPath = ((EntityInternal)map.Entity).TemplateAccessor.Get().ConfigPath;
            int num = configPath.LastIndexOf("/", StringComparison.Ordinal);
            return num <= 0 ? configPath : configPath.Substring(num + 1);
        }

        static bool RoundTimeTooShortForMeasuring(StatisticsNode statictics) {
            float startRoundTimeInSec = statictics.performanceStatisticsHelper.startRoundTimeInSec;
            int delayInSecBeforeMeasuringStarted = statictics.performanceStatisticsSettings.DelayInSecBeforeMeasuringStarted;
            return UnityTime.realtimeSinceStartup - startRoundTimeInSec < delayInSecBeforeMeasuringStarted;
        }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;

            public UserUidComponent userUid;
        }

        public class RoundUserNode : Node {
            public RoundUserComponent roundUser;

            public UserGroupComponent userGroup;
        }

        public class BattleUserNode : Node {
            public SelfBattleUserComponent selfBattleUser;

            public UserReadyToBattleComponent userReadyToBattle;
        }

        public class StatisticsNode : Node {
            public PerformanceStatisticsHelperComponent performanceStatisticsHelper;
            public PerformanceStatisticsSettingsComponent performanceStatisticsSettings;
        }
    }
}