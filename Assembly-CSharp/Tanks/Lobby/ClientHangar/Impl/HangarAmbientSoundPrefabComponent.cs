using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class HangarAmbientSoundPrefabComponent : MonoBehaviour, Component {
        [SerializeField] HangarAmbientSoundController hangarAmbientSoundController;

        public HangarAmbientSoundController HangarAmbientSoundController {
            get => hangarAmbientSoundController;
            set => hangarAmbientSoundController = value;
        }
    }
}