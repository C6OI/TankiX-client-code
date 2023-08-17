using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

public class CreateGraffitiEvent : Event {
    public CreateGraffitiEvent(Vector3 position, Vector3 direction, Vector3 up) {
        Position = position;
        Direction = direction;
        Up = up;
    }

    public Vector3 Position { get; set; }

    public Vector3 Direction { get; set; }

    public Vector3 Up { get; set; }

    public bool Success { get; set; }
}