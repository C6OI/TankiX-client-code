using Tanks.Battle.ClientGraphics.Impl;
using UnityEngine;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class CardsSoundBehaviour : MonoBehaviour {
        [SerializeField] SoundController openCardsContainerSource;

        public SoundController OpenCardsContainerSource => openCardsContainerSource;
    }
}