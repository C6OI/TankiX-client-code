using Lobby.ClientControls.API;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientGarage.API;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class WeaponGarageItemsScreenNavigationSystem : ECSSystem {
        [OnEventFire]
        public void InitShellsNavigation(NodeAddedEvent evt, SingleNode<ShellsButtonComponent> shellsButton) =>
            shellsButton.Entity.AddComponent(new ShellsButtonNavigationComponent(false));

        [OnEventFire]
        public void RecalculateShellsButtonNavigation(NodeAddedEvent evt, [Combine] NotShellsButtonNode notShellsButton,
            [JoinByScreen] [Context] ShellsButtonNavigationNode shellsButtonNavigation) =>
            ScheduleRecalculateNavigation(shellsButtonNavigation);

        [OnEventFire]
        public void RecalculateShellsButtonNavigation(NodeRemoveEvent evt, NotShellsButtonNode notShellsButton,
            [JoinByScreen] ShellsButtonNavigationNode shellsButtonNavigation) =>
            ScheduleRecalculateNavigation(shellsButtonNavigation);

        [OnEventFire]
        public void SetNavigationFromShellsButtonToMountButton(RecalculateShellsButtonNavigationEvent evt,
            ShellsButtonNavigationNode shellsButtonNavigation, [JoinByScreen] MountItemButtonNode mountButton) =>
            SetNavigationForShellsButton(shellsButtonNavigation, null, mountButton.mountItemButton.GetComponent<Button>());

        [OnEventFire]
        public void SetNavigationFromShellsButtonToBuyButton(RecalculateShellsButtonNavigationEvent evt,
            ShellsButtonNavigationNode shellsButtonNavigation, [JoinByScreen] BuyItemButtonNode buyItemButton) =>
            SetNavigationForShellsButton(shellsButtonNavigation, null, buyItemButton.confirmButton.button);

        [OnEventFire]
        public void SetNavigationFromShellsButtonToUpgradeButton(RecalculateShellsButtonNavigationEvent evt,
            ShellsButtonNavigationNode shellsButtonNavigation, [JoinByScreen] UpgradeButtonNode upgradeButton,
            [JoinByScreen] Optional<MountItemButtonNode> mountButton) {
            Button component = upgradeButton.upgradeButton.GetComponent<Button>();
            Navigation navigation = component.navigation;

            if (mountButton.IsPresent()) {
                Button selectOnDown =
                    (Button)(navigation.selectOnUp = mountButton.Get().mountItemButton.GetComponent<Button>());

                component.navigation = navigation;
                SetNavigationForShellsButton(shellsButtonNavigation, component, selectOnDown);
            } else {
                navigation.selectOnUp = shellsButtonNavigation.shellsButton.GetComponent<Button>();
                component.navigation = navigation;
                SetNavigationForShellsButton(shellsButtonNavigation, component, component);
            }
        }

        [OnEventFire]
        public void SetNavigationFromShellsButtonToPropertiesButton(RecalculateShellsButtonNavigationEvent evt,
            ShellsButtonNavigationNode shellsButtonNavigation,
            [JoinByScreen] ItemPropertiesButtonNode itemPropertiesButton) =>
            SetNavigationForShellsButton(shellsButtonNavigation,
                itemPropertiesButton.itemPropertiesButton.GetComponent<Button>());

        void ScheduleRecalculateNavigation(ShellsButtonNavigationNode shellsButtonNavigation) {
            ShellsButtonNavigationComponent shellsButtonNavigation2 = shellsButtonNavigation.shellsButtonNavigation;

            if (!shellsButtonNavigation2.RecalculateNavigationScheduled) {
                shellsButtonNavigation2.RecalculateNavigationScheduled = true;
                NewEvent<RecalculateShellsButtonNavigationEvent>().Attach(shellsButtonNavigation).ScheduleDelayed(0f);
            }
        }

        void SetNavigationForShellsButton(ShellsButtonNavigationNode shellsButtonNavigationNode, Button selectOnUp = null,
            Button selectOnDown = null) {
            shellsButtonNavigationNode.shellsButtonNavigation.RecalculateNavigationScheduled = false;
            Button component = shellsButtonNavigationNode.shellsButton.GetComponent<Button>();
            Navigation navigation = component.navigation;

            if (selectOnUp != null) {
                navigation.selectOnUp = selectOnUp;
            }

            if (selectOnDown != null) {
                navigation.selectOnDown = selectOnDown;
            }

            component.navigation = navigation;
        }

        public class ShellsButtonNavigationNode : Node {
            public ScreenGroupComponent screenGroup;
            public ShellsButtonComponent shellsButton;

            public ShellsButtonNavigationComponent shellsButtonNavigation;
        }

        [Not(typeof(ShellsButtonComponent))]
        public class NotShellsButtonNode : Node {
            public ButtonMappingComponent buttonMapping;

            public ScreenGroupComponent screenGroup;
        }

        public class MountItemButtonNode : Node {
            public MountItemButtonComponent mountItemButton;

            public ScreenGroupComponent screenGroup;
        }

        public class ItemPropertiesButtonNode : Node {
            public ItemPropertiesButtonComponent itemPropertiesButton;

            public ScreenGroupComponent screenGroup;
        }

        public class BuyItemButtonNode : Node {
            public BuyButtonComponent buyButton;

            public ConfirmButtonComponent confirmButton;

            public ScreenGroupComponent screenGroup;
        }

        public class UpgradeButtonNode : Node {
            public ScreenGroupComponent screenGroup;
            public UpgradeButtonComponent upgradeButton;
        }
    }
}