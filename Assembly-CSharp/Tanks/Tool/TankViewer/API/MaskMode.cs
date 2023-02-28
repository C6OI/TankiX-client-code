using UnityEngine;

namespace Tanks.Tool.TankViewer.API {
    public class MaskMode : CameraMode {
        readonly Camera camera;

        readonly GameObject plane;

        public MaskMode(Camera camera, GameObject plane) {
            this.camera = camera;
            this.plane = plane;
        }

        public void SwitchOn() {
            camera.gameObject.SetActive(true);
            plane.SetActive(false);
        }

        public void SwithOff() {
            camera.gameObject.SetActive(false);
            plane.SetActive(true);
        }
    }
}