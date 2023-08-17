using System.Collections.Generic;
using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Tanks.Battle.ClientCore.Impl {
    public class ValidateSplashHitPointsEvent : Event {
        public ValidateSplashHitPointsEvent() { }

        public ValidateSplashHitPointsEvent(SplashHitData splashHit, List<GameObject> excludeObjects) {
            SplashHit = splashHit;
            this.excludeObjects = excludeObjects;
        }

        public SplashHitData SplashHit { get; set; }

        public List<GameObject> excludeObjects { get; set; }
    }
}