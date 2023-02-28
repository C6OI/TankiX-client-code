using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class BattleResultTankCameraController : MonoBehaviour {
        readonly Vector3 awardScreenPosition = new(-13.4f, 4.17f, 18.58f);

        readonly Vector3 awardScreenRotation = new(9.449f, -219.436f, 0.811f);

        readonly float awFieldOfView = 40f;
        readonly Vector3 bestPlayerScreenPosition = new(-4.041f, 0.481f, 8.276f);

        readonly Vector3 bestPlayerScreenRotation = new(-8.188f, -219.687f, 0.818f);

        readonly float bpFieldOfView = 70f;

        Camera cam;

        RenderTexture renderTex;

        void Awake() {
            cam = GetComponent<Camera>();
            cam.enabled = true;
        }

        public void SetRenderTexture(RenderTexture tex) {
            cam.targetTexture = tex;
        }

        public void SetupForBestPlayer() {
            transform.localPosition = bestPlayerScreenPosition;
            transform.localEulerAngles = bestPlayerScreenRotation;
            cam.fieldOfView = bpFieldOfView;
        }

        public void SetupForAwardScren() {
            transform.localPosition = awardScreenPosition;
            transform.localEulerAngles = awardScreenRotation;
            cam.fieldOfView = awFieldOfView;
        }
    }
}