using Platform.Kernel.ECS.ClientEntitySystem.API;
using UnityEngine;

namespace Lobby.ClientPayment.Impl {
    public class GoToUrlSystem : ECSSystem {
        [OnEventFire]
        public void OpenUrl(GoToUrlToPayEvent e, Node node) {
            Application.OpenURL(e.Url);
        }
    }
}