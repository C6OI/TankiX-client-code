using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Lobby.ClientEntrance.API {
    [RequireComponent(typeof(Selectable))]
    public class DependentInteractivityComponent : MonoBehaviour, Component {
        public List<InteractivityPrerequisiteComponent> prerequisitesObjects;

        public virtual void SetInteractable(bool value) => GetComponent<Selectable>().interactable = value;
    }
}