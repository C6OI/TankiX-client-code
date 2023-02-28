using Tanks.Lobby.ClientNavigation.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientNavigation.Impl {
    [RequireComponent(typeof(Animator))]
    public class BackgroundComponent : BackgroundDimensionsChangeComponent, Component, NoScaleScreen {
        const string VISIBLE_ANIMATION_PARAM = "Visible";

        public virtual void Hide() {
            GetComponent<Animator>().SetBool("Visible", false);
        }

        public virtual void Show() {
            GetComponent<Animator>().SetBool("Visible", true);
        }
    }
}