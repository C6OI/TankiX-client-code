using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class HangarTankCommonPrefabComponent : BehaviourComponent {
        [SerializeField] GameObject tankCommonPrefab;

        public GameObject TankCommonPrefab => tankCommonPrefab;
    }
}