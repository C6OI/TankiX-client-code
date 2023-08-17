using Assets.lobby.modules.ClientControls.Scripts.API;
using Lobby.ClientControls.API;
using Lobby.ClientEntrance.API;
using Lobby.ClientSettings.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientProfile.API;

namespace Tanks.Lobby.ClientProfile.Impl {
    public class GameSettingsScreenSystem : ECSSystem {
        [OnEventFire]
        public void FillScreenWithCurrentSettings(NodeAddedEvent e, InvertMovementControlsCheckboxNode checkboxNode,
            [JoinAll] SingleNode<GameTankSettingsComponent> settings) =>
            checkboxNode.checkbox.IsChecked = settings.component.MovementControlsInverted;

        [OnEventFire]
        public void FillScreenWithCurrentSettings(NodeAddedEvent e, MouseControlAllowedCheckboxNode checkboxNode,
            [JoinAll] SingleNode<GameMouseSettingsComponent> settings) =>
            checkboxNode.checkbox.IsChecked = settings.component.MouseControlAllowed;

        [OnEventFire]
        public void FillScreenWithCurrentSettings(NodeAddedEvent e, MouseControlAllowedCheckboxNode checkboxNode,
            MouseVerticalInvertedCheckboxNode MouseVerticalInvertedCheckbox,
            [JoinAll] SingleNode<GameMouseSettingsComponent> settings) =>
            MouseVerticalInvertedCheckbox.dependentInteractivity.SetInteractable(settings.component.MouseControlAllowed);

        [OnEventFire]
        public void FillScreenWithCurrentSettings(NodeAddedEvent e, MouseVerticalInvertedCheckboxNode checkboxNode,
            [JoinAll] SingleNode<GameMouseSettingsComponent> settings) =>
            checkboxNode.checkbox.IsChecked = settings.component.MouseVerticalInverted;

        [OnEventFire]
        public void InitMouseSensivitySliderBar(NodeAddedEvent e, MouseSensivitySliderBarNode mouseSensivitySliderBar,
            [JoinAll] SingleNode<GameMouseSettingsComponent> settings) {
            mouseSensivitySliderBar.sliderBar.Value = settings.component.MouseSensivity;
            ScheduleEvent(new SettingsChangedEvent<GameMouseSettingsComponent>(settings.component), settings);
        }

        [OnEventFire]
        public void ChangeMovementControlSettings(CheckboxEvent e, InvertMovementControlsCheckboxNode checkboxNode,
            [JoinAll] SingleNode<GameTankSettingsComponent> settings) {
            settings.component.MovementControlsInverted = checkboxNode.checkbox.IsChecked;
            ScheduleEvent(new SettingsChangedEvent<GameTankSettingsComponent>(settings.component), settings);
        }

        [OnEventFire]
        public void ChangeMovementControlSettings(CheckboxEvent e, MouseControlAllowedCheckboxNode checkboxNode,
            [JoinAll] SingleNode<GameMouseSettingsComponent> settings,
            [JoinAll] MouseVerticalInvertedCheckboxNode MouseVerticalInvertedCheckbox) {
            settings.component.MouseControlAllowed = checkboxNode.checkbox.IsChecked;
            MouseVerticalInvertedCheckbox.dependentInteractivity.SetInteractable(settings.component.MouseControlAllowed);
            ScheduleEvent(new SettingsChangedEvent<GameMouseSettingsComponent>(settings.component), settings);
        }

        [OnEventFire]
        public void ChangeMovementControlSettings(CheckboxEvent e, MouseVerticalInvertedCheckboxNode checkboxNode,
            [JoinAll] SingleNode<GameMouseSettingsComponent> settings) {
            settings.component.MouseVerticalInverted = checkboxNode.checkbox.IsChecked;
            ScheduleEvent(new SettingsChangedEvent<GameMouseSettingsComponent>(settings.component), settings);
        }

        [OnEventFire]
        public void OnMinMouseSensivitySliderBar(SliderBarSetToMinValueEvent e,
            MouseSensivitySliderBarNode mouseSensivitySliderBar, [JoinAll] SingleNode<GameMouseSettingsComponent> settings) {
            settings.component.MouseSensivity = mouseSensivitySliderBar.sliderBar.Value;
            ScheduleEvent(new SettingsChangedEvent<GameMouseSettingsComponent>(settings.component), settings);
        }

        [OnEventFire]
        public void OnChangedMouseSensivitySliderBar(SliderBarValueChangedEvent e,
            MouseSensivitySliderBarNode mouseSensivitySliderBar, [JoinAll] SingleNode<GameMouseSettingsComponent> settings) {
            settings.component.MouseSensivity = mouseSensivitySliderBar.sliderBar.Value;
            ScheduleEvent(new SettingsChangedEvent<GameMouseSettingsComponent>(settings.component), settings);
        }

        public class InvertMovementControlsCheckboxNode : Node {
            public CheckboxComponent checkbox;

            public InvertMovementControlsCheckboxComponent invertMovementControlsCheckbox;
        }

        public class MouseControlAllowedCheckboxNode : Node {
            public CheckboxComponent checkbox;

            public MouseControlAllowedCheckboxComponent mouseControlAllowedCheckbox;
        }

        public class MouseVerticalInvertedCheckboxNode : Node {
            public CheckboxComponent checkbox;

            public DependentInteractivityComponent dependentInteractivity;

            public MouseVerticalInvertedCheckboxComponent mouseVerticalInvertedCheckbox;
        }

        public class MouseSensivitySliderBarNode : Node {
            public MouseSensivitySliderBarComponent mouseSensivitySliderBar;

            public SliderBarComponent sliderBar;
        }
    }
}