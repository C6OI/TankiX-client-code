using System.Collections.Generic;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class BaseDialogComponent : BehaviourComponent {
        public virtual void Show(List<Animator> animators = null) { }

        public virtual void Hide() { }
    }
}