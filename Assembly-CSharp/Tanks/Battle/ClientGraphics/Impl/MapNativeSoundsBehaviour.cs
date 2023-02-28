using Platform.Library.ClientDataStructures.API;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class MapNativeSoundsBehaviour : MonoBehaviour {
        [SerializeField] FadeSoundFilter[] sounds;

        void Update() {
            if (transform.childCount <= 0) {
                DestroyObject(gameObject);
            }
        }

        public void Play() {
            sounds.ForEach(delegate(FadeSoundFilter s) {
                s.Play();
            });
        }

        public void Stop() {
            enabled = true;

            sounds.ForEach(delegate(FadeSoundFilter s) {
                s.Stop();
            });
        }
    }
}