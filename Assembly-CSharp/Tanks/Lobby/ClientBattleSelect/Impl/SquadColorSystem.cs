using System.Collections.Generic;
using System.Linq;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientBattleSelect.API;
using Tanks.Lobby.ClientEntrance.API;
using UnityEngine;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class SquadColorSystem : ECSSystem {
        [OnEventFire]
        public void SetSelfLabelColor(NodeAddedEvent e, SelfSquadUser selfUserLabel, [JoinAll] SquadRegisterNode registerNode, [JoinAll] SquadColorsNode squadColorsNode) {
            registerNode.squadsRegister.squads[selfUserLabel.squadGroup.Key] = squadColorsNode.squadColors.SelfSquadColor;
        }

        [OnEventFire]
        public void SetLabelColor(NodeAddedEvent e, [Combine] SquadColorLabel userLabel, [JoinAll] [Context] SelfTeamUser selfUserLabel, [JoinAll] SquadRegisterNode registerNode,
            [JoinAll] SquadColorsNode squadColorsNode) {
            bool friendlySquad = userLabel.teamColor.TeamColor == selfUserLabel.teamColor.TeamColor;
            userLabel.userSquadColor.Color = GetColorForSquad(userLabel.squadGroup.Key, friendlySquad, registerNode.squadsRegister, squadColorsNode.squadColors);
        }

        [OnEventFire]
        public void RemoveSquadFromRegister(NodeRemoveEvent e, SquadColorLabel userLabel, [JoinBySquad] ICollection<SquadColorLabel> usersInSquad,
            [JoinAll] SquadRegisterNode registerNode) {
            userLabel.userSquadColor.Color = Color.clear;

            if (usersInSquad.Count <= 1 && registerNode.squadsRegister.squads.ContainsKey(userLabel.squadGroup.Key)) {
                registerNode.squadsRegister.squads.Remove(userLabel.squadGroup.Key);
            }
        }

        public Color GetColorForSquad(long key, bool friendlySquad, SquadsRegisterComponent squadsRegister, SquadColorsComponent squadColors) {
            if (squadsRegister.squads.ContainsKey(key)) {
                return squadsRegister.squads[key];
            }

            Color[] array = !friendlySquad ? squadColors.EnemyColor : squadColors.FriendlyColor;

            foreach (Color color in array) {
                if (ColorIsUnique(color, squadsRegister)) {
                    squadsRegister.squads[key] = color;
                    return color;
                }
            }

            return Color.white;
        }

        bool ColorIsUnique(Color color, SquadsRegisterComponent squadsRegister) {
            return squadsRegister.squads.Keys.All(key => !squadsRegister.squads[key].Equals(color));
        }

        public class SquadColorLabel : Node {
            public SquadGroupComponent squadGroup;

            public TeamColorComponent teamColor;
            public UserGroupComponent userGroup;

            public UserSquadColorComponent userSquadColor;
        }

        public class SelfTeamUser : Node {
            public SelfUserComponent selfUser;

            public TeamColorComponent teamColor;
            public UserGroupComponent userGroup;
        }

        public class SelfSquadUser : SelfTeamUser {
            public SquadGroupComponent squadGroup;
        }

        public class SquadColorsNode : Node {
            public SquadColorsComponent squadColors;
        }

        public class SquadRegisterNode : Node {
            public SquadsRegisterComponent squadsRegister;
        }
    }
}