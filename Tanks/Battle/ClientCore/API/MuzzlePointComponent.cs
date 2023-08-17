using Platform.Library.ClientProtocol.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(635824352682945226L)]
    public class MuzzlePointComponent : Component {
        public int CurrentIndex { get; set; }

        public Transform[] Points { get; set; }

        public Transform Current => Points[CurrentIndex];
    }
}