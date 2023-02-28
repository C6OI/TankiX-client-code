using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientEntrance.Impl {
    [Shared]
    [SerialVersionUID(1439808320725L)]
    public class InviteComponent : SharedChangeableComponent {
        string inviteCode;

        public bool ShowScreenOnEntrance { get; set; }

        [ProtocolOptional] public string InviteCode {
            get => inviteCode;
            set {
                inviteCode = value;
                OnChange();
            }
        }
    }
}