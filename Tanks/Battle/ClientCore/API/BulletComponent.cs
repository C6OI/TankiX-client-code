using Platform.Library.ClientProtocol.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(635824352452485226L)]
    public class BulletComponent : Component {
        public Vector3 Position { get; set; }

        public Vector3 Direction { get; set; }

        public float Distance { get; set; }

        public float LastUpdateTime { get; set; }

        public float Radius { get; set; }

        public float Speed { get; set; }

        public int ShotId { get; set; }
    }
}