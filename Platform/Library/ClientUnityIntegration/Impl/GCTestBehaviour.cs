using System;
using UnityEngine;

namespace Platform.Library.ClientUnityIntegration.Impl {
    public class GCTestBehaviour : MonoBehaviour {
        public float GCPeriod;

        float nextGCTime;

        void Update() {
            if (Time.time > nextGCTime) {
                GC.Collect();
                nextGCTime = Time.time + GCPeriod;
            }
        }
    }
}