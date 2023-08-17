using System;
using System.Linq;
using Lobby.ClientControls.API;
using Lobby.ClientEntrance.API;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Lobby.ClientEntrance.Impl {
    public class SaveLoginSystem : ECSSystem {
        public const string LOGIN_PLAYERPREFS_KEY = "PlayerLogin";

        public const string REMEMBERME_PLAYERPREFS_KEY = "RemeberMeFlag";

        [OnEventFire]
        public void SaveLogin(NodeAddedEvent e, SelfUserNode node) => PlayerPrefs.SetString("PlayerLogin", node.userUid.Uid);

        [OnEventFire]
        public void SaveLogin(UIDChangedEvent e, SelfUserNode node) =>
            PlayerPrefs.SetString("PlayerLogin", node.userUid.Uid);

        [OnEventComplete]
        public void RetrieveLogin(NodeAddedEvent e, LoginInputFieldNode loginInput,
            [JoinByScreen] [Context] PasswordInputFieldNode passwordInput) {
            string commandlineParam = GetCommandlineParam("login", PlayerPrefs.GetString("PlayerLogin"));

            if (!string.IsNullOrEmpty(commandlineParam)) {
                loginInput.inputField.Input = commandlineParam;
                passwordInput.inputField.InputField.Select();
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
        public void StoreRemeberMeOption(ButtonClickEvent e, SingleNode<LoginButtonComponent> loginButton,
            [JoinByScreen] SingleNode<EntranceScreenComponent> entranceScreen) =>
            PlayerPrefs.SetInt("RemeberMeFlag", entranceScreen.component.RememberMe ? 1 : 0);

        [OnEventComplete]
        public void RetrievePassword(NodeAddedEvent e, PasswordInputFieldNode passwordInput,
            [JoinByScreen] [Context] SingleNode<EntranceScreenComponent> entranceScreen) {
            string commandlineParam = GetCommandlineParam("password", string.Empty);

            if (!string.IsNullOrEmpty(commandlineParam)) {
                passwordInput.inputField.Input = commandlineParam;
                entranceScreen.component.RememberMe = false;
            }
        }

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