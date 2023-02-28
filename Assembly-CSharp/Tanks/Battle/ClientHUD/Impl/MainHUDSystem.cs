using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientBattleSelect.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientNavigation.API;
using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    public class MainHUDSystem : ECSSystem {
        [OnEventFire]
        public void SetDefaultHUD(NodeAddedEvent e, SelfUser selfuser) {
            string key = "BattleHudVersion";

            if (!PlayerPrefs.HasKey(key)) {
                PlayerPrefs.SetInt(key, (int)(selfuser.Entity.Id % 2) == 0 ? 1 : 2);
            }
        }

        [OnEventFire]
        public void InitHP(NodeAddedEvent e, HUDNodes.SelfTankNode tank, SingleNode<MainHUDComponent> hud) {
            hud.component.MaxHpValue = tank.healthConfig.BaseHealth;
            hud.component.CurrentHpValue = tank.health.CurrentHealth;
        }

        [OnEventFire]
        public void InitTankIcons(NodeAddedEvent e, SingleNode<MainHUDComponent> hud, SelfUser selfUser) {
            hud.component.HullId = selfUser.userEquipment.HullId;
            hud.component.TurretId = selfUser.userEquipment.WeaponId;
        }

        [OnEventFire]
        public void InitHUD(NodeAddedEvent e, MainHUDNode hud, HUDNodes.SemiActiveSelfTankNode tank) {
            hud.mainHUD.Activate();
            hud.mainHUD.CurrentEnergyValue = 0f;

            if (!hud.Entity.HasComponent<ScreenGroupComponent>()) {
                hud.Entity.CreateGroup<ScreenGroupComponent>();
            }
        }

        [OnEventFire]
        public void AttachHudChildren(NodeAddedEvent e, MainHudScreenGroup mainHud) {
            EntityBehaviour component = mainHud.mainHUD.GetComponent<EntityBehaviour>();
            EntityBehaviour[] componentsInChildren = mainHud.mainHUD.GetComponentsInChildren<EntityBehaviour>(true);

            foreach (EntityBehaviour entityBehaviour in componentsInChildren) {
                if (!entityBehaviour.Equals(component)) {
                    AttachToScreenComponent attachToScreenComponent = entityBehaviour.gameObject.GetComponent<AttachToScreenComponent>();

                    if (attachToScreenComponent == null) {
                        attachToScreenComponent = entityBehaviour.gameObject.AddComponent<AttachToScreenComponent>();
                    }

                    attachToScreenComponent.JoinEntityBehaviour = component;
                    Entity entity = entityBehaviour.Entity;

                    if (entity != null && entityBehaviour.handleAutomaticaly) {
                        entity.RemoveComponentIfPresent<AttachToScreenComponent>();
                        entity.RemoveComponentIfPresent<ScreenGroupComponent>();
                        entity.AddComponent(attachToScreenComponent);
                    }
                }
            }
        }

        [OnEventFire]
        public void InitHUDForSpectator(NodeAddedEvent e, HUDNodes.SelfBattleUserAsSpectatorNode spectator, SingleNode<MainHUDComponent> hud) {
            hud.component.Activate();
        }

        [OnEventFire]
        public void InitHUD(NodeAddedEvent e, HUDNodes.ActiveSelfTankNode tank, SingleNode<MainHUDComponent> hud) {
            hud.component.Activate();
        }

        [OnEventFire]
        public void InitForSpectator(NodeAddedEvent e, HUDNodes.SelfBattleUserAsSpectatorNode spec, SingleNode<MainHUDComponent> hud, SingleNode<DMHUDMessagesComponent> messages) {
            hud.component.SetSpecatatorMode();
            hud.component.ShowMessageWithPriority(messages.component.SpectatorMessage);
        }

        [OnEventFire]
        public void SetTDMMessagePosition(NodeAddedEvent e, SingleNode<MainHUDComponent> hud, HUDNodes.SelfBattleUserAsSpectatorNode spec, [JoinByBattle] TDMBattleNode battle) {
            hud.component.SetMessageTDMPosition();
        }

        [OnEventFire]
        public void SetDMMessagePosition(NodeAddedEvent e, SingleNode<MainHUDComponent> hud, HUDNodes.SelfBattleUserAsSpectatorNode spec, [JoinByBattle] DMBattleNode battle) {
            hud.component.SetMessageTDMPosition();
        }

        [OnEventFire]
        public void SetCTFMessagePosition(NodeAddedEvent e, SingleNode<MainHUDComponent> hud, HUDNodes.SelfBattleUserAsSpectatorNode spec, [JoinByBattle] CTFBattleNode battle) {
            hud.component.SetMessageCTFPosition();
        }

        [OnEventFire]
        public void InitForTank(NodeAddedEvent e, HUDNodes.SelfBattleUserAsTankNode tank, SingleNode<MainHUDComponent> hud) {
            hud.component.SetTankMode();
        }

        [OnEventFire]
        public void InitHP(NodeAddedEvent e, HUDNodes.DeadSelfTankNode tank, SingleNode<MainHUDComponent> hud) {
            hud.component.CurrentHpValue = 0f;
            hud.component.CurrentEnergyValue = 0f;
        }

        [OnEventFire]
        public void ChangeHP(NodeAddedEvent e, HUDNodes.SelfTankNode tank, SingleNode<MainHUDComponent> hud, SingleNode<HPContainerComponent> hpContainer) {
            hud.component.CurrentHpValue = tank.health.CurrentHealth;
        }

        [OnEventFire]
        public void ChangeHP(HealthChangedEvent e, HUDNodes.SelfTankNode tank, [JoinAll] SingleNode<MainHUDComponent> hud) {
            hud.component.CurrentHpValue = tank.health.CurrentHealth;
        }

        [OnEventFire]
        public void UpdateTimer(UpdateEvent e, BattleNode battle, [JoinByBattle] HUDNodes.SelfBattleUserNode self, [JoinByBattle] RoundNode round, [JoinAll] MainHUDNode hud) {
            float num = battle.timeLimit.TimeLimitSec;
            float time = battle.timeLimit.WarmingUpTimeLimitSec;

            if (battle.Entity.HasComponent<BattleStartTimeComponent>()) {
                float unityTime = battle.Entity.GetComponent<BattleStartTimeComponent>().RoundStartTime.UnityTime;

                if (!round.Entity.HasComponent<RoundWarmingUpStateComponent>()) {
                    num -= Date.Now.UnityTime - unityTime;
                } else {
                    time = unityTime - Date.Now.UnityTime;
                }
            }

            hud.mainHUDTimers.Timer.Set(num);
            hud.mainHUDTimers.WarmingUpTimer.Set(time);
        }

        public class BattleScreenNode : Node {
            public BattleGroupComponent battleGroup;

            public BattleScreenComponent battleScreen;
        }

        public class MainHUDNode : Node {
            public MainHUDComponent mainHUD;

            public MainHUDTimersComponent mainHUDTimers;
        }

        public class MainHudScreenGroup : MainHUDNode {
            public ScreenGroupComponent screenGroup;
        }

        public class BattleNode : Node {
            public BattleGroupComponent battleGroup;
            public TimeLimitComponent timeLimit;
        }

        public class TeamBattleNode : BattleNode {
            public TeamBattleComponent teamBattle;
        }

        public class DMBattleNode : BattleNode {
            public DMComponent dm;
        }

        public class TDMBattleNode : BattleNode {
            public TDMComponent tdm;
        }

        public class CTFBattleNode : BattleNode {
            public CTFComponent ctf;
        }

        public class BattleWithTimeNode : BattleNode {
            public BattleStartTimeComponent battleStartTime;
        }

        public class RoundNode : Node {
            public RoundActiveStateComponent roundActiveState;
            public RoundStopTimeComponent roundStopTime;
        }

        public class SelfUser : Node {
            public SelfUserComponent selfUser;

            public UserEquipmentComponent userEquipment;
        }
    }
}