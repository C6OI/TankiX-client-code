using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Lobby.ClientNavigation.Impl {
    public class ReportButtonSystem : ECSSystem {
        [OnEventFire]
        public void NavigateToReport(ButtonClickEvent e, SingleNode<ReportButtonComponent> button,
            [JoinByScreen] SingleNode<ErrorScreenTextComponent> errorData) =>
            Application.OpenURL(errorData.component.ReportUrl);
    }
}