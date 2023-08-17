using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class HangarAmbientSoundController : MonoBehaviour {
        [SerializeField] AmbientSoundFilter background;

        [SerializeField] HangarHymnSoundBehaviour hymn;

        void Update() {
            if (background == null && hymn == null) {
                DestroyObject(gameObject);
            }
        }

        public void Play(bool playWithNitro) {
            background.Play();
            hymn.Play(playWithNitro);
        }

        public void Stop() {
            background.Stop();
            hymn.Stop();
        }
    }
}