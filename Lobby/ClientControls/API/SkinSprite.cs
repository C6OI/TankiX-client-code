using System;
using UnityEngine;

namespace Lobby.ClientControls.API {
    [Serializable]
    public class SkinSprite {
        [SerializeField] string uid;

        [SerializeField] Sprite sprite;

        public string Uid => uid;

        public Sprite Sprite {
            get => sprite;
            set => sprite = value;
        }
    }
}