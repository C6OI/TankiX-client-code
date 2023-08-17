using UnityEngine;
using UnityEngine.UI;

namespace Lobby.ClientControls.API {
    [RequireComponent(typeof(Graphic))]
    [ExecuteInEditMode]
    public class PaletteColor : MonoBehaviour {
        [SerializeField] Palette palette;

        [SerializeField] int uid;

        void Start() {
            GetComponent<Graphic>().color = palette.Get(uid);

            if (Application.isPlaying) {
                Destroy(this);
            }
        }
    }
}