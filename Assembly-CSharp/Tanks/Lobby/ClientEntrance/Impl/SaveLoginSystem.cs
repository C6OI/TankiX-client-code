using System;
using System.Linq;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Exchange.ClientNetwork.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientNavigation.API;
using UnityEngine;

namespace Tanks.Lobby.ClientEntrance.Impl {
    public class SaveLoginSystem : ECSSystem {
        public const string LOGIN_PLAYERPREFS_KEY = "PlayerLogin";

        public const string REMEMBERME_PLAYERPREFS_KEY = "RemeberMeFlag";

        [OnEventFire]
        public void SaveLogin(NodeAddedEvent e, SelfUserNode node) {
            PlayerPrefs.SetString("PlayerLogin", node.userUid.Uid);
        }

        [OnEventFire]
        public void SaveLogin(UIDChangedEvent e, SelfUserNode node) {
            PlayerPrefs.SetString("PlayerLogin", node.userUid.Uid);
        }

        [OnEventComplete]
        public void RetrieveLogin(NodeAddedEvent e, LoginInputFieldNode loginInput, [Context] [JoinByScreen] PasswordInputFieldNode passwordInput,
            [JoinAll] SingleNode<ClientSessionComponent> clientSessionNode) {
            string savedLogin = GetSavedLogin();

            if (!string.IsNullOrEmpty(savedLogin)) {
                loginInput.inputField.Input = savedLogin;
                SelectPasswordField(passwordInput);
            }
        }

        [OnEventFire]
        public void FillLoginFromWebId(NodeAddedEvent e, LoginInputFieldNode loginInput, [Context] [JoinByScreen] PasswordInputFieldNode passwordInput,
            [JoinAll] SingleNode<ScreensRegistryComponent> screenRegistry, [JoinAll] SingleNode<WebIdComponent> clientSession) {
            string savedLogin = GetSavedLogin();

            if (string.IsNullOrEmpty(savedLogin)) {
                string webIdUid = clientSession.component.WebIdUid;

                if (!string.IsNullOrEmpty(webIdUid)) {
                    loginInput.inputField.Input = webIdUid;
                    SelectPasswordField(passwordInput);
                }
            }
        }

        void SelectPasswordField(PasswordInputFieldNode passwordInput) {
            InputFieldComponent inputField = passwordInput.inputField;

            if (inputField.InputField != null) {
                inputField.InputField.Select();
            } else {
                inputField.TMPInputField.Select();
            }
        }

        [OnEventFire]
        public void SetRemeberMeOptionOnLoad(NodeAddedEvent e, SingleNode<EntranceScreenComponent> entranceScreen) {
            if (PlayerPrefs.HasKey("RemeberMeFlag")) {
                entranceScreen.component.RememberMe = PlayerPrefs.GetInt("RemeberMeFlag") != 0;
            } else {
                entranceScreen.component.RememberMe = true;
            }
        }

        [OnEventFire]
        public void StoreRemeberMeOption(ButtonClickEvent e, SingleNode<LoginButtonComponent> loginButton, [JoinByScreen] SingleNode<EntranceScreenComponent> entranceScreen) {
            PlayerPrefs.SetInt("RemeberMeFlag", entranceScreen.component.RememberMe ? 1 : 0);
        }

        [OnEventComplete]
        public void RetrievePassword(NodeAddedEvent e, PasswordInputFieldNode passwordInput, [Context] [JoinByScreen] SingleNode<EntranceScreenComponent> entranceScreen) {
            string commandlineParam = GetCommandlineParam("password", string.Empty);

            if (!string.IsNullOrEmpty(commandlineParam)) {
                passwordInput.inputField.Input = commandlineParam;
                entranceScreen.component.RememberMe = false;
            }
        }

        public static string GetSavedLogin() => GetCommandlineParam("login", PlayerPrefs.GetString("PlayerLogin"));

        static string GetCommandlineParam(string paramName, string defaultValue) {
            string[] commandLineArgs = Environment.GetCommandLineArgs();
            string result = defaultValue;
            string paramWithSeparator = paramName + "=";
            string text = commandLineArgs.FirstOrDefault(arg => arg.StartsWith(paramWithSeparator));

            if (!string.IsNullOrEmpty(text)) {
                result = text.Substring(paramWithSeparator.Length);
            }

            return result;
        }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;
            public UserComponent user;

            public UserGroupComponent userGroup;

            public UserUidComponent userUid;
        }

        public class LoginInputFieldNode : Node {
            public ESMComponent esm;

            public InputFieldComponent inputField;

            public InteractivityPrerequisiteStateComponent interactivityPrerequisiteState;
            public LoginInputFieldComponent loginInputField;
        }

        public class PasswordInputFieldNode : Node {
            public ESMComponent esm;

            public InputFieldComponent inputField;

            public InteractivityPrerequisiteStateComponent interactivityPrerequisiteState;
            public PasswordInputFieldComponent passwordInputField;
        }
    }
}