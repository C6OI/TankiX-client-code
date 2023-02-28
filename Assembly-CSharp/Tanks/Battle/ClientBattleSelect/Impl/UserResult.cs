using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientBattleSelect.Impl {
    public class UserResult {
        public long UserId { get; set; }

        public string Uid { get; set; }

        public int Rank { get; set; }

        public string AvatarId { get; set; }

        public long BattleUserId { get; set; }

        public double ReputationInBattle { get; set; }

        public long EnterTime { get; set; }

        public int Place { get; set; }

        public int Kills { get; set; }

        public int KillAssists { get; set; }

        public int KillStrike { get; set; }

        public int Deaths { get; set; }

        public int Damage { get; set; }

        public int Score { get; set; }

        public int ScoreWithoutPremium { get; set; }

        public int ScoreToExperience { get; set; }

        public int RankExpDelta { get; set; }

        public int ItemsExpDelta { get; set; }

        public int Flags { get; set; }

        public int FlagAssists { get; set; }

        public int FlagReturns { get; set; }

        public long WeaponId { get; set; }

        public long HullId { get; set; }

        public long PaintId { get; set; }

        public long CoatingId { get; set; }

        public long HullSkinId { get; set; }

        public long WeaponSkinId { get; set; }

        public List<ModuleInfo> Modules { get; set; }

        public int BonusesTaken { get; set; }

        public bool UnfairMatching { get; set; }

        public bool Deserted { get; set; }

        public Entity League { get; set; }

        public override string ToString() => EcsToStringUtil.ToStringWithProperties(this, "\n");
    }
}