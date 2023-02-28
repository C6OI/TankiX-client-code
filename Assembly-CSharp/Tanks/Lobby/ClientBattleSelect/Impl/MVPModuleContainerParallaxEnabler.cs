using Tanks.Lobby.ClientControls.API;
using UnityEngine;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class MVPModuleContainerParallaxEnabler : HoverHandler {
        [SerializeField] ParallaxContainer parallaxContainer;

        protected override bool pointerIn {
            set {
                base.pointerIn = value;
                parallaxContainer.IsActive = value;
            }
        }
    }
}