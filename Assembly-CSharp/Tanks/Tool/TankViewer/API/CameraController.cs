using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Tool.TankViewer.API {
    public class CameraController : MonoBehaviour {
        public CameraModeController cameraModeController;

        public TargetCameraController targetCameraController;

        public FreeCameraController freeCameraController;

        public void Awake() {
            targetCameraController.enabled = true;
            freeCameraController.enabled = false;
        }

        public void ChangeMode() {
            cameraModeController.ChangeMode();
        }

        public void ChangeController() {
            if (targetCameraController.enabled) {
                targetCameraController.enabled = false;
                freeCameraController.enabled = true;
            } else {
                targetCameraController.enabled = true;
                freeCameraController.enabled = false;
            }
        }
    }
}