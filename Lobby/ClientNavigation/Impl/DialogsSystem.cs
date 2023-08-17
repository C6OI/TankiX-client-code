using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Lobby.ClientNavigation.Impl {
    public class DialogsSystem : ECSSystem {
        [OnEventFire]
        public void CloseDialog(NodeRemoveEvent e, SingleNode<ActiveScreenComponent> screen,
            [JoinAll] SingleNode<DialogsComponent> dialogs) => dialogs.component.CloseAll();

        [OnEventComplete]
        public void MergeDialogs(NodeAddedEvent e, SingleNode<DialogsComponent> newDialogs,
            [JoinAll] [Combine] SingleNode<DialogsComponent> dialogs) {
            if (newDialogs.Entity != dialogs.Entity) {
                while (newDialogs.component.transform.childCount > 0) {
                    Transform child = newDialogs.component.transform.GetChild(0);
                    child.SetParent(dialogs.component.transform, false);
                }

                Object.Destroy(newDialogs.component.gameObject);
            }
        }
    }
}