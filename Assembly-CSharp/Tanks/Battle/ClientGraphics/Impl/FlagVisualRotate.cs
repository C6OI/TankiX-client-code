using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class FlagVisualRotate : MonoBehaviour {
        public GameObject flag;

        public Transform tank;

        public float timerUpsideDown;

        public float scale;

        public float origin;

        public float distanceForRotateFlag;

        public Component sprite;

        public Sprite3D spriteComponent;

        Transform child;

        float curentAngle;

        Vector3 deltaPos;

        Vector3 direction;

        Vector3 newPos;

        Vector3 oldPos;

        float targetAngle;

        float timeSinceUpsideDown;

        void Start() {
            child = flag.transform.GetChild(0);
            spriteComponent = flag.transform.GetComponent<Sprite3D>();
        }

        void Update() {
            if (flag.transform.parent == null) {
                return;
            }

            newPos = tank.position;
            deltaPos = newPos - oldPos;
            direction = tank.InverseTransformDirection(deltaPos);

            if (direction.z > distanceForRotateFlag) {
                targetAngle = 0f;
            }

            if (direction.z < 0f - distanceForRotateFlag) {
                targetAngle = -180f;
            }

            curentAngle = Mathf.LerpAngle(curentAngle, targetAngle - flag.transform.parent.localEulerAngles.y, Time.deltaTime);
            child.transform.localEulerAngles = new Vector3(0f, curentAngle, 0f);
            oldPos = tank.position;

            if (flag.transform.up.y <= 0f) {
                timeSinceUpsideDown += Time.deltaTime;

                if (timeSinceUpsideDown >= timerUpsideDown) {
                    spriteComponent.scale = scale;
                    spriteComponent.originY = origin;
                }
            } else {
                timeSinceUpsideDown = 0f;
                spriteComponent.scale = 0f;
                spriteComponent.originY = origin;
            }
        }
    }
}