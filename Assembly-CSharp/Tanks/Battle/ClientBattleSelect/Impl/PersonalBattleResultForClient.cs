using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientBattleSelect.Impl {
    public class PersonalBattleResultForClient {
        public TeamColor UserTeamColor { get; set; }

        public TeamBattleResult TeamBattleResult { get; set; }

        public int Energy { get; set; }

        public int EnergyDelta { get; set; }

        public int CrystalsForExtraEnergy { get; set; }

        [ProtocolOptional] public EnergySource MaxEnergySource { get; set; }

        public int CurrentBattleSeries { get; set; }

        public int MaxBattleSeries { get; set; }

        public float ScoreBattleSeriesMultiplier { get; set; }

        public int RankExp { get; set; }

        public int RankExpDelta { get; set; }

        public int WeaponExp { get; set; }

        public int TankLevel { get; set; }

        public int WeaponLevel { get; set; }

        public int WeaponInitExp { get; set; }

        public int WeaponFinalExp { get; set; }

        public int TankExp { get; set; }

        public int TankInitExp { get; set; }

        public int TankFinalExp { get; set; }

        public int ItemsExpDelta { get; set; }

        public int ContainerScore { get; set; }

        public int ContainerScoreDelta { get; set; }

        public int ContainerScoreLimit { get; set; }

        public Entity Container { get; set; }

        public float ContainerScoreMultiplier { get; set; }

        public double Reputation { get; set; }

        public double ReputationDelta { get; set; }

        public Entity League { get; set; }

        public Entity PrevLeague { get; set; }

        public int LeaguePlace { get; set; }

        [ProtocolOptional] public Entity Reward { get; set; }
    }
}