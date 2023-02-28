using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class BattleSoundsBehaviour : MonoBehaviour {
        const float DESTROY_DELAY = 1f;

        [SerializeField] float minRemainigRoundTimeSec = 5f;

        [SerializeField] int minDMScoreDiff = 5;

        [SerializeField] int minTDMScoreDiff = 7;

        [SerializeField] int minCTFScoreDiff = 2;

        [SerializeField] AudioSource[] startSounds;

        [SerializeField] AudioSource shortNeutralSound;

        [SerializeField] AudioSource shortWinSound;

        [SerializeField] AudioSource shortLostSound;

        [SerializeField] AmbientSoundFilter victoryMelody;

        [SerializeField] AmbientSoundFilter defeatMelody;

        [SerializeField] AmbientSoundFilter neutralMelody;

        public float MinRemainigRoundTimeSec => minRemainigRoundTimeSec;

        public int MinDmScoreDiff => minDMScoreDiff;

        public int MinTdmScoreDiff => minTDMScoreDiff;

        public int MinCtfScoreDiff => minCTFScoreDiff;

        public void PlayStartSound(Transform root, float delay = -1f) {
            InstantiateAndPlay(startSounds[Random.Range(0, startSounds.Length)], root, delay);
        }

        public void PlayShortNeutralSound(Transform root, float delay = -1f) {
            InstantiateAndPlay(shortNeutralSound, root, delay);
        }

        public void PlayShortNonNeutralSound(Transform root, bool win, float delay = -1f) {
            AudioSource source = !win ? shortLostSound : shortWinSound;
            InstantiateAndPlay(source, root, delay);
        }

        public AmbientSoundFilter PlayNeutralMelody(Transform root, float delay = -1f) => InstantiateAndPlay(neutralMelody, root, delay);

        public AmbientSoundFilter PlayNonNeutralMelody(Transform root, bool win, float delay = -1f) => InstantiateAndPlay(!win ? defeatMelody : victoryMelody, root, delay);

        AmbientSoundFilter InstantiateAndPlay(AmbientSoundFilter source, Transform root, float delay) {
            AmbientSoundFilter ambientSoundFilter = Instantiate(source);
            Transform instanceTransform = ambientSoundFilter.transform;
            ApplyParentTransformData(instanceTransform, root);

            if (delay > 0f) {
                ambientSoundFilter.Play(delay);
                return ambientSoundFilter;
            }

            ambientSoundFilter.Play();
            return ambientSoundFilter;
        }

        void ApplyParentTransformData(Transform instanceTransform, Transform root) {
            instanceTransform.parent = root;
            instanceTransform.localPosition = Vector3.zero;
            instanceTransform.localRotation = Quaternion.identity;
            instanceTransform.localScale = Vector3.one;
        }

        void InstantiateAndPlay(AudioSource source, Transform root, float delay) {
            AudioSource audioSource = Instantiate(source);
            Transform instanceTransform = audioSource.transform;
            ApplyParentTransformData(instanceTransform, root);

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