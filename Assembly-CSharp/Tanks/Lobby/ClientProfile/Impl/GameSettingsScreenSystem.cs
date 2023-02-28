using Assets.lobby.modules.ClientControls.Scripts.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientProfile.API;
using Tanks.Lobby.ClientSettings.API;

namespace Tanks.Lobby.ClientProfile.Impl {
    public class GameSettingsScreenSystem : ECSSystem {
        [OnEventFire]
        public void FillScreenWithCurrentSettings(NodeAddedEvent e, TargetFocusEnabledCheckboxNode checkboxNode, [JoinAll] SingleNode<TargetFocusSettingsComponent> settings) {
            checkboxNode.checkbox.IsChecked = settings.component.Enabled;
        }

        [OnEventFire]
        public void FillScreenWithCurrentSettings(NodeAddedEvent e, LaserSightEnabledCheckboxNode checkboxNode, [JoinAll] SingleNode<LaserSightSettingsComponent> settings) {
            checkboxNode.checkbox.IsChecked = settings.component.Enabled;
        }

        [OnEventFire]
        public void FillScreenWithCurrentSettings(NodeAddedEvent e, CameraShakerEnabledCheckboxNode checkboxNode, [JoinAll] SingleNode<GameCameraShakerSettingsComponent> settings) {
            checkboxNode.checkbox.IsChecked = settings.component.Enabled;
        }

        [OnEventFire]
        public void FillScreenWithCurrentSettings(NodeAddedEvent e, HealthFeedbackEnabledCheckboxNode checkboxNode, [JoinAll] SingleNode<GameTankSettingsComponent> settings,
            [JoinAll] SingleNode<FeedbackGraphicsRestrictionsComponent> quality) {
            checkboxNode.dependentInteractivity.SetInteractable(quality.component.HealthFeedbackAllowed);
            checkboxNode.checkbox.IsChecked = settings.component.HealthFeedbackEnabled;
        }

        [OnEventFire]
        public void FillScreenWithCurrentSettings(NodeAddedEvent e, SelfTargetHitFeedbackEnabledCheckboxNode checkboxNode, [JoinAll] SingleNode<GameTankSettingsComponent> settings,
            [JoinAll] SingleNode<FeedbackGraphicsRestrictionsComponent> quality) {
            checkboxNode.dependentInteractivity.SetInteractable(quality.component.SelfTargetHitFeedbackAllowed);
            checkboxNode.checkbox.IsChecked = settings.component.SelfTargetHitFeedbackEnabled;
        }

        [OnEventFire]
        public void FillScreenWithCurrentSettings(NodeAddedEvent e, DamageInfoSettingsNode checkboxNode, [JoinAll] SingleNode<GameTankSettingsComponent> settings) {
            checkboxNode.checkbox.IsChecked = settings.component.DamageInfoEnabled;
        }

        [OnEventFire]
        public void FillScreenWithCurrentSettings(NodeAddedEvent e, InvertMovementControlsCheckboxNode checkboxNode, [JoinAll] SingleNode<GameTankSettingsComponent> settings) {
            checkboxNode.checkbox.IsChecked = settings.component.MovementControlsInverted;
        }

        [OnEventFire]
        public void FillScreenWithCurrentSettings(NodeAddedEvent e, MouseControlAllowedCheckboxNode checkboxNode, [JoinAll] SingleNode<GameMouseSettingsComponent> settings) {
            checkboxNode.checkbox.IsChecked = settings.component.MouseControlAllowed;
        }

        [OnEventFire]
        public void FillScreenWithCurrentSettings(NodeAddedEvent e, MouseControlAllowedCheckboxNode checkboxNode, MouseVerticalInvertedCheckboxNode MouseVerticalInvertedCheckbox,
            [JoinAll] SingleNode<GameMouseSettingsComponent> settings) {
            MouseVerticalInvertedCheckbox.dependentInteractivity.SetInteractable(settings.component.MouseControlAllowed);
        }

        [OnEventFire]
        public void FillScreenWithCurrentSettings(NodeAddedEvent e, MouseVerticalInvertedCheckboxNode checkboxNode, [JoinAll] SingleNode<GameMouseSettingsComponent> settings) {
            checkboxNode.checkbox.IsChecked = settings.component.MouseVerticalInverted;
        }

        [OnEventFire]
        public void InitMouseSensivitySliderBar(NodeAddedEvent e, MouseSensivitySliderBarNode mouseSensivitySliderBar, [JoinAll] SingleNode<GameMouseSettingsComponent> settings) {
            mouseSensivitySliderBar.sliderBar.Value = settings.component.MouseSensivity;
            ScheduleEvent(new SettingsChangedEvent<GameMouseSettingsComponent>(settings.component), settings);
        }

        [OnEventFire]
        public void ChangeMovementControlSettings(CheckboxEvent e, InvertMovementControlsCheckboxNode checkboxNode, [JoinAll] SingleNode<GameTankSettingsComponent> settings) {
            settings.component.MovementControlsInverted = checkboxNode.checkbox.IsChecked;
            ScheduleEvent(new SettingsChangedEvent<GameTankSettingsComponent>(settings.component), settings);
        }

        [OnEventFire]
        public void ChangeMouseControlAllowedSettings(CheckboxEvent e, MouseControlAllowedCheckboxNode checkboxNode, [JoinAll] SingleNode<GameMouseSettingsComponent> settings,
            [JoinAll] MouseVerticalInvertedCheckboxNode MouseVerticalInvertedCheckbox) {
            settings.component.MouseControlAllowed = checkboxNode.checkbox.IsChecked;
            MouseVerticalInvertedCheckbox.dependentInteractivity.SetInteractable(settings.component.MouseControlAllowed);
            ScheduleEvent(new SettingsChangedEvent<GameMouseSettingsComponent>(settings.component), settings);
        }

        [OnEventFire]
        public void ChangeDamageInfoEnabledSettings(CheckboxEvent e, DamageInfoSettingsNode checkboxNode, [JoinAll] SingleNode<GameTankSettingsComponent> settings) {
            settings.component.DamageInfoEnabled = checkboxNode.checkbox.IsChecked;
            ScheduleEvent(new SettingsChangedEvent<GameTankSettingsComponent>(settings.component), settings);
        }

        [OnEventFire]
        public void ChangeMouseVerticalInvertedSettings(CheckboxEvent e, MouseVerticalInvertedCheckboxNode checkboxNode, [JoinAll] SingleNode<GameMouseSettingsComponent> settings) {
            settings.component.MouseVerticalInverted = checkboxNode.checkbox.IsChecked;
            ScheduleEvent(new SettingsChangedEvent<GameMouseSettingsComponent>(settings.component), settings);
        }

        [OnEventFire]
        public void ChangeTargetFocusSettings(CheckboxEvent e, TargetFocusEnabledCheckboxNode checkboxNode, [JoinAll] SingleNode<TargetFocusSettingsComponent> settings) {
            settings.component.Enabled = checkboxNode.checkbox.IsChecked;
            ScheduleEvent(new SettingsChangedEvent<TargetFocusSettingsComponent>(settings.component), settings);
        }

        [OnEventFire]
        public void ChangeTargetFocusSettings(CheckboxEvent e, LaserSightEnabledCheckboxNode checkboxNode, [JoinAll] SingleNode<LaserSightSettingsComponent> settings) {
            settings.component.Enabled = checkboxNode.checkbox.IsChecked;
            ScheduleEvent(new SettingsChangedEvent<LaserSightSettingsComponent>(settings.component), settings);
        }

        [OnEventFire]
        public void ChangeCameraShakerSettings(CheckboxEvent e, CameraShakerEnabledCheckboxNode checkboxNode, [JoinAll] SingleNode<GameCameraShakerSettingsComponent> settings) {
            settings.component.Enabled = checkboxNode.checkbox.IsChecked;
            ScheduleEvent(new SettingsChangedEvent<GameCameraShakerSettingsComponent>(settings.component), settings);
        }

        [OnEventFire]
        public void ChangeHealthFeedbackSettings(CheckboxEvent e, HealthFeedbackEnabledCheckboxNode checkboxNode, [JoinAll] SingleNode<GameTankSettingsComponent> settings) {
            settings.component.HealthFeedbackEnabled = checkboxNode.checkbox.IsChecked;
            ScheduleEvent(new SettingsChangedEvent<GameTankSettingsComponent>(settings.component), settings);
        }

        [OnEventFire]
        public void SelfTargetHitFeedbackSettings(CheckboxEvent e, SelfTargetHitFeedbackEnabledCheckboxNode checkboxNode, [JoinAll] SingleNode<GameTankSettingsComponent> settings) {
            settings.component.SelfTargetHitFeedbackEnabled = checkboxNode.checkbox.IsChecked;
            ScheduleEvent(new SettingsChangedEvent<GameTankSettingsComponent>(settings.component), settings);
        }

        [OnEventFire]
        public void OnMinMouseSensivitySliderBar(SliderBarSetToMinValueEvent e, MouseSensivitySliderBarNode mouseSensivitySliderBar,
            [JoinAll] SingleNode<GameMouseSettingsComponent> settings) {
            settings.component.MouseSensivity = mouseSensivitySliderBar.sliderBar.Value;
            ScheduleEvent(new SettingsChangedEvent<GameMouseSettingsComponent>(settings.component), settings);
        }

        [OnEventFire]
        public void OnChangedMouseSensivitySliderBar(SliderBarValueChangedEvent e, MouseSensivitySliderBarNode mouseSensivitySliderBar,
            [JoinAll] SingleNode<GameMouseSettingsComponent> settings) {
            settings.component.MouseSensivity = mouseSensivitySliderBar.sliderBar.Value;
            ScheduleEvent(new SettingsChangedEvent<GameMouseSettingsComponent>(settings.component), settings);
        }

        [OnEventComplete]
        public void SetDefaultInvertSettings(SetDefaultControlSettingsEvent e, Node any, [JoinAll] InvertMovementControlsCheckboxNode movementControlCheckboxNode,
            [JoinAll] MouseControlAllowedCheckboxNode mouseControlAllowedCheckboxNode, [JoinAll] MouseVerticalInvertedCheckboxNode mouseVerticalInvertedCheckboxNode,
            [JoinAll] MouseSensivitySliderBarNode mouseSensivitySliderBar, [JoinAll] SingleNode<GameTankSettingsComponent> moveSettings,
            [JoinAll] SingleNode<GameMouseSettingsComponent> mouseSettings) {
            movementControlCheckboxNode.checkbox.IsChecked = moveSettings.component.MovementControlsInverted;
            mouseControlAllowedCheckboxNode.checkbox.IsChecked = mouseSettings.component.MouseControlAllowed;
            mouseVerticalInvertedCheckboxNode.checkbox.IsChecked = mouseSettings.component.MouseVerticalInverted;
            mouseSensivitySliderBar.sliderBar.Value = mouseSettings.component.MouseSensivity;
        }

        public class TargetFocusEnabledCheckboxNode : Node {
            public CheckboxComponent checkbox;

            public TargetFocusCheckboxComponent targetFocusCheckbox;
        }

        public class LaserSightEnabledCheckboxNode : Node {
            public CheckboxComponent checkbox;

            public LaserSightCheckboxComponent laserSightCheckbox;
        }

        public class InvertMovementControlsCheckboxNode : Node {
            public CheckboxComponent checkbox;

            public InvertMovementControlsCheckboxComponent invertMovementControlsCheckbox;
        }

        public class MouseControlAllowedCheckboxNode : Node {
            public CheckboxComponent checkbox;

            public MouseControlAllowedCheckboxComponent mouseControlAllowedCheckbox;
        }

        public class CameraShakerEnabledCheckboxNode : Node {
            public CameraShakerCheckboxComponent cameraShakerCheckbox;
            public CheckboxComponent checkbox;
        }

        public class HealthFeedbackEnabledCheckboxNode : Node {
            public CheckboxComponent checkbox;

            public DependentInteractivityComponent dependentInteractivity;

            public HealthFeedbackCheckboxComponent healthFeedbackCheckbox;
        }

        public class SelfTargetHitFeedbackEnabledCheckboxNode : Node {
            public CheckboxComponent checkbox;

            public DependentInteractivityComponent dependentInteractivity;

            public SelfTargetHitFeedbackCheckboxComponent selfTargetHitFeedbackCheckbox;
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

        public class DamageInfoSettingsNode : Node {
            public CheckboxComponent checkbox;

            public DamageInfoEnabledCheckboxComponent damageInfoEnabledCheckbox;
        }
    }
}