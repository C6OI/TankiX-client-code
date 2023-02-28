using System.Collections.Generic;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientBattleSelect.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientUserProfile.API;
using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class SquadEnergySystem : ECSSystem {
        [OnEventFire]
        public void CheckSquadEnergy(CheckSquadEnergyEvent e, SquadSelfUserNode squadSelfUser, [JoinBySquad] ICollection<SquadUserNode> squadUsers) {
            bool flag = true;

            foreach (SquadUserNode squadUser in squadUsers) {
                CheckUserEnergyEvent checkUserEnergyEvent = new();
                ScheduleEvent(checkUserEnergyEvent, squadUser);
                flag &= checkUserEnergyEvent.HaveEnoughtEnergyForEntrance;
                Debug.Log(string.Concat("SquadEnergySystem.CheckSquadEnergy ", squadUser.Entity, " ", checkUserEnergyEvent.HaveEnoughtEnergyForEntrance, " ", flag));
            }

            e.HaveEnoughtEnergyForEntrance = flag;
        }

        [OnEventFire]
        public void CheckUserEnergy(CheckUserEnergyEvent e, SquadUserNode user, [JoinByUser] EnergyUserItemNode energy, SquadUserNode userToLeague,
            [JoinByLeague] SingleNode<LeagueEnergyConfigComponent> league) {
            e.HaveEnoughtEnergyForEntrance = energy.userItemCounter.Count >= league.component.Cost;
        }

        public class SquadNode : Node {
            public SquadComponent squad;

            public SquadGroupComponent squadGroup;
        }

        public class SquadUserNode : Node {
            public SquadGroupComponent squadGroup;
            public UserComponent user;

            public UserGroupComponent userGroup;
        }

        public class SquadSelfUserNode : SquadUserNode {
            public SelfUserComponent selfUser;
        }

        public class EnergyUserItemNode : Node {
            public EnergyItemComponent energyItem;
            public UserGroupComponent userGroup;

            public UserItemComponent userItem;

            public UserItemCounterComponent userItemCounter;
        }

        public class CheckUserEnergyEvent : Event {
            public bool HaveEnoughtEnergyForEntrance { get; set; }
        }
    }
}