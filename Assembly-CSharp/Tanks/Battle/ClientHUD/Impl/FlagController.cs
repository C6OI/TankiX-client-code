using System;
using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    [RequireComponent(typeof(Animator))]
    public class FlagController : MonoBehaviour {
        string lastState;
        Action onReset;

        void Reset() {
            if (onReset != null) {
                onReset();
            }

            onReset = null;
        }

        void OnEnable() {
            if (lastState != null) {
                GetComponent<Animator>().SetTrigger(lastState);
            }
        }

        void OnDisable() {
            lastState = null;
        }

        public void TurnIn(Action onReset) {
            this.onReset = (Action)Delegate.Combine(this.onReset, onReset);
            SetState("TurnIn");
        }

        public void Drop() {
            SetState("Drop");
        }

        public void Return(Action onReset) {
            this.onReset = (Action)Delegate.Combine(this.onReset, onReset);
            SetState("Return");
        }

        public void PickUp() {
            SetState("PickUp");
        }

        void SetState(string state) {
            lastState = state;

            if (gameObject.activeInHierarchy) {
                GetComponent<Animator>().SetTrigger(state);
            }
        }
    }
}