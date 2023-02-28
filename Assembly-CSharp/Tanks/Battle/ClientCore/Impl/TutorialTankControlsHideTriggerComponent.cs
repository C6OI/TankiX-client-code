using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Battle.ClientCore.Impl {
    public class TutorialTankControlsHideTriggerComponent : TutorialHideTriggerComponent {
        static readonly string RIGHT_AXIS = "MoveRight";

        static readonly string LEFT_AXIS = "MoveLeft";

        static readonly string FORWARD_AXIS = "MoveForward";

        static readonly string BACKWARD_AXIS = "MoveBackward";

        [Inject] public static InputManager InputManager { get; set; }

        protected void Update() {
            if (!triggered && (InputManager.GetUnityAxis(RIGHT_AXIS) != 0f || InputManager.GetUnityAxis(LEFT_AXIS) != 0f || InputManager.GetUnityAxis(FORWARD_AXIS) != 0f ||
                               InputManager.GetUnityAxis(BACKWARD_AXIS) != 0f)) {
                Triggered();
            }
        }
    }
}