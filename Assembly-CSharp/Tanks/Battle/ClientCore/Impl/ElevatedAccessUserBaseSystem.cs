using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientEntrance.API;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tanks.Battle.ClientCore.Impl {
    public class ElevatedAccessUserBaseSystem : ECSSystem {
        protected Entity user;

        protected void InitConsole(SelfUserNode selfUser) {
            user = selfUser.Entity;
            SmartConsole smartConsole = Object.FindObjectOfType<SmartConsole>();

            if (!(smartConsole != null)) {
                GameObject smartConsole2 = Object.FindObjectOfType<SmartConsoleActivator>().SmartConsole;
                Object.Instantiate(smartConsole2);
                SmartConsole.RegisterCommand("changeEnergy", "changeEnergy -10", "change user energy", ChangeEnergy);
                SmartConsole.RegisterCommand("incScore", "increaseScore RED", "Increases score of a team", IncreaseScore);
                SmartConsole.RegisterCommand("finish", "finish", "Finish current battle", FinishBattle);

                SmartConsole.RegisterCommand("banUser", "banUser User1 CHEATING DAY",
                    "Bans user chat for REASON for TYPE. Possible reasons: FLOOD, FOUL, SEX, POLITICS... Possible type: NONE, WARN, MINUTE, HOUR, DAY...", BanUser);

                SmartConsole.RegisterCommand("unbanUser", "unbanUser User1", "Unbans user chat", UnbanUser);
                SmartConsole.RegisterCommand("addScore", "addScore 100", "Add user score", AddScore);
                SmartConsole.RegisterCommand("addKills", "addKills 10", "Add kills to user", AddKills);
                SmartConsole.RegisterCommand("giveBattleQuest", "giveBattleQuest tutorial/supply", "give battle quest to user", GiveBattleQuest);
                SmartConsole.RegisterCommand("changeReputation", "changeReputation -10", "change reputation", ChangeReputation);
            }
        }

        protected void IncreaseScore(string parameters) {
            string value = parameters.Split(' ')[1];
            int amount = 1;

            try {
                amount = int.Parse(parameters.Split(' ')[2]);
            } catch (Exception) { }

            TeamColor teamColor = (TeamColor)Enum.Parse(typeof(TeamColor), value);
            ElevatedAccessUserIncreaseScoreEvent elevatedAccessUserIncreaseScoreEvent = new();
            elevatedAccessUserIncreaseScoreEvent.TeamColor = teamColor;
            elevatedAccessUserIncreaseScoreEvent.Amount = amount;
            ScheduleEvent(elevatedAccessUserIncreaseScoreEvent, user);
        }

        protected void FinishBattle(string parameters) {
            ScheduleEvent(new ElevatedAccessUserFinishBattleEvent(), user);
        }

        protected void RunCommand(string parameters) {
            string command = parameters.Replace("runCommand ", string.Empty);
            ElevatedAccessUserRunCommandEvent elevatedAccessUserRunCommandEvent = new();
            elevatedAccessUserRunCommandEvent.Command = command;
            ScheduleEvent(elevatedAccessUserRunCommandEvent, user);
        }

        T Punish<T>(string parameters) where T : ElevatedAccessUserBasePunishEvent, new() {
            string empty = string.Empty;
            string empty2 = string.Empty;

            try {
                empty = parameters.Split(' ')[1];
                empty2 = parameters.Split(' ')[2];
            } catch (Exception) {
                SmartConsole.WriteLine("Wrong parameters");
                return null;
            }

            return new T {
                Uid = empty,
                Reason = empty2
            };
        }

        protected void BanUser(string parameters) {
            ElevatedAccessUserBanUserEvent elevatedAccessUserBanUserEvent = Punish<ElevatedAccessUserBanUserEvent>(parameters);

            if (elevatedAccessUserBanUserEvent != null) {
                string type = string.Empty;

                try {
                    type = parameters.Split(' ')[3];
                } catch (Exception) {
                    SmartConsole.WriteLine("Wrong parameters");
                }

                elevatedAccessUserBanUserEvent.Type = type;
                ScheduleEvent(elevatedAccessUserBanUserEvent, user);
            }
        }

        protected void BlockUser(string parameters) {
            ScheduleEvent(Punish<ElevatedAccessUserBlockUserEvent>(parameters), user);
        }

        protected void UnbanUser(string parameters) {
            string uid = string.Empty;

            try {
                uid = parameters.Split(' ')[1];
            } catch (Exception) {
                SmartConsole.WriteLine("Wrong parameters");
            }

            ScheduleEvent(new ElevatedAccessUserUnbanUserEvent {
                Uid = uid
            }, user);
        }

        protected void AddScore(string parameters) {
            int num = 0;

            try {
                string s = parameters.Split(' ')[1];
                num = int.Parse(s);
            } catch (Exception) {
                SmartConsole.WriteLine("Wrong parameter");
                return;
            }

            ScheduleEvent(new ElevatedAccessUserAddScoreEvent {
                Count = num
            }, user);
        }

        protected void ThrowNullPointer(string parameters) {
            throw new NullReferenceException();
        }

        protected void DropSupply(string parameters) {
            BonusType bonusType = ExtractTypeFromParams<BonusType>(parameters);
            ElevatedAccessUserDropSupplyEvent elevatedAccessUserDropSupplyEvent = new();
            elevatedAccessUserDropSupplyEvent.BonusType = bonusType;
            ScheduleEvent(elevatedAccessUserDropSupplyEvent, user);
        }

        T ExtractTypeFromParams<T>(string parameters) where T : struct, IConvertible {
            string value = parameters.Split(' ')[1];
            return (T)Enum.Parse(typeof(T), value);
        }

        protected void DropGold(string parameters) {
            GoldType goldType = GoldType.TELEPORT;

            try {
                goldType = ExtractTypeFromParams<GoldType>(parameters);
            } catch (Exception) { }

            ElevatedAccessUserDropSupplyGoldEvent elevatedAccessUserDropSupplyGoldEvent = new();
            elevatedAccessUserDropSupplyGoldEvent.GoldType = goldType;
            ScheduleEvent(elevatedAccessUserDropSupplyGoldEvent, user);
        }

        protected void ChangeEnergy(string parameters) {
            int num = 0;

            try {
                string s = parameters.Split(' ')[1];
                num = int.Parse(s);
            } catch (Exception) {
                SmartConsole.WriteLine("Wrong parameter");
                return;
            }

            ScheduleEvent(new ElevatedAccessUserChangeEnergyEvent {
                Count = num
            }, user);
        }

        protected void CreateUserItem(string parameters) {
            long num = 0L;
            long num2 = 0L;

            try {
                string s = parameters.Split(' ')[2];
                num = long.Parse(s);
                s = parameters.Split(' ')[1];
                num2 = long.Parse(s);
            } catch (Exception) {
                SmartConsole.WriteLine("Wrong parameter");
                return;
            }

            ScheduleEvent(new ElevatedAccessUserCreateItemEvent {
                Count = num,
                ItemId = num2
            }, user);
        }

        protected void WipeUserItems(string parameters) {
            ScheduleEvent(new ElevatedAccessUserWipeUserItemsEvent(), user);
        }

        protected void AddBotsToBattle(string parameters) {
            int count;
            TeamColor teamColor;

            try {
                string s = parameters.Split(' ')[2];
                count = int.Parse(s);
                s = parameters.Split(' ')[1];
                teamColor = (TeamColor)Enum.Parse(typeof(TeamColor), s);
            } catch (Exception) {
                SmartConsole.WriteLine("Wrong parameter");
                return;
            }

            ScheduleEvent(new ElevatedAccessUserAddBotsToBattleEvent {
                TeamColor = teamColor,
                Count = count
            }, user);
        }

        protected void AddKills(string parameters) {
            int count;

            try {
                string s = parameters.Split(' ')[1];
                count = int.Parse(s);
            } catch (Exception) {
                SmartConsole.WriteLine("Wrong parameter");
                return;
            }

            ScheduleEvent(new ElevatedAccessUserAddKillsEvent {
                Count = count
            }, user);
        }

        protected void GiveBattleQuest(string parameters) {
            string questPath = string.Empty;

            try {
                questPath = parameters.Split(' ')[1];
            } catch (Exception) {
                SmartConsole.WriteLine("Wrong parameters");
            }

            ScheduleEvent(new ElevatedAccessUserGiveBattleQuestEvent {
                QuestPath = questPath
            }, user);
        }

        protected void ChangeReputation(string parameters) {
            int num = 0;

            try {
                string s = parameters.Split(' ')[1];
                num = int.Parse(s);
            } catch (Exception) {
                SmartConsole.WriteLine("Wrong parameter");
                return;
            }

            ScheduleEvent(new ElevatedAccessUserChangeReputationEvent {
                Count = num
            }, user);
        }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;
        }
    }
}