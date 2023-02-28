using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Platform.System.Data.Exchange.ClientNetwork.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using UnityEngine;

namespace Tanks.Lobby.ClientEntrance.Impl {
    public class EntranceStatisticsSystem : ECSSystem {
        public enum EntranceSource {
            CLIENT = 0,
            STEAM = 1
        }

        [OnEventFire]
        public void InvalidLogin(NodeAddedEvent e, InvalidLoginFieldNode login, [JoinAll] SessionNode session) {
            ScheduleEvent(new IncrementRegistrationNicksEvent(login.inputField.Input), session);
        }

        [OnEventFire]
        public void ValidLogin(NodeAddedEvent e, ValidLoginFieldNode login, [JoinAll] SessionNode session) {
            ScheduleEvent(new IncrementRegistrationNicksEvent(login.inputField.Input), session);
        }

        [OnEventFire]
        public void InvalidPassword(NodeAddedEvent e, InvalidPasswordFieldNode password, [JoinAll] SessionNode session) {
            ScheduleEvent<InvalidRegistrationPasswordEvent>(session);
        }

        [OnEventFire]
        public void InvalidPasswordRepeat(NodeAddedEvent e, InvalidPasswordRepeatFieldNode password, [JoinAll] SessionNode session) {
            ScheduleEvent<InvalidRegistrationPasswordEvent>(session);
        }

        [OnEventFire]
        public void SendClientInfoStatistics(NodeAddedEvent e, UserOnlineNode userNode, [JoinAll] SessionNode session, Optional<SingleNode<SteamMarkerComponent>> steamNode) {
            ClientInfo clientInfo = new();
            clientInfo.deviceModel = SystemInfo.deviceModel;
            clientInfo.deviceName = SystemInfo.deviceName;
            clientInfo.deviceType = SystemInfo.deviceType.ToString();
            clientInfo.deviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier;
            clientInfo.graphicsDeviceName = SystemInfo.graphicsDeviceName;
            clientInfo.graphicsDeviceVendor = SystemInfo.graphicsDeviceVendor;
            clientInfo.graphicsDeviceVersion = SystemInfo.graphicsDeviceVersion;
            clientInfo.graphicsDeviceID = SystemInfo.graphicsDeviceID;
            clientInfo.graphicsDeviceType = SystemInfo.graphicsDeviceType.ToString();
            clientInfo.graphicsDeviceVendorID = SystemInfo.graphicsDeviceVendorID;
            clientInfo.graphicsMemorySize = SystemInfo.graphicsMemorySize;
            clientInfo.graphicsShaderLevel = SystemInfo.graphicsShaderLevel;
            clientInfo.operatingSystem = SystemInfo.operatingSystem;
            clientInfo.systemMemorySize = SystemInfo.systemMemorySize;
            clientInfo.processorType = SystemInfo.processorType;
            clientInfo.processorCount = SystemInfo.processorCount;
            clientInfo.processorFrequency = SystemInfo.processorFrequency;
            clientInfo.supportsLocationService = SystemInfo.supportsLocationService;
            clientInfo.qualityLevel = QualitySettings.GetQualityLevel();
            clientInfo.resolution = Screen.currentResolution.ToString();
            clientInfo.dpi = Screen.dpi;
            clientInfo.entranceSource = (steamNode.IsPresent() ? EntranceSource.STEAM : EntranceSource.CLIENT).ToString();
            ClientInfo obj = clientInfo;
            ScheduleEvent(new ClientInfoSendEvent(JsonUtility.ToJson(obj)), session);
        }

        public class SessionNode : Node {
            public ClientSessionComponent clientSession;
        }

        public class ValidLoginFieldNode : Node {
            public InputFieldComponent inputField;

            public InputFieldInvalidStateComponent inputFieldInvalidState;
            public RegistrationLoginInputComponent registrationLoginInput;
        }

        public class InvalidLoginFieldNode : Node {
            public InputFieldComponent inputField;

            public InputFieldValidStateComponent inputFieldValidState;
            public RegistrationLoginInputComponent registrationLoginInput;
        }

        public class InvalidPasswordFieldNode : Node {
            public InputFieldInvalidStateComponent inputFieldInvalidState;
            public RegistrationPasswordInputComponent registrationPasswordInput;
        }

        public class InvalidPasswordRepeatFieldNode : Node {
            public InputFieldInvalidStateComponent inputFieldInvalidState;
            public RepetitionPasswordInputComponent repetitionPasswordInput;
        }

        public class UserOnlineNode : Node {
            public SelfUserComponent selfUser;

            public UserComponent user;

            public UserGroupComponent userGroup;

            public UserOnlineComponent userOnline;
        }
    }
}