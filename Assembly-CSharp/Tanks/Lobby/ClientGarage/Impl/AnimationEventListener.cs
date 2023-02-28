using System;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class AnimationEventListener : MonoBehaviour {
        Action hideHandler;

        Action partyHandler;

        public void OnHide() {
            if (hideHandler != null) {
                hideHandler();
            }
        }

        public void OnPartyFinish() {
            if (partyHandler != null) {
                partyHandler();
                partyHandler = null;
            }
        }

        public void SetPartyHandler(Action handler) {
            partyHandler = handler;
        }

        public void SetHideHandler(Action handler) {
            hideHandler = handler;
        }
    }
}