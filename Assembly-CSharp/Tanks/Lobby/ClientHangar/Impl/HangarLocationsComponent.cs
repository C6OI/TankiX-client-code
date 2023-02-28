using System.Collections.Generic;
using Tanks.Lobby.ClientHangar.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class HangarLocationsComponent : Component {
        public Dictionary<HangarLocation, Transform> Locations { get; set; }
    }
}