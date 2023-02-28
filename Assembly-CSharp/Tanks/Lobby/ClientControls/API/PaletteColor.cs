using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientControls.API {
    [ExecuteInEditMode]
    public class PaletteColor : MonoBehaviour {
        [SerializeField] Palette palette;

        [SerializeField] int uid;

        [SerializeField] bool applyToChildren;

        readonly List<Graphic> graphicCache = new();

        void Start() {
            Apply(palette);

            if (Application.isPlaying) {
                Destroy(this);
            }
        }

        void Apply(Palette palette) {
            if (applyToChildren) {
                GetComponentsInChildren(graphicCache);

                {
                    foreach (Graphic item in graphicCache) {
                        ApplyToGraphic(item, palette);
                    }

                    return;
                }
            }

            Graphic component = GetComponent<Graphic>();
            ApplyToGraphic(component, palette);
        }

        void ApplyToGraphic(Graphic graphic, Palette palette) {
            graphic.color = palette.Apply(uid, graphic.color);
        }
    }
}