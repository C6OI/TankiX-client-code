using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientNavigation.API;

namespace Tanks.Lobby.ClientEntrance.Impl {
    [SerialVersionUID(635824350810855226L)]
    public class InviteScreenComponent : BehaviourComponent, NoScaleScreen {
        public InputFieldComponent InviteField;

        public InviteScreenComponent(InputFieldComponent inviteField) => InviteField = inviteField;
    }
}