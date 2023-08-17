using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientHUD.API;

namespace Tanks.Battle.ClientHUD.Impl {
    public class UIElementsVisibilityControllerSystem : ECSSystem {
        readonly string NORMALLY_VISIBLE_PREREQUISITE = "NORMALLY_VISIBLE_PREREQUISITE";

        readonly string TAB_PRESSED_PREREQUISITE = "TAB_PRESSED_PREREQUISITE";

        readonly string TAB_PRESSED_WHILE_IN_ACTIVE_STATE = "TAB_PRESSED_WHILE_IN_ACTIVE_STATE";

        readonly string TANK_INACTIVE_PREREQUISITE = "TANK_INACTIVE_PREREQUISITE";

        readonly string VISIBLE_PERIOD_PREREQUISITE = "VISIBLE_PERIOD_PREREQUISITE";

        [OnEventFire]
        public void ShowUIElementIfTankDead(NodeAddedEvent e, DeadSelfTankNode deadSelfTank,
            [Combine] ShowWhileTankInactiveNode uiElement) =>
            uiElement.visibilityPrerequisites.AddShowPrerequisite(TANK_INACTIVE_PREREQUISITE);

        [OnEventFire]
        public void ShowUIElementIfTankSpawn(NodeAddedEvent e, SpawnSelfTankNode spawnSelfTank,
            [Combine] ShowWhileTankInactiveNode uiElement) =>
            uiElement.visibilityPrerequisites.AddShowPrerequisite(TANK_INACTIVE_PREREQUISITE);

        [OnEventFire]
        public void ShowUIElementIfTankSemiActive(NodeAddedEvent e, SemiActiveSelfTankNode semiActiveSelfTank,
            [Combine] ShowWhileTankInactiveNode uiElement) =>
            uiElement.visibilityPrerequisites.AddShowPrerequisite(TANK_INACTIVE_PREREQUISITE);

        [OnEventFire]
        public void HideUIElementIfTankActivated(NodeAddedEvent e, ActiveSelfTankNode activeSelfTank,
            [Combine] [JoinAll] ShowWhileTankInactiveNode uiElement) =>
            uiElement.visibilityPrerequisites.RemoveShowPrerequisite(TANK_INACTIVE_PREREQUISITE);

        [OnEventFire]
        public void ShowUIElementIfTabPressed(NodeAddedEvent e, ShowWhileTabPressedNode uiElement) =>
            uiElement.visibilityPrerequisites.AddShowPrerequisite(TAB_PRESSED_PREREQUISITE, true);

        [OnEventFire]
        public void HideUIElementIfTabPressed(NodeAddedEvent e, HideWhileTabPressedNode uiElement) =>
            uiElement.visibilityPrerequisites.AddHidePrerequisite(TAB_PRESSED_PREREQUISITE, true);

        [OnEventFire]
        public void ShowUIElementIfTabPressed(NodeRemoveEvent e, HideWhileTabPressedNode uiElement) =>
            uiElement.visibilityPrerequisites.RemoveHidePrerequisite(TAB_PRESSED_PREREQUISITE, true);

        [OnEventFire]
        public void HideUIElementIfTabUnpressed(NodeRemoveEvent e, ShowWhileTabPressedNode showWhileTabPressed) =>
            showWhileTabPressed.visibilityPrerequisites.RemoveShowPrerequisite(TAB_PRESSED_PREREQUISITE, true);

        [OnEventFire]
        public void ShowUIElementIfTabPressedInActiveState(NodeAddedEvent e,
            [Combine] ShowWhileTabPressedActiveStateNode uiElement, ActiveSelfTankNode activeTank) =>
            uiElement.visibilityPrerequisites.AddShowPrerequisite(TAB_PRESSED_WHILE_IN_ACTIVE_STATE, true);

        [OnEventFire]
        public void HideUIElementIfTabUnpressedInActiveState(NodeRemoveEvent e,
            [Combine] ShowWhileTabPressedActiveStateNode showWhileTabPressed, ActiveSelfTankNode activeTank) =>
            showWhileTabPressed.visibilityPrerequisites.RemoveShowPrerequisite(TAB_PRESSED_WHILE_IN_ACTIVE_STATE, true);

        [OnEventFire]
        public void OnStopVisibilityPeriod(StopVisiblePeriodEvent e,
            SingleNode<VisibilityPrerequisitesComponent> uiElement) =>
            uiElement.component.RemoveShowPrerequisite(VISIBLE_PERIOD_PREREQUISITE);

        [OnEventFire]
        public void ScheduleElementConcealment(StartVisiblePeriodEvent e,
            SingleNode<VisibilityPrerequisitesComponent> uiElement) {
            uiElement.component.AddShowPrerequisite(VISIBLE_PERIOD_PREREQUISITE);
            NewEvent<StopVisiblePeriodEvent>().Attach(uiElement).ScheduleDelayed(e.DurationInSec);
        }

        [OnEventFire]
        public void AddNormallyVisiblePrerequisite(NodeAddedEvent e, NormallyVisibleNode uiElement) =>
            uiElement.visibilityPrerequisites.AddShowPrerequisite(NORMALLY_VISIBLE_PREREQUISITE);

        [OnEventFire]
        public void RemoveNormallyVisiblePrerequisite(NodeRemoveEvent e, NormallyVisibleNode uiElement) =>
            uiElement.visibilityPrerequisites.RemoveShowPrerequisite(NORMALLY_VISIBLE_PREREQUISITE);

        public class DeadSelfTankNode : Node {
            public SelfTankComponent selfTank;
            public TankDeadStateComponent tankDeadState;
        }

        public class ActiveSelfTankNode : Node {
            public SelfTankComponent selfTank;
            public TankActiveStateComponent tankActiveState;
        }

        public class SemiActiveSelfTankNode : Node {
            public SelfTankComponent selfTank;
            public TankSemiActiveStateComponent tankSemiActiveState;
        }

        public class SpawnSelfTankNode : Node {
            public SelfTankComponent selfTank;
            public TankSpawnStateComponent tankSpawnState;
        }

        public class ShowWhileTabPressedNode : Node {
            public ShowWhileTabPressedComponent showWhileTabPressed;
            public TabPressedComponent tabPressed;

            public VisibilityPrerequisitesComponent visibilityPrerequisites;
        }

        public class HideWhileTabPressedNode : Node {
            public HideWhileTabPressedComponent hideWhileTabPressed;
            public TabPressedComponent tabPressed;

            public VisibilityPrerequisitesComponent visibilityPrerequisites;
        }

        public class ShowWhileTabPressedActiveStateNode : Node {
            public ShowWhileTabPressedActiveStateComponent showWhileTabPressedActiveState;
            public TabPressedComponent tabPressed;

            public VisibilityPrerequisitesComponent visibilityPrerequisites;
        }

        public class ShowWhileTankInactiveNode : Node {
            public ShowWhileTankInactiveComponent showWhileTankInactive;
            public VisibilityPrerequisitesComponent visibilityPrerequisites;
        }

        public class NormallyVisibleNode : Node {
            public NormallyVisibleComponent normallyVisible;
            public VisibilityPrerequisitesComponent visibilityPrerequisites;
        }
    }
}