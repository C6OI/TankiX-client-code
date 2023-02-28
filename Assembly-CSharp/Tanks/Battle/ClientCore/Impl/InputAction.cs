using System;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class InputAction : MonoBehaviour {
        [SerializeField] public InputActionContextId contextId;

        [SerializeField] public InputActionId actionId;

        public KeyCode[] keys;

        public MultiKeys[] multiKeys;

        public UnityInputAxes[] axes;

        public bool onlyPositiveAxes;

        public bool onlyNegativeAxes;

        public bool invertAxes;

        protected bool activated;

        protected Action startHandler;

        protected Action stopHandler;

        public bool Activated => activated;

        public Action StartHandler {
            get => startHandler;
            set => startHandler = value;
        }

        public Action StopHandler {
            get => stopHandler;
            set => stopHandler = value;
        }

        public void StartInputAction() {
            activated = true;

            if (startHandler != null) {
                startHandler();
            }
        }

        public void StopInputAction() {
            activated = false;

            if (stopHandler != null) {
                stopHandler();
            }
        }

        public static implicit operator bool(InputAction action) => action != null && action.activated;

        public override string ToString() => string.Format("[InputAction: name={0}, keys={1}]", actionId.actionName, keys);
    }
}