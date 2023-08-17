using Lobby.ClientControls.API;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Exchange.ClientNetwork.API;

namespace Lobby.ClientEntrance.Impl {
    public class ChangeUserNameScreenSystem : ECSSystem {
        [OnEventFire]
        public void ChangeUserName(ButtonClickEvent e, SingleNode<ChangeUserNameButtonComponent> node,
            [JoinByScreen] LoginInput loginInput, [JoinAll] SingleNode<ClientSessionComponent> clientSession) =>
            ScheduleEvent(new ChangeUsernameEvent(loginInput.inputField.Input), clientSession);

        public class LoginInput : Node {
            public InputFieldComponent inputField;
            public RegistrationLoginInputComponent registrationLoginInput;
        }
    }
}