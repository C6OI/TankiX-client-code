using System;
using Platform.Library.ClientDataStructures.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class FramesStatisticsCollectorBehaviour : MonoBehaviour, FramesStatisticsCollector {
        StatisticCollection frames;

        Consumer<StatisticCollection> onComplete;

        float startCollectingTime;
        int timeInSec;

        public void Awake() => enabled = false;

        void Update() {
            int value = (int)(Time.deltaTime * 1000f);
            frames.Add(value);

            if (Time.realtimeSinceStartup - startCollectingTime >= timeInSec) {
                Consumer<StatisticCollection> consumer = onComplete;
                StatisticCollection t = frames;
                Stop();
                consumer(t);
            }
        }

        public void Collect(int timeInSec, int maxFrameDurationInMs, Consumer<StatisticCollection> onComplete) {
            if (enabled) {
                throw new Exception("FramesStatisticsCollector is collecting data already! You have to stop it.");
            }

            enabled = true;
            this.timeInSec = timeInSec;
            this.onComplete = onComplete;
            startCollectingTime = Time.realtimeSinceStartup;
            frames = new StatisticCollection(maxFrameDurationInMs);
        }

        public void Stop() {
            onComplete = null;
            frames = null;
            enabled = false;
        }
    }
}