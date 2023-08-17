using System;
using UnityEngine;
using UnityEngine.Events;

namespace Lobby.ClientControls.API {
    public class Blinker : MonoBehaviour {
        const float FORWARD = 1f;

        const float BACKWARD = -1f;

        public float maxValue = 1f;

        public float minValue;

        public float speedCoeff = 1f;

        public float initialBlinkTimeInterval = 1f;

        public float minBlinkTimeInterval = 1f;

        public float blinkTimeIntervalDecrement;

        public float timeOffset;

        public OnBlinkEvent onBlink;

        float currentBlinkTimeInterval;

        float speed;

        float time;

        float timeBeforeBlink;

        float value;

        float valueDelta;

        void Reset() {
            timeBeforeBlink = timeOffset;
            currentBlinkTimeInterval = initialBlinkTimeInterval;
            valueDelta = maxValue - minValue;
            value = maxValue;
            speed = GetSpeed(-1f);
        }

        void Update() {
            timeBeforeBlink -= Time.deltaTime;

            if (timeBeforeBlink > 0f) {
                return;
            }

            time += Time.deltaTime;
            value += speed * Time.deltaTime;

            if (value > maxValue) {
                value = maxValue;
            }

            if (value < minValue) {
                value = minValue;
            }

            if (time >= currentBlinkTimeInterval) {
                if (currentBlinkTimeInterval > minBlinkTimeInterval) {
                    currentBlinkTimeInterval -= blinkTimeIntervalDecrement;

                    if (currentBlinkTimeInterval < minBlinkTimeInterval) {
                        currentBlinkTimeInterval = minBlinkTimeInterval;
                    }
                }

                time = 0f;
                speed = GetSpeed(!(speed < 0f) ? -1f : 1f);
            }

            onBlink.Invoke(value);
        }

        void OnEnable() => Reset();

        void OnDisable() => StopBlink();

        public void StartBlink() {
            Reset();
            onBlink.Invoke(maxValue);
            enabled = true;
        }

        public void StopBlink() {
            enabled = false;
            onBlink.Invoke(maxValue);
        }

        float GetSpeed(float direction) => direction * speedCoeff * valueDelta / currentBlinkTimeInterval;

        [Serializable]
        public class OnBlinkEvent : UnityEvent<float> { }
    }
}