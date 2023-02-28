using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientUserProfile.Impl;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class ReturnToBattleSystem : ECSSystem {
        const float TIMEOUT = 0.05f;

        [OnEventFire]
        public void TryShowReturnToBattleDialog(NodeAddedEvent e, HomeScreenNode homeScreen, [JoinAll] SelfUserWithReservNode user) {
            NewEvent<TryShowDialog>().Attach(user).ScheduleDelayed(0.05f);
        }

        [OnEventFire]
        public void HideReturnToBattleDialog(NodeRemoveEvent e, SelfUserWithReservNode user, [JoinAll] DialogNode dialog, [JoinAll] HomeScreenNode homeScreen) {
            dialog.modalConfirmWindow.Hide();
        }

        [OnEventFire]
        public void TurnOffTutorial(NodeAddedEvent e, SelfUserWithReservNode user) {
            if (!user.Entity.HasComponent<TurnOffTutorialUserComponent>()) {
                user.Entity.AddComponent<TurnOffTutorialUserComponent>();
            }
        }

        [OnEventFire]
        public void TurnOnTutorial(NodeRemoveEvent e, SelfUserWithReservNode user) {
            if (user.Entity.HasComponent<TurnOffTutorialUserComponent>()) {
                user.Entity.RemoveComponent<TurnOffTutorialUserComponent>();
            }

            if (!user.Entity.HasComponent<BattleGroupComponent>()) {
                NewEvent<TryResumeTutorialEvent>().Attach(user).ScheduleDelayed(0.05f);
            }
        }

        [OnEventFire]
        public void TryResumeTutorialOnReleaseReservation(ReleaseReservationInBattleEvent e, SelfUserWithReservNode user,
            [JoinAll] [Combine] SingleNode<TutorialShowTriggerComponent> tutorialTrigger) {
            if (user.Entity.HasComponent<TurnOffTutorialUserComponent>()) {
                user.Entity.RemoveComponent<TurnOffTutorialUserComponent>();
            }

            NewEvent<TryResumeTutorialEvent>().Attach(user).ScheduleDelayed(0.05f);
        }

        [OnEventFire]
        public void TryResumeTutorial(TryResumeTutorialEvent e, Node any, [JoinAll] [Combine] SingleNode<TutorialShowTriggerComponent> tutorialTrigger) {
            if (tutorialTrigger.component.triggerType == TutorialShowTriggerComponent.EventTriggerType.Awake ||
                tutorialTrigger.component.triggerType == TutorialShowTriggerComponent.EventTriggerType.ClickAnyWhereOrDelay) {
                tutorialTrigger.component.Triggered();
            }
        }

        [OnEventFire]
        public void ShowReturnToBattleDialog(TryShowDialog e, SelfUserWithReservNode user, [JoinAll] HomeScreenNode homeScreen, [JoinAll] SingleNode<Dialogs60Component> dialogs) {
            if (!Flow.Current.EntityRegistry.ContainsEntity(user.reservedInBattleInfo.Map)) {
                Log.ErrorFormat("map={0} not found on reservation in battle for user={1}", user.reservedInBattleInfo.Map, user);
            } else if (!TutorialCanvas.Instance.IsShow) {
                Entity entity = Flow.Current.EntityRegistry.GetEntity(user.reservedInBattleInfo.Map);
                string name = entity.GetComponent<DescriptionItemComponent>().Name;
                ReturnToBattleDialogComponent returnToBattleDialogComponent = dialogs.component.Get<ReturnToBattleDialogComponent>();
                ModalConfirmWindowComponent component = returnToBattleDialogComponent.gameObject.GetComponent<ModalConfirmWindowComponent>();
                component.Show(homeScreen.Entity);
                returnToBattleDialogComponent.PreformatedMainText = string.Format(component.MainText, name, user.reservedInBattleInfo.BattleMode);
                component.MainText = InsertLeftTime(returnToBattleDialogComponent.PreformatedMainText, (int)(user.reservedInBattleInfo.ExitTime - Date.Now));
            }
        }

        [OnEventFire]
        public void UpdateTimer(UpdateEvent e, DialogNode dialog, [JoinAll] HomeScreenNode homeScreen, [JoinAll] SelfUserWithReservNode user) {
            int num = (int)(user.reservedInBattleInfo.ExitTime - Date.Now);

            if (dialog.returnToBattleDialog.SecondsLeft != num) {
                dialog.returnToBattleDialog.SecondsLeft = num;
                dialog.modalConfirmWindow.MainText = InsertLeftTime(dialog.returnToBattleDialog.PreformatedMainText, num);

                if (dialog.returnToBattleDialog.SecondsLeft <= 0) {
                    dialog.modalConfirmWindow.Hide();
                    ScheduleEvent<ReleaseReservationInBattleEvent>(user);
                }
            }
        }

        string InsertLeftTime(string template, int time) => template.Replace("[LeftTime]", time.ToString());

        [OnEventFire]
        public void ReturnToBattle(DialogConfirmEvent e, DialogNode dialog, [JoinAll] SelfUserWithReservNode user, [JoinAll] SingleNode<ActiveScreenComponent> screen) {
            screen.Entity.AddComponent<LockedScreenComponent>();
            ScheduleEvent<ReturnToBattleEvent>(user);
        }

        [OnEventFire]
        public void ReleaseReservation(DialogDeclineEvent e, SingleNode<ReturnToBattleDialogComponent> dialog, [JoinAll] SelfUserWithReservNode user) {
            ScheduleEvent<ReleaseReservationInBattleEvent>(user);
        }

        [OnEventFire]
        public void OnReturnFailed(ReturnToBattleFiledEvent e, SingleNode<SelfUserComponent> user) { }

        [OnEventFire]
        public void DeleteEmailConfirmationNotification(NodeAddedEvent e, SingleNode<EmailConfirmationNotificationComponent> notification, [JoinAll] SelfUserWithReservNode user) {
            DeleteEntity(notification.Entity);
        }

        public class HomeScreenNode : Node {
            public ActiveScreenComponent activeScreen;

            public MainScreenComponent mainScreen;
            public ScreenComponent screen;
        }

        public class SelfUserWithReservNode : Node {
            public ReservedInBattleInfoComponent reservedInBattleInfo;
            public SelfUserComponent selfUser;
        }

        public class DialogNode : Node {
            public ModalConfirmWindowComponent modalConfirmWindow;

            public ReturnToBattleDialogComponent returnToBattleDialog;
        }

        public class TryShowDialog : Event { }

        public class TryResumeTutorialEvent : Event { }
    }
}