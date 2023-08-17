using Lobby.ClientControls.API;
using Platform.Library.ClientProtocol.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Lobby.ClientEntrance.Impl {
    [SerialVersionUID(635824350810855226L)]
    public class InviteScreenComponent : MonoBehaviour, Component {
        public InputFieldComponent InviteField;

        public InviteScreenComponent() { }

        public InviteScreenComponent(InputFieldComponent inviteField) => InviteField = inviteField;
    }
}