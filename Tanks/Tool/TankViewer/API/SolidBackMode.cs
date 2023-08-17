using UnityEngine;

namespace Tanks.Tool.TankViewer.API {
    public class SolidBackMode : CameraMode {
        readonly Color backColor;
        readonly Camera solidBackgroundCamera;

        public SolidBackMode(Camera solidBackgroundCamera, Color backColor) {
            this.solidBackgroundCamera = solidBackgroundCamera;
            this.backColor = backColor;
        }

        public void SwitchOn() {
            solidBackgroundCamera.backgroundColor = backColor;
            solidBackgroundCamera.gameObject.SetActive(true);
        }

        public void SwithOff() => solidBackgroundCamera.gameObject.SetActive(false);
    }
}