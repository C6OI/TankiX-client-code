using System.Collections.Generic;
using System.Linq;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    public class CTFFlagsHUDSystem : ECSSystem {
        [OnEventFire]
        public void OnPickUp(NodeAddedEvent e, HUDNodes.SelfBattleUserNode self, [JoinByBattle] [Context] [Combine] CarriedFlagNode flag, [JoinByTeam] [Context] TeamNode color,
            HUDNode hud, Optional<SingleNode<CTFHUDMessagesComponent>> messages) {
            switch (color.colorInBattle.TeamColor) {
                case TeamColor.BLUE:
                    hud.flagsHUD.BlueFlag.PickUp();
                    break;

                case TeamColor.RED:
                    hud.flagsHUD.RedFlag.PickUp();
                    break;
            }

            hud.flagsHUD.RequestShow();
            flag.flagInstance.FlagBeam.SetActive(false);
            HUDNodes.SelfTankNode selfTankNode = Select<HUDNodes.SelfTankNode>(self.Entity, typeof(UserGroupComponent)).FirstOrDefault();

            if (selfTankNode == null || !messages.IsPresent()) {
                return;
            }

            if (selfTankNode.tankGroup.Key == flag.tankGroup.Key && selfTankNode.Entity.GetComponent<TeamGroupComponent>().Key != flag.teamGroup.Key) {
                hud.mainHUD.ShowMessageWithPriority(messages.Get().component.DeliverFlagMessage, 10);
                return;
            }

            FlagCarrierNode flagCarrierNode = Select<FlagCarrierNode>(flag.Entity, typeof(TankGroupComponent)).SingleOrDefault();

            if (flagCarrierNode != null && flagCarrierNode.teamGroup.Key != flag.teamGroup.Key) {
                if (self.Entity.GetComponent<TeamGroupComponent>().Key == flag.teamGroup.Key) {
                    hud.mainHUD.ShowMessageWithPriority(messages.Get().component.ReturnFlagMessage, 100);
                } else {
                    hud.mainHUD.ShowMessageWithPriority(messages.Get().component.ProtectFlagCarrierMessage, 10);
                }
            }
        }

        [OnEventFire]
        public void OnDrop(NodeAddedEvent e, HUDNodes.SelfBattleUserNode self, [JoinByBattle] [Context] [Combine] DroppedFlagNode flag, [JoinByTeam] [Context] TeamNode color, HUDNode hud,
            Optional<SingleNode<CTFHUDMessagesComponent>> messages) {
            switch (color.colorInBattle.TeamColor) {
                case TeamColor.BLUE:
                    hud.flagsHUD.BlueFlag.Drop();
                    break;

                case TeamColor.RED:
                    hud.flagsHUD.RedFlag.Drop();
                    break;
            }

            hud.flagsHUD.RequestShow();
            flag.flagInstance.FlagBeam.SetActive(true);
            HUDNodes.SelfTankNode selfTankNode = Select<HUDNodes.SelfTankNode>(self.Entity, typeof(UserGroupComponent)).FirstOrDefault();

            if (selfTankNode != null && messages.IsPresent()) {
                if (self.Entity.GetComponent<TeamGroupComponent>().Key != flag.teamGroup.Key) {
                    hud.mainHUD.RemoveMessageByPriority(10);
                    hud.mainHUD.ShowMessageWithPriority(messages.Get().component.CaptureFlagMessage);
                } else {
                    hud.mainHUD.ShowMessageWithPriority(messages.Get().component.ReturnFlagMessage);
                }
            }
        }

        [OnEventFire]
        public void OnDeliver(FlagDeliveryEvent e, FlagNode flag, [JoinByTeam] SingleNode<ColorInBattleComponent> color, [JoinByBattle] HUDNodes.SelfBattleUserNode self,
            [JoinAll] HUDNode hud, Optional<SingleNode<CTFHUDMessagesComponent>> messages) {
            switch (color.component.TeamColor) {
                case TeamColor.BLUE:
                    hud.flagsHUD.BlueFlag.TurnIn(delegate {
                        hud.flagsHUD.BlueFlagNormalizedPosition = 0f;
                        hud.flagsHUD.RequestHide();
                    });

                    break;

                case TeamColor.RED:
                    hud.flagsHUD.RedFlag.TurnIn(delegate {
                        hud.flagsHUD.RedFlagNormalizedPosition = 0f;
                        hud.flagsHUD.RequestHide();
                    });

                    break;
            }

            hud.flagsHUD.RequestShow();
            flag.flagInstance.FlagBeam.SetActive(true);
            HUDNodes.SelfTankNode selfTankNode = Select<HUDNodes.SelfTankNode>(self.Entity, typeof(UserGroupComponent)).FirstOrDefault();

            if (selfTankNode != null) {
                if (self.Entity.GetComponent<TeamGroupComponent>().Key == flag.teamGroup.Key) {
                    hud.mainHUD.RemoveMessageByPriority(100);
                } else {
                    hud.mainHUD.RemoveMessageByPriority(10);
                }

                if (messages.IsPresent()) {
                    hud.mainHUD.ShowMessageWithPriority(messages.Get().component.CaptureFlagMessage);
                }
            }
        }

        [OnEventFire]
        public void OnDeliver(FlagNotCountedDeliveryEvent e, FlagNode flag, [JoinByTeam] SingleNode<ColorInBattleComponent> color, [JoinByBattle] HUDNodes.SelfBattleUserNode self,
            [JoinAll] HUDNode hud, Optional<SingleNode<CTFHUDMessagesComponent>> messages) {
            switch (color.component.TeamColor) {
                case TeamColor.BLUE:
                    hud.flagsHUD.BlueFlag.TurnIn(delegate {
                        hud.flagsHUD.BlueFlagNormalizedPosition = 0f;
                        hud.flagsHUD.RequestHide();
                    });

                    break;

                case TeamColor.RED:
                    hud.flagsHUD.RedFlag.TurnIn(delegate {
                        hud.flagsHUD.RedFlagNormalizedPosition = 0f;
                        hud.flagsHUD.RequestHide();
                    });

                    break;
            }

            hud.flagsHUD.RequestShow();
            flag.flagInstance.FlagBeam.SetActive(true);
            HUDNodes.SelfTankNode selfTankNode = Select<HUDNodes.SelfTankNode>(self.Entity, typeof(UserGroupComponent)).FirstOrDefault();

            if (selfTankNode != null) {
                if (self.Entity.GetComponent<TeamGroupComponent>().Key == flag.teamGroup.Key) {
                    hud.mainHUD.RemoveMessageByPriority(100);
                } else {
                    hud.mainHUD.RemoveMessageByPriority(10);
                }

                if (messages.IsPresent()) {
                    hud.mainHUD.ShowMessageWithPriority(messages.Get().component.CaptureFlagMessage);
                }
            }
        }

        [OnEventFire]
        public void OnReturn(FlagReturnEvent e, FlagNode flag, [JoinByTeam] SingleNode<ColorInBattleComponent> color, [JoinByBattle] HUDNodes.SelfBattleUserNode self,
            [JoinAll] HUDNode hud, [JoinAll] Optional<SingleNode<CTFHUDMessagesComponent>> messages) {
            switch (color.component.TeamColor) {
                case TeamColor.BLUE:
                    hud.flagsHUD.BlueFlag.Return(delegate {
                        hud.flagsHUD.BlueFlagNormalizedPosition = 0f;
                        hud.flagsHUD.RequestHide();
                    });

                    break;

                case TeamColor.RED:
                    hud.flagsHUD.RedFlag.Return(delegate {
                        hud.flagsHUD.RedFlagNormalizedPosition = 0f;
                        hud.flagsHUD.RequestHide();
                    });

                    break;
            }

            hud.flagsHUD.RequestShow();
            flag.flagInstance.FlagBeam.SetActive(true);
            HUDNodes.SelfTankNode selfTankNode = Select<HUDNodes.SelfTankNode>(self.Entity, typeof(UserGroupComponent)).FirstOrDefault();

            if (selfTankNode != null) {
                if (self.Entity.GetComponent<TeamGroupComponent>().Key == flag.teamGroup.Key) {
                    hud.mainHUD.RemoveMessageByPriority(100);
                } else {
                    hud.mainHUD.RemoveMessageByPriority(10);
                }

                if (messages.IsPresent()) {
                    hud.mainHUD.ShowMessageWithPriority(messages.Get().component.CaptureFlagMessage);
                }
            }
        }

        [OnEventFire]
        public void OnMoveFlag(TimeUpdateEvent e, CarriedFlagNode flag, [JoinByTeam] SingleNode<ColorInBattleComponent> color, [JoinByBattle] HUDNodes.SelfBattleUserNode self,
            [JoinByBattle] [Combine] TeamNode team, [JoinByBattle] ICollection<FlagPedestalNode> pedestals, [JoinAll] SingleNode<FlagsHUDComponent> hud,
            [JoinAll] HUDNodes.SelfBattleUserNode self1, [JoinByBattle] ICollection<TeamNode> teams) {
            UpdateFlagPosition(flag, pedestals, teams, color.component, hud.component);
        }

        [OnEventFire]
        public void OnDropFlag(NodeAddedEvent e, [Context] [Combine] DroppedFlagNode flag, [JoinByTeam] SingleNode<ColorInBattleComponent> color,
            [JoinByBattle] HUDNodes.SelfBattleUserNode self, [JoinByBattle] ICollection<FlagPedestalNode> pedestals, SingleNode<FlagsHUDComponent> hud,
            [JoinAll] HUDNodes.SelfBattleUserNode self1, [JoinByBattle] ICollection<TeamNode> teams) {
            UpdateFlagPosition(flag, pedestals, teams, color.component, hud.component);
        }

        void UpdateFlagPosition(FlagNode flag, ICollection<FlagPedestalNode> pedestals, ICollection<TeamNode> teams, ColorInBattleComponent colorInBattle, FlagsHUDComponent hud) {
            BoxCollider boxCollider = flag.flagCollider.boxCollider;
            Vector3 point = boxCollider.transform.TransformPoint(boxCollider.center);
            FlagPedestalComponent flagPedestalComponent = null;
            FlagPedestalComponent flagPedestalComponent2 = null;
            IEnumerator<FlagPedestalNode> enumerator = pedestals.GetEnumerator();

            while (enumerator.MoveNext()) {
                FlagPedestalNode current = enumerator.Current;
                TeamNode teamNode = null;
                IEnumerator<TeamNode> enumerator2 = teams.GetEnumerator();

                while (enumerator2.MoveNext()) {
                    TeamNode current2 = enumerator2.Current;

                    if (current2.teamGroup.Key == current.teamGroup.Key) {
                        teamNode = current2;
                        break;
                    }
                }

                switch (teamNode.colorInBattle.TeamColor) {
                    case TeamColor.BLUE:
                        flagPedestalComponent2 = current.flagPedestal;
                        break;

                    case TeamColor.RED:
                        flagPedestalComponent = current.flagPedestal;
                        break;
                }
            }

            switch (colorInBattle.TeamColor) {
                case TeamColor.BLUE:
                    hud.BlueFlagNormalizedPosition = Project(flagPedestalComponent2.Position, flagPedestalComponent.Position, point);
                    break;

                case TeamColor.RED:
                    hud.RedFlagNormalizedPosition = Project(flagPedestalComponent.Position, flagPedestalComponent2.Position, point);
                    break;
            }
        }

        float Project(Vector3 from, Vector3 to, Vector3 point) {
            Vector3 vector = to - from;
            float num = vector.magnitude * 0.95f;
            vector = vector.normalized;
            Vector3 vector2 = point - from;
            float magnitude = vector2.magnitude;
            vector2 = vector2.normalized;
            float num2 = magnitude * Vector3.Dot(vector2, vector);
            return num2 / num;
        }

        [OnEventFire]
        public void OnDrop(NodeRemoveEvent e, CarriedFlagNode flag, [JoinByBattle] HUDNodes.SelfBattleUserNode self, [JoinAll] SingleNode<FlagsHUDComponent> hud) {
            hud.component.RequestHide();
        }

        [OnEventFire]
        public void OnReturn(NodeRemoveEvent e, DroppedFlagNode flag, [JoinByBattle] HUDNodes.SelfBattleUserNode self, [JoinAll] SingleNode<FlagsHUDComponent> hud) {
            hud.component.RequestHide();
        }

        public class FlagNode : Node {
            public BattleGroupComponent battleGroup;

            public FlagColliderComponent flagCollider;

            public FlagInstanceComponent flagInstance;
            public TeamGroupComponent teamGroup;
        }

        public class DroppedFlagNode : FlagNode {
            public FlagGroundedStateComponent flagGroundedState;
        }

        public class CarriedFlagNode : FlagNode {
            public TankGroupComponent tankGroup;
        }

        public class HUDNode : Node {
            public FlagsHUDComponent flagsHUD;
            public MainHUDComponent mainHUD;
        }

        public class FlagCarrierNode : Node {
            public TankComponent tank;

            public TankGroupComponent tankGroup;

            public TeamGroupComponent teamGroup;
        }

        public class TeamNode : Node {
            public ColorInBattleComponent colorInBattle;
            public TeamGroupComponent teamGroup;
        }

        public class FlagPedestalNode : Node {
            public FlagPedestalComponent flagPedestal;

            public TeamGroupComponent teamGroup;
        }
    }
}