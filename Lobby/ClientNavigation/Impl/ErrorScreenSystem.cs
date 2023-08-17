using Lobby.ClientNavigation.API;
using Lobby.ClientNavigation.API.Screens;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Lobby.ClientNavigation.Impl {
    public class ErrorScreenSystem : ECSSystem {
        [OnEventFire]
        public void FillText(NodeAddedEvent e, SingleNode<ErrorScreenComponent> errorScreen,
            [Context] [JoinByScreen] SingleNode<ErrorScreenTextComponent> errorData) {
            errorScreen.component.Description = errorData.component.ErrorText;
            errorScreen.component.ButtonLabel = errorData.component.ButtonLabel;
            errorScreen.component.ReportButtonLabel = errorData.component.ReportButtonLabel;
        }

        [OnEventFire]
        public void DoButtonClickAction(ButtonClickEvent e, SingleNode<ButtonMappingComponent> button,
            [JoinByScreen] SingleNode<ErrorScreenActionComponent> errorData) => errorData.component.Action();

        [OnEventFire]
        public void DeleteContextEntity(NodeRemoveEvent e, SingleNode<ErrorScreenComponent> screen,
            [JoinByScreen] SingleNode<ErrorScreenTextComponent> context) => DeleteEntity(context.Entity);
    }
}