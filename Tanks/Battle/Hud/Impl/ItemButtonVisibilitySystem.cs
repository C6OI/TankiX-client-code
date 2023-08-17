using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientHUD.Impl;

namespace Tanks.Battle.Hud.Impl {
    public class ItemButtonVisibilitySystem : ECSSystem {
        public const string ANIMATION_PARAM_VISIBLE = "IsTab";

        [OnEventFire]
        public void OnTabPressed(NodeAddedEvent e, TabPressedNode tabPressedNode) =>
            tabPressedNode.itemButton.Animator.SetBool("IsTab", true);

        [OnEventFire]
        public void OnTabReleased(NodeRemoveEvent e, TabPressedNode tabPressedNode) =>
            tabPressedNode.itemButton.Animator.SetBool("IsTab", false);

        public class TabPressedNode : Node {
            public ItemButtonComponent itemButton;
            public TabPressedComponent tabPressed;
        }
    }
}