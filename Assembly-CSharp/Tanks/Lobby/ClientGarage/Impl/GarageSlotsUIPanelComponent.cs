using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientGarage.API;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class GarageSlotsUIPanelComponent : BehaviourComponent {
        [SerializeField] SlotUIComponent[] slots;

        public SlotUIComponent GetSlot(Slot slot) {
            if ((int)slot >= slots.Length) {
                return null;
            }

            return slots[(uint)slot];
        }
    }
}