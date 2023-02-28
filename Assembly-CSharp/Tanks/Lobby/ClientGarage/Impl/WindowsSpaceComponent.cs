using System.Collections.Generic;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class WindowsSpaceComponent : BehaviourComponent {
        [SerializeField] List<Animator> animators;

        public List<Animator> Animators => animators;
    }
}