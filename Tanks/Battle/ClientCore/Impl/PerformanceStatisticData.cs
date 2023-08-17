namespace Tanks.Battle.ClientCore.Impl {
    public class PerformanceStatisticData {
        public string UserName { get; set; }

        public string GraphicDeviceName { get; set; }

        public string GraphicsDeviceType { get; set; }

        public int GraphicsMemorySize { get; set; }

        public string DefaultQuality { get; set; }

        public string Quality { get; set; }

        public string Resolution { get; set; }

        public string MapName { get; set; }

        public int BattleRoundTimeInMin { get; set; }

        public int TankCountModa { get; set; }

        public int Moda { get; set; }

        public int Average { get; set; }

        public int StandardDeviationInMs { get; set; }

        public int MinAverageForInterval { get; set; }

        public int MaxAverageForInterval { get; set; }

        public int HugeFrameCount { get; set; }

        public string GraphicDeviceKey { get; set; }

        public string GraphicsDeviceVersion { get; set; }

        public override string ToString() => string.Format(
            "UserName={0}\nGraphicDeviceName={1}\nGraphicsDeviceType={2}\nGraphicsMemorySize={3}\nDefaultQuality = {4}\nQuality={5}\nResolution = {6}\nBattleRoundTimeInMin = {7}\nMapName={8}\nTankCountModa={9}\nModa={10}\nAverage={11}\nStandardDeviationInMs={12}\nMinAverageForInterval={13}\nMaxAverageForInterval={14}\nHugeFrameCount={15}GraphicDeviceKey={16}\nGraphicsDeviceVersion={17}\n",
            UserName,
            GraphicDeviceName,
            GraphicsDeviceType,
            GraphicsMemorySize,
            DefaultQuality,
            Quality,
            Resolution,
            BattleRoundTimeInMin,
            MapName,
            TankCountModa,
            Moda,
            Average,
            StandardDeviationInMs,
            MinAverageForInterval,
            MaxAverageForInterval,
            HugeFrameCount,
            GraphicDeviceKey,
            GraphicsDeviceVersion);
    }
}