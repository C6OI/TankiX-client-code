using System;
using System.Collections.Generic;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientSettings.API;
using UnityEngine;
using Profiler = UnityEngine.Profiling.Profiler;

namespace Tanks.Battle.ClientCore.Impl {
    public class PerformanceStatisticsSystem : ECSSystem {
        public const string CONFIG_PATH = "service/performancestatistics";

        [Inject] public static UnityTime UnityTime { get; set; }

        [OnEventFire]
        public void CreatePerformanceStatisticsEntity(NodeAddedEvent e, SingleNode<SelfUserComponent> selfUser) {
            CreateEntity<PerformanceStatisticsTemplate>("service/performancestatistics");
        }

        [OnEventFire]
        public void InitMeasuringOnRoundStart(NodeAddedEvent e, RoundUserNode roundUser, [Context] [JoinByUser] BattleUserNode selfBattleUser, [JoinAll] StatisticsNode statistics) {
            PerformanceStatisticsHelperComponent performanceStatisticsHelper = statistics.performanceStatisticsHelper;
            PerformanceStatisticsSettingsComponent performanceStatisticsSettings = statistics.performanceStatisticsSettings;
            performanceStatisticsHelper.startRoundTimeInSec = UnityTime.realtimeSinceStartup;
            performanceStatisticsHelper.frames = new FramesCollection(performanceStatisticsSettings.HugeFrameDurationInMs, performanceStatisticsSettings.MeasuringIntervalInSec);
            performanceStatisticsHelper.tankCount = new StatisticCollection(50);
        }

        [OnEventFire]
        public void Update(TimeUpdateEvent e, BattleUserNode selfBattleUser, [JoinByUser] SingleNode<RoundUserComponent> selfRoundUser,
            [JoinByBattle] ICollection<SingleNode<RoundUserComponent>> allRoundUsers, [JoinAll] StatisticsNode statistics) {
            if (!RoundTimeTooShortForMeasuring(statistics)) {
                PerformanceStatisticsHelperComponent performanceStatisticsHelper = statistics.performanceStatisticsHelper;
                int num = (int)(e.DeltaTime * 1000f);
                performanceStatisticsHelper.frames.AddFrame(num);
                performanceStatisticsHelper.tankCount.Add(allRoundUsers.Count, num);
            }
        }

        [OnEventFire]
        public void SendStatisticDataOnRoundStop(NodeRemoveEvent e, SingleNode<RoundUserComponent> roundUser, [JoinByUser] BattleUserNode battleUser, [JoinByUser] SelfUserNode selfUser,
            [JoinByUser] SingleNode<RoundUserComponent> node, [JoinByBattle] SingleNode<BattleComponent> battle, [JoinByMap] SingleNode<MapComponent> map,
            [JoinAll] StatisticsNode statistics) {
            if (!RoundTimeTooShortForMeasuring(statistics)) {
                PerformanceStatisticsHelperComponent performanceStatisticsHelper = statistics.performanceStatisticsHelper;
                FramesCollection frames = performanceStatisticsHelper.frames;
                string[] handlerNames = new string[0];
                int[] handlerCallCounts = new int[0];
                PerformanceStatisticData performanceStatisticData = new();
                performanceStatisticData.UserName = selfUser.userUid.Uid;
                performanceStatisticData.GraphicDeviceName = SystemInfo.graphicsDeviceName;
                performanceStatisticData.GraphicsDeviceType = SystemInfo.graphicsDeviceType.ToString();
                performanceStatisticData.GraphicsMemorySize = SystemInfo.graphicsMemorySize;
                performanceStatisticData.DefaultQuality = GraphicsSettings.INSTANCE.DefaultQuality.Name;
                performanceStatisticData.Quality = QualitySettings.names[QualitySettings.GetQualityLevel()];
                performanceStatisticData.Resolution = GraphicsSettings.INSTANCE.CurrentResolution.ToString();
                performanceStatisticData.MapName = GetMapName(map);
                performanceStatisticData.BattleRoundTimeInMin = (int)((Time.realtimeSinceStartup - performanceStatisticsHelper.startRoundTimeInSec) / 60f);
                performanceStatisticData.TankCountModa = performanceStatisticsHelper.tankCount.Moda;
                performanceStatisticData.Moda = frames.Moda;
                performanceStatisticData.Average = frames.Average;
                performanceStatisticData.StandardDeviationInMs = frames.StandartDevation;
                performanceStatisticData.HugeFrameCount = frames.HugeFrameCount;
                performanceStatisticData.MinAverageForInterval = frames.MinAverageForInterval;
                performanceStatisticData.MaxAverageForInterval = frames.MaxAverageForInterval;
                performanceStatisticData.GraphicDeviceKey = string.Format("DeviceVendorID: {0}; DeviceID: {1}", SystemInfo.graphicsDeviceVendorID, SystemInfo.graphicsDeviceID);
                performanceStatisticData.AveragePing = battleUser.battlePing.getAveragePing();
                performanceStatisticData.PingModa = battleUser.battlePing.getMediana();
                performanceStatisticData.GraphicsDeviceVersion = SystemInfo.graphicsDeviceVersion;
                performanceStatisticData.CustomSettings = GraphicsSettings.INSTANCE.customSettings;
                performanceStatisticData.Windowed = !Screen.fullScreen;
                performanceStatisticData.SaturationLevel = GraphicsSettings.INSTANCE.CurrentSaturationLevel;
                performanceStatisticData.VegetationLevel = GraphicsSettings.INSTANCE.CurrentVegetationLevel;
                performanceStatisticData.GrassLevel = GraphicsSettings.INSTANCE.CurrentGrassLevel;
                performanceStatisticData.AntialiasingQuality = GraphicsSettings.INSTANCE.CurrentAntialiasingQuality;
                performanceStatisticData.AnisotropicQuality = GraphicsSettings.INSTANCE.CurrentAnisotropicQuality;
                performanceStatisticData.TextureQuality = GraphicsSettings.INSTANCE.CurrentTextureQuality;
                performanceStatisticData.ShadowQuality = GraphicsSettings.INSTANCE.CurrentShadowQuality;
                performanceStatisticData.AmbientOcclusion = GraphicsSettings.INSTANCE.currentAmbientOcclusion;
                performanceStatisticData.Bloom = GraphicsSettings.INSTANCE.currentBloom;
                performanceStatisticData.RenderResolutionQuality = GraphicsSettings.INSTANCE.CurrentRenderResolutionQuality;
                performanceStatisticData.SystemMemorySize = SystemInfo.systemMemorySize;
                performanceStatisticData.TotalReservedMemory = Profiler.GetTotalReservedMemory();
                performanceStatisticData.TotalAllocatedMemory = Profiler.GetTotalAllocatedMemory();
                performanceStatisticData.MonoHeapSize = Profiler.GetMonoHeapSize();
                performanceStatisticData.HandlerNames = handlerNames;
                performanceStatisticData.HandlerCallCounts = handlerCallCounts;
                PerformanceStatisticData performanceStatisticData2 = performanceStatisticData;
                Log.InfoFormat("{0}\n{1}", "PerformanceStatisticData", EcsToStringUtil.ToStringWithProperties(performanceStatisticData2));
                ScheduleEvent(new SendPerfomanceStatisticDataEvent(performanceStatisticData2), selfUser);
            }
        }

        static string GetMapName(Node map) {
            string configPath = ((EntityInternal)map.Entity).TemplateAccessor.Get().ConfigPath;
            int num = configPath.LastIndexOf("/", StringComparison.Ordinal);
            return num <= 0 ? configPath : configPath.Substring(num + 1);
        }

        static bool RoundTimeTooShortForMeasuring(StatisticsNode statistics) {
            float startRoundTimeInSec = statistics.performanceStatisticsHelper.startRoundTimeInSec;
            int delayInSecBeforeMeasuringStarted = statistics.performanceStatisticsSettings.DelayInSecBeforeMeasuringStarted;
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
            public BattlePingComponent battlePing;
            public SelfBattleUserComponent selfBattleUser;

            public UserReadyToBattleComponent userReadyToBattle;
        }

        public class StatisticsNode : Node {
            public PerformanceStatisticsHelperComponent performanceStatisticsHelper;
            public PerformanceStatisticsSettingsComponent performanceStatisticsSettings;
        }
    }
}