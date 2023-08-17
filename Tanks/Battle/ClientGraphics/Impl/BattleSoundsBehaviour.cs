using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class BattleSoundsBehaviour : MonoBehaviour {
        const float DESTROY_DELAY = 1f;

        [SerializeField] AudioSource nonTeamRoundStartSound;

        [SerializeField] AudioSource teamWinRoundStartSound;

        [SerializeField] AudioSource teamLostRoundStartSound;

        public void PlayNonTeamSound(Transform root, float delay = -1f) =>
            InstantiateAndPlay(nonTeamRoundStartSound, root, delay);

        public void PlayTeamSound(Transform root, bool win, float delay = -1f) {
            AudioSource source = !win ? teamLostRoundStartSound : teamWinRoundStartSound;
            InstantiateAndPlay(source, root, delay);
        }

        void InstantiateAndPlay(AudioSource source, Transform root, float delay) {
            AudioSource audioSource = Instantiate(source);
            Transform transform = audioSource.transform;
            transform.parent = root;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;

            if (delay > 0f) {
                audioSource.PlayScheduled(AudioSettings.dspTime + delay);
                DestroyObject(audioSource.gameObject, delay + audioSource.clip.length + 1f);
            } else {
                audioSource.Play();
                DestroyObject(audioSource.gameObject, audioSource.clip.length + 1f);
            }
        }
    }
}