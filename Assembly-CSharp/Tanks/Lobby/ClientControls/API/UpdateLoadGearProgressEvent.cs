using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Tanks.Lobby.ClientControls.API {
    public class UpdateLoadGearProgressEvent : Event {
        public UpdateLoadGearProgressEvent(float value) => Value = Mathf.Clamp01(value);

        public float Value { get; set; }
    }
}