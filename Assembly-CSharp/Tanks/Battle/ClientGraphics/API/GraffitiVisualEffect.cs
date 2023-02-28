using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.API {
    public class GraffitiVisualEffect : MonoBehaviour {
        public ImageSkin Image;

        [SerializeField] GameObject _rareEffect;

        [SerializeField] GameObject _epicEffect;

        [SerializeField] GameObject _legendaryEffect;

        public ItemRarityType Rarity {
            set {
                switch (value) {
                    case ItemRarityType.RARE:
                        _rareEffect.SetActive(true);
                        break;

                    case ItemRarityType.EPIC:
                        _epicEffect.SetActive(transform);
                        break;

                    case ItemRarityType.LEGENDARY:
                        _legendaryEffect.SetActive(true);
                        break;
                }
            }
        }

        void OnDisable() {
            _rareEffect.SetActive(false);
            _epicEffect.SetActive(false);
            _legendaryEffect.SetActive(false);
        }
    }
}