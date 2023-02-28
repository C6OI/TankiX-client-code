using System.Collections.Generic;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class WindowsSpaceFillComponent : BehaviourComponent {
        [SerializeField] List<Animator> animators = new();

        public List<Animator> Animators => animators;
    }
}