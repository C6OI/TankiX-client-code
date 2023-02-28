using Tanks.Lobby.ClientGarage.API;
using UnityEngine;
using UnityEngine.EventSystems;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class SlotsPanelComponent : UIBehaviour, Component {
        [SerializeField] GameObject slotPrefab;

        [SerializeField] string[] slotSpriteUids;

        [SerializeField] GameObject[] slots;

        public string GetIconByType(ModuleBehaviourType moduleBehaviourType) => slotSpriteUids[(uint)moduleBehaviourType];

        public GameObject SetSlot(Slot slot) => slots[(uint)slot];
    }
}