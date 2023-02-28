using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientBattleSelect.Impl {
    public class BattleResultForClient {
        public long BattleId { get; set; }

        public long MapId { get; set; }

        public BattleMode BattleMode { get; set; }

        public BattleType MatchMakingModeType { get; set; }

        public bool Custom { get; set; }

        public int RedTeamScore { get; set; }

        public int BlueTeamScore { get; set; }

        public int DmScore { get; set; }

        public List<UserResult> RedUsers { get; set; }

        public List<UserResult> BlueUsers { get; set; }

        public List<UserResult> DmUsers { get; set; }

        public bool Spectator { get; set; }

        [ProtocolOptional] public PersonalBattleResultForClient PersonalResult { get; set; }

        public UserResult FindUserResultByUserId(long id) {
            Predicate<UserResult> match = r => r.UserId == id;
            UserResult userResult = RedUsers.Find(match);

            if (userResult != null) {
                return userResult;
            }

            userResult = BlueUsers.Find(match);

            if (userResult != null) {
                return userResult;
            }

            userResult = DmUsers.Find(match);

            if (userResult != null) {
                return userResult;
            }

            throw new Exception("User result not found: " + id);
        }

        public override string ToString() => EcsToStringUtil.ToStringWithProperties(this, "\n");
    }
}