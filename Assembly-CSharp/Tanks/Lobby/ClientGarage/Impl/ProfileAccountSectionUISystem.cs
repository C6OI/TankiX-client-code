using System.Collections.Generic;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientEntrance.Impl;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientPayment.API;
using Tanks.Lobby.ClientProfile.API;
using Tanks.Lobby.ClientUserProfile.API;
using Tanks.Lobby.ClientUserProfile.Impl;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ProfileAccountSectionUISystem : ECSSystem {
        [OnEventFire]
        public void InitScreen(NodeAddedEvent e, ScreenNode screen, [JoinAll] SelfUserNode selfUser) {
            screen.profileAccountSectionUI.UserNickname = selfUser.userUid.Uid;
        }

        [OnEventFire]
        public void ViewConfirmedEmail(NodeAddedEvent e, ScreenNode screen, SelfUserWithConfirmedEmailNode user,
            [JoinAll] Optional<SelfUserWithUnconfirmedEmailNode> unconfirmedOptional) {
            string text = "%EMAIL%";
            string email = user.confirmedUserEmail.Email;
            string unconfirmedEmail = string.Empty;

            if (unconfirmedOptional.IsPresent()) {
                string text2 = GetConfirmedEmailFormatText(screen.profileAccountSectionUI) + " " + getColorFormattedEmail(screen.profileAccountSectionUI, "%UNCEMAIL%");
                text = text + "\n" + text2;
                unconfirmedEmail = unconfirmedOptional.Get().unconfirmedUserEmail.Email;
            }

            screen.profileAccountSectionUI.SetEmail(text, email, unconfirmedEmail);
        }

        [OnEventFire]
        public void ViewUnconfirmedEmail(NodeAddedEvent e, ScreenNode screen, SelfUserWithUnconfirmedEmailNode userEmail,
            [JoinAll] Optional<SelfUserWithConfirmedEmailNode> confirmedOptional) {
            string format = GetUnconfirmedEmailFormatText(screen.profileAccountSectionUI);
            string email = string.Empty;
            string email2 = userEmail.unconfirmedUserEmail.Email;

            if (confirmedOptional.IsPresent()) {
                format = "%EMAIL%\n" + GetConfirmedEmailFormatText(screen.profileAccountSectionUI) + " " + getColorFormattedEmail(screen.profileAccountSectionUI, "%UNCEMAIL%");
                email = confirmedOptional.Get().confirmedUserEmail.Email;
            }

            screen.profileAccountSectionUI.SetEmail(format, email, email2);
        }

        string GetConfirmedEmailFormatText(ProfileAccountSectionUIComponent screen) {
            string colorFormattedEmail = getColorFormattedEmail(screen);
            return string.Format("<size={0}>{1}</size>", screen.EmailMessageSize, screen.UnconfirmedLocalization.Value.Replace("%EMAIL%", colorFormattedEmail));
        }

        string GetUnconfirmedEmailFormatText(ProfileAccountSectionUIComponent screen) {
            string colorFormattedEmail = getColorFormattedEmail(screen, "%UNCEMAIL%");
            return string.Format("<size={0}>{1}</size>", screen.EmailMessageSize, screen.UnconfirmedLocalization.Value.Replace("%EMAIL%", colorFormattedEmail));
        }

        static string getColorFormattedEmail(ProfileAccountSectionUIComponent screen, string type = "%EMAIL%") =>
            string.Format("<color=#{1}>{0}</color>", type, screen.EmailColor.ToHexString());

        void ShowDialog(ConfirmDialogComponent dialog, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens) {
            List<Animator> animators = new();

            if (screens.IsPresent()) {
                animators = screens.Get().component.Animators;
            }

            dialog.Show(animators);
        }

        [OnEventFire]
        public void GoToForum(ButtonClickEvent e, SingleNode<ForumButtonComponent> button) {
            Application.OpenURL(button.component.forumUrl.Value);
        }

        [OnEventFire]
        public void SetChangeNicknamePrice(NodeAddedEvent e, SingleNode<ChangeNicknameButtonComponent> changeNicknameButton, [JoinAll] ChangeUIDNode changeUID,
            [JoinAll] SelfUserXMoneyNode selfUserXMoney) {
            if (IsFreeNickChange(selfUserXMoney)) {
                changeNicknameButton.component.XPrice = "0";
                changeNicknameButton.component.Enough = true;
            } else {
                changeNicknameButton.component.XPrice = changeUID.goodsXPrice.Price.ToString();
                changeNicknameButton.component.Enough = selfUserXMoney.userXCrystals.Money >= changeUID.goodsXPrice.Price;
            }
        }

        static bool IsFreeNickChange(SelfUserXMoneyNode selfUserXMoney) =>
            selfUserXMoney.Entity.HasComponent<SteamUserComponent>() && selfUserXMoney.Entity.GetComponent<SteamUserComponent>().FreeUidChange;

        [OnEventFire]
        public void ShowChangeNicknameDialog(ButtonClickEvent e, SingleNode<StartChangeNicknameButtonComponent> changeNicknameButton, [JoinAll] SingleNode<Dialogs60Component> dialogs,
            [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens) {
            ShowDialog(dialogs.component.Get<NicknameChangeDialog>(), screens);
        }

        [OnEventFire]
        public void ChangeNickname(ButtonClickEvent e, SingleNode<ChangeNicknameButtonComponent> changeNicknameButton, [JoinAll] LoginInputFieldValidStateNode inputField,
            [JoinAll] SelfUserXMoneyNode selfUserXMoney, [JoinAll] ChangeUIDNode changeUID, [JoinAll] SingleNode<Dialogs60Component> dialogs) {
            long money = selfUserXMoney.userXCrystals.Money;
            long num = changeUID.goodsXPrice.Price;
            bool flag = IsFreeNickChange(selfUserXMoney);

            if (money >= num || flag) {
                if (flag) {
                    num = 0L;
                }

                ScheduleEvent(new BuyUIDChangeEvent {
                    Uid = inputField.inputField.Input,
                    Price = num
                }, selfUserXMoney);
            } else {
                dialogs.component.Get<NicknameChangeDialog>().Hide();
                ShopTabManager.shopTabIndex = 3;
                MainScreenComponent.Instance.ShowHome();
                MainScreenComponent.Instance.ShowShopIfNotVisible();
            }
        }

        [OnEventFire]
        public void CompleteBuyUIDChange(CompleteBuyUIDChangeEvent e, SelfUserNode userNode, [JoinAll] ScreenNode screen,
            [JoinAll] SingleNode<ChangeNicknameButtonComponent> changeNicknameButton, [JoinAll] SingleNode<Dialogs60Component> dialogs,
            [JoinAll] Optional<SingleNode<UserProfileUI>> profileUI) {
            dialogs.component.Get<NicknameChangeDialog>().Hide();

            if (e.Success) {
                screen.profileAccountSectionUI.UserNickname = userNode.userUid.Uid;

                if (profileUI.IsPresent()) {
                    profileUI.Get().component.UpdateNickname();
                }
            }
        }

        [OnEventFire]
        public void EmailChanged(ConfirmedUserEmailChangedEvent e, SelfUserWithConfirmedEmailNode user, [JoinAll] Optional<SelfUserWithUnconfirmedEmailNode> unconfirmedOptional,
            [JoinAll] ScreenNode screen) {
            string text = "%EMAIL%";
            string email = user.confirmedUserEmail.Email;
            string unconfirmedEmail = string.Empty;

            if (unconfirmedOptional.IsPresent()) {
                string unconfirmedEmailFormatText = GetUnconfirmedEmailFormatText(screen.profileAccountSectionUI);
                text = text + "\n" + unconfirmedEmailFormatText;
                unconfirmedEmail = unconfirmedOptional.Get().unconfirmedUserEmail.Email;
            }

            screen.profileAccountSectionUI.SetEmail(text, email, unconfirmedEmail);
        }

        [OnEventFire]
        public void ShowChangeEmailDialog(ButtonClickEvent e, SingleNode<ShowChangeEmailDialogButtonComponent> changeNicknameButton, [JoinAll] SingleNode<Dialogs60Component> dialogs,
            [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens) {
            ShowDialog(dialogs.component.Get<ChangeEmailDialogComponent>(), screens);
        }

        [OnEventFire]
        public void BlockEmailConfirmationNotification(NodeAddedEvent e, SingleNode<EmailConfirmationNotificationComponent> notification,
            [JoinAll] SingleNode<ForceEnterEmailDialogComponent> dialog) {
            DeleteEntity(notification.Entity);
        }

        [OnEventComplete]
        public void SetActiveHint(NodeAddedEvent e, SingleNode<ChangeEmailDialogComponent> screen, [JoinAll] Optional<SelfUserWithConfirmedEmailNode> user) {
            screen.component.SetActiveHint(!user.IsPresent());
        }

        [OnEventFire]
        public void RequestChangeEmail(ButtonClickEvent e, SingleNode<ChangeUserEmailButtonComponent> button, [JoinByScreen] EmailInputNode emailInput,
            [JoinAll] SingleNode<SelfUserComponent> selfUser, [JoinAll] SingleNode<ChangeEmailDialogComponent> dialog) {
            ScheduleEvent(new RequestChangeUserEmailEvent(emailInput.inputField.Input), selfUser);

            if (!dialog.Entity.HasComponent<LockedScreenComponent>()) {
                dialog.Entity.AddComponent<LockedScreenComponent>();
            }
        }

        [OnEventFire]
        public void RequestChangeEmail(ButtonClickEvent e, SingleNode<ChangeUserEmailButtonComponent> button, [JoinByScreen] EmailInputNode emailInput,
            [JoinAll] SingleNode<SelfUserComponent> selfUser, [JoinAll] SingleNode<ForceEnterEmailDialogComponent> dialog) {
            ScheduleEvent(new RequestChangeUserEmailEvent(emailInput.inputField.Input), selfUser);

            if (!dialog.Entity.HasComponent<LockedScreenComponent>()) {
                dialog.Entity.AddComponent<LockedScreenComponent>();
            }
        }

        [OnEventFire]
        public void Proceed(EmailVacantEvent e, Node any, [JoinAll] LockedChangeEmailDialog lockedDialog, [JoinAll] SingleNode<SelfUserComponent> user) {
            string email = !user.Entity.HasComponent<ConfirmedUserEmailComponent>() ? e.Email : user.Entity.GetComponent<ConfirmedUserEmailComponent>().Email;
            lockedDialog.changeEmailDialog.ShowEmailConfirm(email);
        }

        [OnEventFire]
        public void UnlockScreen(EmailInvalidEvent e, Node any, [JoinAll] LockedChangeEmailDialog screen, [JoinByScreen] EmailInputNode emailInput) {
            if (screen.Entity.HasComponent<LockedScreenComponent>()) {
                screen.Entity.RemoveComponent<LockedScreenComponent>();
            }

            emailInput.esm.Esm.ChangeState<InputFieldStates.InvalidState>();
        }

        [OnEventFire]
        public void UnlockScreen(EmailOccupiedEvent e, Node any, [JoinAll] LockedChangeEmailDialog screen, [JoinByScreen] EmailInputNode emailInput) {
            if (screen.Entity.HasComponent<LockedScreenComponent>()) {
                screen.Entity.RemoveComponent<LockedScreenComponent>();
            }

            emailInput.esm.Esm.ChangeState<InputFieldStates.InvalidState>();
        }

        [OnEventFire]
        public void SendAgain(ButtonClickEvent e, SingleNode<SendAgainButtonComponent> button, [JoinAll] SelfUserWithUnconfirmedEmailNode user) {
            ScheduleEvent<RequestSendAgainConfirmationEmailEvent>(user);
        }

        [OnEventFire]
        public void GoToEmailScreen(NotificationClickEvent e, SingleNode<EmailConfirmationNotificationComponent> notification, [JoinAll] SingleNode<SelfUserComponent> selfUser,
            [JoinAll] SingleNode<Dialogs60Component> dialogs, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens) {
            if (selfUser.Entity.HasComponent<UnconfirmedUserEmailComponent>()) {
                EmailConfirmDialog emailConfirmDialog = dialogs.component.Get<EmailConfirmDialog>();
                emailConfirmDialog.ShowEmailConfirm(selfUser.Entity.GetComponent<UnconfirmedUserEmailComponent>().Email);
                ShowDialog(emailConfirmDialog, screens);
            } else {
                ChangeEmailDialogComponent dialog = dialogs.component.Get<ChangeEmailDialogComponent>();
                ShowDialog(dialog, screens);
            }
        }

        [OnEventComplete]
        public void ShowForceEnterEmailDialog(NodeAddedEvent e, SelfUserRequiredEmailNode require, [JoinAll] [Context] MainScreenNode mainScreen,
            [JoinAll] SingleNode<Dialogs60Component> dialogs, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens) {
            ForceEnterEmailDialogComponent dialog = dialogs.component.Get<ForceEnterEmailDialogComponent>();
            ShowDialog(dialog, screens);
        }

        [OnEventFire]
        public void Proceed(EmailVacantEvent e, Node any, [JoinAll] LockedForceChangeEmailDialog lockedDialog) {
            lockedDialog.forceEnterEmailDialog.Hide();
        }

        [OnEventFire]
        public void ExitFromGame(ButtonClickEvent e, SingleNode<ExitTheGameButtonComponent> exitButton, [JoinAll] SingleNode<Dialogs60Component> dialogs,
            [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens) {
            List<Animator> animators = new();

            if (screens.IsPresent()) {
                animators = screens.Get().component.Animators;
            }

            ForceEnterEmailDialogComponent forceEnterEmailDialogComponent = dialogs.component.Get<ForceEnterEmailDialogComponent>();
            forceEnterEmailDialogComponent.Hide();
            ExitGameDialog exitGameDialog = dialogs.component.Get<ExitGameDialog>();
            exitGameDialog.Show(animators);
        }

        [OnEventFire]
        public void Exit(DialogDeclineEvent e, SingleNode<ExitGameDialog> exitDialog, [JoinAll] SingleNode<RequireEnterEmailComponent> require,
            [JoinAll] SingleNode<Dialogs60Component> dialogs, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens) {
            ForceEnterEmailDialogComponent dialog = dialogs.component.Get<ForceEnterEmailDialogComponent>();
            ShowDialog(dialog, screens);
        }

        [OnEventFire]
        public void ShowPromocodeDialog(ButtonClickEvent e, SingleNode<ShowPromocodeDialogButtonComponent> showPromocodeButton, [JoinAll] SingleNode<Dialogs60Component> dialogs,
            [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens) {
            ShowDialog(dialogs.component.Get<PromocodeDialog>(), screens);
        }

        [OnEventFire]
        public void OpenChangeCountryDialog(ButtonClickEvent e, SingleNode<OpenSelectCountryButtonComponent> button, [JoinAll] SingleNode<Dialogs60Component> dialogs,
            [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens) {
            SelectCountryDialogComponent dialog = dialogs.component.Get<SelectCountryDialogComponent>();
            ShowDialog(dialog, screens);
        }

        public class MainScreenNode : Node {
            public ActiveScreenComponent activeScreen;
            public MainScreenComponent mainScreen;
        }

        public class ChangeUIDNode : Node {
            public ChangeUIDComponent changeUid;

            public GoodsXPriceComponent goodsXPrice;
        }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;
            public UserComponent user;

            public UserUidComponent userUid;
        }

        public class SelfUserXMoneyNode : SelfUserNode {
            public UserXCrystalsComponent userXCrystals;
        }

        public class ScreenNode : Node {
            public ProfileAccountSectionUIComponent profileAccountSectionUI;
        }

        public class LoginInputFieldValidStateNode : Node {
            public InputFieldComponent inputField;

            public InputFieldValidStateComponent inputFieldValidState;

            public RegistrationLoginInputComponent registrationLoginInput;
        }

        public class EmailInputNode : Node {
            public EmailInputFieldComponent emailInputField;
            public ESMComponent esm;

            public InputFieldComponent inputField;

            public InputFieldValidStateComponent inputFieldValidState;
        }

        public class LockedChangeEmailDialog : Node {
            public ChangeEmailDialogComponent changeEmailDialog;

            public LockedScreenComponent lockedScreen;
        }

        public class LockedForceChangeEmailDialog : Node {
            public ForceEnterEmailDialogComponent forceEnterEmailDialog;

            public LockedScreenComponent lockedScreen;
        }

        public class SelfUserWithConfirmedEmailNode : Node {
            public ConfirmedUserEmailComponent confirmedUserEmail;

            public SelfUserComponent selfUser;
        }

        public class SelfUserWithUnconfirmedEmailNode : Node {
            public SelfUserComponent selfUser;
            public UnconfirmedUserEmailComponent unconfirmedUserEmail;
        }

        public class SelfUserRequiredEmailNode : Node {
            public RequireEnterEmailComponent requireEnterEmail;
            public SelfUserComponent selfUser;
        }
    }
}