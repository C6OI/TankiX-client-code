using Tanks.Lobby.ClientGarage.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ModuleCardOutlineView : MonoBehaviour {
        [SerializeField] Color[] tierColor;

        [SerializeField] OutlineObject outline;

        [SerializeField] ModuleCardView card;

        public void Start() {
            outline.GlowColor = tierColor[card.tierNumber];
        }
    }
}