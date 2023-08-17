using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientEntrance.Impl {
    [Shared]
    [SerialVersionUID(1439808320725L)]
    public class InviteComponent : SharedChangeableComponent {
        string inviteCode;

        [ProtocolOptional] public string InviteCode {
            get => inviteCode;
            set {
                inviteCode = value;
                OnChange();
            }
        }
    }
}