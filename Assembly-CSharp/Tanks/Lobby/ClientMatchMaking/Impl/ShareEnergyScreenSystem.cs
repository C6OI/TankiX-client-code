using System;
using System.Collections.Generic;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientBattleSelect.API;
using Tanks.Lobby.ClientBattleSelect.Impl;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientUserProfile.API;
using UnityEngine;
using UnityEngine.UI;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Tanks.Lobby.ClientMatchMaking.Impl {
    public class ShareEnergyScreenSystem : ECSSystem {
        [OnEventFire]
        public void SetEnergy(NodeAddedEvent e, UserEnergyBarNode screen, [JoinAll] SelfUserInSquadNode user) {
            ScheduleEvent<UpdateSelfUserEnergyEvent>(user);
        }

        [OnEventFire]
        public void SetEnergy(UpdateClientEnergyEvent e, EnergyItemNode energy, [JoinByUser] UserInSquadNode user, [JoinAll] SelfUserInSquadNode selfUser) {
            ScheduleEvent<UpdateSelfUserEnergyEvent>(selfUser);
        }

        [OnEventFire]
        public void SetSelfEnergy(UpdateSelfUserEnergyEvent e, SelfUserInSquadNode user, [JoinByUser] EnergyItemNode energy, SelfUserInSquadNode user1, [JoinByLeague] LeagueNode league,
            [JoinAll] UserEnergyBarNode screen) {
            UserGiftEnergyEvent userGiftEnergyEvent = new();
            ScheduleEvent(userGiftEnergyEvent, user);
            screen.userEnergyBarUi.SetEnergyLevel(energy.userItemCounter.Count - GetPayed(user) + userGiftEnergyEvent.totalGift, league.leagueEnergyConfig.Capacity);
        }

        [OnEventFire]
        public void SetSquadLeaderView(NodeAddedEvent e, SingleNode<ShareEnergyScreenComponent> dialog, [JoinAll] Optional<SelfSquadLeaderNode> selfSquadLeader) {
            dialog.component.SelfPlayerIsSquadLeader = selfSquadLeader.IsPresent();
        }

        [OnEventFire]
        public void SetSquadLeaferView(NodeAddedEvent e, SelfSquadLeaderNode selfSquadLeader, [JoinAll] SingleNode<ShareEnergyScreenComponent> dialog) {
            dialog.component.SelfPlayerIsSquadLeader = true;
        }

        [OnEventFire]
        public void SetSquadLeaderView(NodeRemoveEvent e, SelfSquadLeaderNode selfSquadLeader, [JoinAll] SingleNode<ShareEnergyScreenComponent> dialog) {
            dialog.component.SelfPlayerIsSquadLeader = false;
        }

        [OnEventFire]
        public void CreateUser(NodeAddedEvent e, SingleNode<UsersEnergyCellsListUIComponent> list, [Combine] UserInSquadNode userInSquad, [JoinBySquad] SelfUserInSquadNode selfUser) {
            UserEnergyCellUIComponent userEnergyCellUIComponent = list.component.AddUserCell();
            userInSquad.userGroup.Attach(userEnergyCellUIComponent.GetComponent<EntityBehaviour>().Entity);
            UpdateSquadTeleportInfo(userInSquad);
        }

        [OnEventFire]
        public void RemoveUser(NodeRemoveEvent e, UserInSquadNode userInSquad, [JoinByUser] SingleNode<UserEnergyCellUIComponent> userCell,
            [JoinAll] SingleNode<UsersEnergyCellsListUIComponent> list) {
            list.component.RemoveUserCell(userCell.component);
            UpdateSquadTeleportInfo(userInSquad);
        }

        [OnEventFire]
        public void InitUser(NodeAddedEvent e, UserEnergyCellNode userEnergyCell, [JoinByUser] UserInSquadNode user, [JoinByLeague] LeagueNode league, UserEnergyCellNode userEnergyCell1,
            [JoinByUser] Optional<EnergyItemNode> energy) {
            UserGiftEnergyEvent userGiftEnergyEvent = new();
            ScheduleEvent(userGiftEnergyEvent, user);

            userEnergyCell.userEnergyCellUi.Setup(user.userUid.Uid, !energy.IsPresent() ? 0 : energy.Get().userItemCounter.Count + userGiftEnergyEvent.totalGift,
                league.leagueEnergyConfig.Cost);

            userEnergyCell.userEnergyCellUi.SetGiftValue(userGiftEnergyEvent.totalGift, userGiftEnergyEvent.uids);
        }

        [OnEventFire]
        public void EnergyAdded(NodeAddedEvent e, EnergyItemNode energy, [JoinByUser] UserInSquadNode user, [JoinByUser] UserEnergyCellNode userCell,
            [JoinByUser] UserInSquadNode userInSquad, [JoinByLeague] LeagueNode league) {
            UpdateEnergyCell(user, userCell, energy, league);
            UpdateSquadTeleportInfo(user);
        }

        [OnEventFire]
        public void UpdateEnergyCell(UpdateClientEnergyEvent e, EnergyItemNode energy, [JoinByUser] UserInSquadNode user, [JoinByUser] UserEnergyCellNode userCell,
            [JoinByUser] UserInSquadNode userInSquad, [JoinByLeague] LeagueNode league) {
            UpdateEnergyCell(user, userCell, energy, league);
            UpdateSquadTeleportInfo(user);
        }

        [OnEventFire]
        public void UpdateEnergyCells(SquadEnergyChangedEvent e, SquadNode squad, [JoinBySquad] SelfUserInSquadNode selfUserInSquad, [JoinBySquad] [Combine] UserInSquadNode user,
            [JoinByUser] UserEnergyCellNode userCell, [JoinByUser] EnergyItemNode energy, [JoinByUser] UserInSquadNode userInSquad, [JoinByLeague] LeagueNode league) {
            UpdateEnergyCell(user, userCell, energy, league);
        }

        void UpdateEnergyCell(UserInSquadNode user, [JoinByUser] UserEnergyCellNode userCell, [JoinByUser] EnergyItemNode energy, [JoinByLeague] LeagueNode league) {
            UserGiftEnergyEvent userGiftEnergyEvent = new();
            ScheduleEvent(userGiftEnergyEvent, user);
            userCell.userEnergyCellUi.Setup(user.userUid.Uid, energy.userItemCounter.Count + userGiftEnergyEvent.totalGift, league.leagueEnergyConfig.Cost);
            userCell.userEnergyCellUi.SetGiftValue(userGiftEnergyEvent.totalGift, userGiftEnergyEvent.uids);
        }

        [OnEventFire]
        public void SetReadyPlayers(UpdateTeleportInfoEvent e, Node any, [JoinAll] SelfUserInSquadNode selfUserInSquad, [JoinAll] ICollection<UserEnergyCellNode> userCells,
            [JoinAll] SingleNode<ShareEnergyScreenComponent> shareEnergyDialog) {
            int num = 0;

            foreach (UserEnergyCellNode userCell in userCells) {
                if (userCell.userEnergyCellUi.Ready) {
                    num++;
                }
            }

            shareEnergyDialog.component.ReadyPlayers(num, userCells.Count);
        }

        [OnEventComplete]
        public void SquadEnergyChanged(SquadEnergyChangedEvent e, SquadNode squad, [JoinBySquad] SelfUserInSquadNode selfUserInSquad) {
            UpdateSquadTeleportInfo(selfUserInSquad);
        }

        [OnEventFire]
        public void ShowAdditionalTeleportValueForUserCell(NodeAddedEvent e, EnergyPreviewCellNode previewCell) {
            UpdateSquadTeleportInfo(previewCell);
        }

        [OnEventFire]
        public void HideAdditionalTeleportValueForUserCell(NodeRemoveEvent e, EnergyPreviewCellNode previewCell, [JoinAll] SingleNode<ShareEnergyScreenComponent> shareEnergyDialog) {
            UpdateSquadTeleportInfo(previewCell);
        }

        void UpdateSquadTeleportInfo(Node any) {
            NewEvent<UpdateTeleportInfoEvent>().Attach(any).ScheduleDelayed(0f);
        }

        [OnEventFire]
        public void UpdateSquadTeleportInfo(UpdateTeleportInfoEvent e, Node any, [JoinAll] SelfUserInSquadNode selfUserInSquad,
            [JoinAll] SingleNode<ShareEnergyScreenComponent> shareEnergyDialog, [JoinAll] UserEnergyBarNode userEnergyBar, [JoinAll] Optional<EnergyPreviewCellNode> previewCell,
            [JoinByUser] Optional<EnergyItemNode> previewUserEnergy, [JoinByUser] Optional<UserInSquadNode> previewUserInSquad, [JoinByLeague] Optional<LeagueNode> previewUserLeague) {
            SquadTeleportPriceEvent squadTeleportPriceEvent = new();
            ScheduleEvent(squadTeleportPriceEvent, selfUserInSquad);
            SelfExcessEnergyEvent selfExcessEnergyEvent = new();
            ScheduleEvent(selfExcessEnergyEvent, selfUserInSquad);
            SquadCurrentEnergy squadCurrentEnergy = new();
            ScheduleEvent(squadCurrentEnergy, selfUserInSquad);
            shareEnergyDialog.component.TeleportPriceProgressBar.Progress = squadCurrentEnergy.CurrentEnergy / (float)squadTeleportPriceEvent.TeleportPrice;
            ScheduleEvent<UpdateSelfUserEnergyEvent>(selfUserInSquad);
            long num = 0L;

            if (previewCell.IsPresent()) {
                bool flag = previewUserInSquad.Get().Entity.HasComponent<SelfUserComponent>();
                UserGiftEnergyEvent userGiftEnergyEvent = new();
                ScheduleEvent(userGiftEnergyEvent, previewUserInSquad.Get());
                num = previewUserLeague.Get().leagueEnergyConfig.Cost - previewUserEnergy.Get().userItemCounter.Count - userGiftEnergyEvent.totalGift;

                if (num <= 0) {
                    previewCell.Get().userEnergyCellUi.HideShareButton();
                } else if (flag || selfExcessEnergyEvent.ExcessEnergy <= 0) {
                    previewCell.Get().userEnergyCellUi.SetShareEnergyText(num, true);
                    userEnergyBar.userEnergyBarUi.ShowAdditionalEnergyLevel(num);
                    shareEnergyDialog.component.TeleportPriceProgressBar.AdditionalProgress = num / (float)squadTeleportPriceEvent.TeleportPrice;
                } else {
                    long num2 = (long)Mathf.Min(selfExcessEnergyEvent.ExcessEnergy, num);
                    previewCell.Get().userEnergyCellUi.SetShareEnergyText(num2, false);
                    userEnergyBar.userEnergyBarUi.SetSharedEnergyLevel(num2);
                    shareEnergyDialog.component.TeleportPriceProgressBar.AdditionalProgress = num2 / (float)squadTeleportPriceEvent.TeleportPrice;
                }
            }
        }

        [OnEventFire]
        public void InitShareButton(NodeAddedEvent e, SingleNode<ShareEnergyButtonComponent> button) {
            if (button.Entity.HasComponent<UserGroupComponent>()) {
                button.Entity.RemoveComponent<UserGroupComponent>();
            }

            UserEnergyCellUIComponent componentInParent = button.component.GetComponentInParent<UserEnergyCellUIComponent>();
            componentInParent.GetComponent<EntityBehaviour>().Entity.GetComponent<UserGroupComponent>().Attach(button.Entity);
        }

        [OnEventFire]
        public void ShareEnergy(ButtonClickEvent e, SingleNode<ShareEnergyButtonComponent> button, [JoinByUser] UserInSquadNode userInSquad, [JoinByUser] UserEnergyCellNode userCell,
            [JoinAll] SelfUserInSquadNode selfUserInSquad, [JoinAll] SingleNode<Dialogs60Component> dialogs) {
            userCell.userEnergyCellUi.HideShareButton();

            if (userCell.userEnergyCellUi.Buy) {
                EnergyPriceEvent energyPriceEvent = new();
                energyPriceEvent.count = userCell.userEnergyCellUi.ShareEnergyValue;
                EnergyPriceEvent energyPriceEvent2 = energyPriceEvent;
                ScheduleEvent(energyPriceEvent2, userInSquad);
                BuyEnergyDialogComponent buyEnergyDialogComponent = dialogs.component.Get<BuyEnergyDialogComponent>();
                buyEnergyDialogComponent.Show(energyPriceEvent2.count, energyPriceEvent2.price);
            } else {
                ScheduleEvent(new ShareEnergyEvent {
                    ReceiverId = userInSquad.Entity.Id
                }, selfUserInSquad);
            }
        }

        [OnEventFire]
        public void GetEnergyPriceEvent(EnergyPriceEvent e, UserInSquadNode userInSquad, [JoinByUser] EnergyItemNode energy,
            [JoinByMarketItem] SingleNode<XPriceItemComponent> priceNode) {
            long price = (long)Math.Ceiling(priceNode.component.Price * (double)e.count / priceNode.component.Pieces);
            e.price = price;
        }

        [OnEventFire]
        public void BuyEnergy(DialogConfirmEvent e, SingleNode<BuyEnergyDialogComponent> dialog) {
            ScheduleEvent(new PressEnergyContextBuyButtonEvent {
                Count = dialog.component.EnergyCount,
                XPrice = dialog.component.Price
            }, dialog);
        }

        [OnEventFire]
        public void HideAllShareButtons(HideAllShareButtonsEvent e, Node any, [JoinAll] ICollection<UserEnergyCellNode> cells) {
            foreach (UserEnergyCellNode cell in cells) {
                cell.userEnergyCellUi.HideShareButton();
            }
        }

        [OnEventFire]
        public void InitStartBattleButton(NodeAddedEvent e, SingleNode<StartSquadBattleButtonComponent> button, [JoinAll] SelfUserInSquadNode selfUserInSquad,
            [JoinBySquad] SquadNode squad) {
            button.component.GetComponent<Button>().interactable = !squad.Entity.HasComponent<NotAllowedToBattleEntranceComponent>();
        }

        [OnEventFire]
        public void DeactivateStartBattleButton(NodeAddedEvent e, SingleNode<NotAllowedToBattleEntranceComponent> notAllowedToBattleEntrance,
            [JoinAll] SingleNode<StartSquadBattleButtonComponent> button) {
            button.component.GetComponent<Button>().interactable = false;
        }

        [OnEventFire]
        public void ActivateStartBattleButton(NodeRemoveEvent e, SingleNode<NotAllowedToBattleEntranceComponent> notAllowedToBattleEntrance,
            [JoinAll] SingleNode<StartSquadBattleButtonComponent> button) {
            button.component.GetComponent<Button>().interactable = true;
        }

        [OnEventFire]
        public void GetSelfExcessEnergy(SelfExcessEnergyEvent e, Node any, [JoinAll] SelfUserInSquadNode selfUserInSquad, [JoinByUser] EnergyItemNode selfEnergy,
            [JoinByUser] SelfUserInSquadNode selfUserInSquad1, [JoinByLeague] LeagueNode selfLeague) {
            e.ExcessEnergy = selfEnergy.userItemCounter.Count - selfLeague.leagueEnergyConfig.Cost - GetPayed(selfUserInSquad);
        }

        public long GetPayed(UserInSquadNode userInSquad) {
            long num = 0L;

            foreach (long key in userInSquad.battleEntrancePayer.EnergyPayments.Keys) {
                if (key != userInSquad.Entity.Id) {
                    num += userInSquad.battleEntrancePayer.EnergyPayments[key];
                }
            }

            return num;
        }

        [OnEventFire]
        public void GetUserGiftEnergy(UserGiftEnergyEvent e, UserInSquadNode user, [JoinBySquad] ICollection<UserInSquadNode> users) {
            foreach (UserInSquadNode user2 in users) {
                if (user2 == user) {
                    continue;
                }

                Dictionary<long, long> energyPayments = user2.battleEntrancePayer.EnergyPayments;

                foreach (long key in energyPayments.Keys) {
                    if (key == user.Entity.Id) {
                        e.uids.Add(user2.userUid.Uid);
                        e.totalGift += energyPayments[key];
                    }
                }
            }
        }

        [OnEventFire]
        public void GetSquadCurrentEnergy(SquadCurrentEnergy e, UserInSquadNode userInSquad, [JoinBySquad] [Combine] UserInSquadNode user, [JoinByUser] EnergyItemNode userEnergy,
            [JoinByUser] UserInSquadNode user1, [JoinByLeague] LeagueNode league) {
            UserGiftEnergyEvent userGiftEnergyEvent = new();
            ScheduleEvent(userGiftEnergyEvent, user);
            long num = (long)Mathf.Min(league.leagueEnergyConfig.Cost, userEnergy.userItemCounter.Count + userGiftEnergyEvent.totalGift);
            e.CurrentEnergy += num;
        }

        [OnEventFire]
        public void GetSquadTeleportPrice(SquadTeleportPriceEvent e, UserInSquadNode userInSquad, [JoinBySquad] [Combine] UserInSquadNode user, [JoinByLeague] LeagueNode league) {
            e.TeleportPrice += league.leagueEnergyConfig.Cost;
        }

        public class UserEnergyBarNode : Node {
            public UserEnergyBarUIComponent userEnergyBarUi;
        }

        public class SelfUserNode : Node {
            public LeagueGroupComponent leagueGroup;
            public SelfUserComponent selfUser;

            public UserRankComponent userRank;
        }

        public class EnergyItemNode : Node {
            public EnergyItemComponent energyItem;

            public UserGroupComponent userGroup;

            public UserItemComponent userItem;

            public UserItemCounterComponent userItemCounter;
        }

        public class LeagueNode : Node {
            public ChestBattleRewardComponent chestBattleReward;

            public LeagueConfigComponent leagueConfig;
            public LeagueEnergyConfigComponent leagueEnergyConfig;

            public LeagueGroupComponent leagueGroup;
        }

        public class UserInSquadNode : Node {
            public BattleEntrancePayerComponent battleEntrancePayer;

            public LeagueGroupComponent leagueGroup;

            public SquadGroupComponent squadGroup;
            public UserComponent user;

            public UserGroupComponent userGroup;

            public UserUidComponent userUid;
        }

        public class SelfUserInSquadNode : UserInSquadNode {
            public SelfUserComponent selfUser;
        }

        public class SelfSquadLeaderNode : SelfUserInSquadNode {
            public SquadLeaderComponent squadLeader;
        }

        public class SquadNode : Node {
            public SquadComponent squad;

            public SquadGroupComponent squadGroup;
        }

        public class UserEnergyCellNode : Node {
            public UserEnergyCellUIComponent userEnergyCellUi;
            public UserGroupComponent userGroup;
        }

        public class EnergyPreviewCellNode : UserEnergyCellNode {
            public AdditionalTeleportEnergyPreviewComponent additionalTeleportEnergyPreview;
        }

        public class UpdateSelfUserEnergyEvent : Event { }

        public class UpdateTeleportInfoEvent : Event { }

        public class SquadTeleportPriceEvent : Event {
            public long TeleportPrice;
        }

        public class SquadCurrentEnergy : Event {
            public long CurrentEnergy;
        }

        public class SelfExcessEnergyEvent : Event {
            public long ExcessEnergy;
        }

        public class EnergyPriceEvent : Event {
            public long count;

            public long price;
        }

        public class UserGiftEnergyEvent : Event {
            public long totalGift;

            public List<string> uids = new();
        }
    }
}