using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.Impl;
using Tanks.Lobby.ClientLoading.API;
using UnityEngine;

namespace Tanks.Lobby.ClientLoading.Impl {
    public class IntroCinematicSystem : ECSSystem {
        [OnEventFire]
        public void Play(NodeAddedEvent e, SingleNode<IntroCinematicComponent> cinematic, SingleNode<LobbyLoadScreenComponent> _) {
            if (PlayerPrefs.GetInt("Intro", 0) == 0) {
                cinematic.Entity.AddComponent<TanyaSleepComponent>();
                PlayerPrefs.SetInt("Intro", 1);
                cinematic.component.Play();
            } else {
                cinematic.component.OnIntroHide();
            }
        }
    }
}