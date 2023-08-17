using Lobby.ClientControls.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientGarage.Impl {
    public class BlockKeyboardForListOnConfirmButtonSystem : ECSSystem {
        [OnEventFire]
        public void Block(ConfirmButtonClickEvent e, Node node,
            [Combine] [JoinAll] SingleNode<SimpleHorizontalListComponent> list) =>
            list.component.IsKeyboardNavigationAllowed = false;

        [OnEventFire]
        public void Unblock(ConfirmButtonClickNoEvent e, Node node,
            [JoinAll] [Combine] SingleNode<SimpleHorizontalListComponent> list) =>
            list.component.IsKeyboardNavigationAllowed = true;

        [OnEventFire]
        public void Unblock(ConfirmButtonClickYesEvent e, Node node,
            [Combine] [JoinAll] SingleNode<SimpleHorizontalListComponent> list) =>
            list.component.IsKeyboardNavigationAllowed = true;
    }
}