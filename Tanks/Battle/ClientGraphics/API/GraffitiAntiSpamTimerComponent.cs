using System.Collections.Generic;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.API {
    public class GraffitiAntiSpamTimerComponent : MonoBehaviour, Component {
        public Dictionary<string, GraffityInfo> GraffitiDelayDictionary = new();

        public float SprayDelay { get; set; }

        public class GraffityInfo {
            public CreateGraffitiEvent CreateGraffitiEvent;

            public float Time;
        }
    }
}