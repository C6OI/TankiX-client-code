using System.Collections.Generic;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class FullRegistrationDependenciesComponents : MonoBehaviour, Component {
        [SerializeField] List<GameObject> dependecies;

        public List<GameObject> Dependecies => dependecies;
    }
}