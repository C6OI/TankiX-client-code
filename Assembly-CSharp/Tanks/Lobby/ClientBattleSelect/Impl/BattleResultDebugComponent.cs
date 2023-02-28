using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine.EventSystems;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class BattleResultDebugComponent : UIBehaviour, Component {
        public double Reputation = 2100.0;

        public double ReputationDelta = 10.0;

        public int Energy = 10;

        public int EnergyDelta = 5;

        public int CrystalsForExtraEnergy = 1000;

        public EnergySource MaxEnergySource = EnergySource.MVP_BONUS;

        public int RankExp = 800;

        public int RankExpDelta = 100;

        public int WeaponExp = 8000;

        public int WeaponInitExp = 500;

        public int WeaponFinalExp = 2000;

        public int TankExp = 6000;

        public int TankInitExp = 5000;

        public int TankFinalExp = 10000;

        public int ItemsExpDelta = 1000;

        public int ContainerScore = 50;

        public float ContainerScoreMultiplier = 2f;

        public int ContainerScoreDelta = 300;

        public int ContainerScoreLimit = 100;

        public long Reward = 4294967358L;

        public long HullId = 537781597L;

        public long WeaponId = 1021054379L;

        public long HullSkinId = -1194388226L;

        public long WeaponSkinId = -472765007L;
    }
}