using System.Collections.Generic;
using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Lobby.ClientControls.API {
    public class RegisterScreensEvent : Event {
        public IEnumerable<GameObject> Screens { get; set; }
    }
}