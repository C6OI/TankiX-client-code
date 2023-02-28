using System.Collections.Generic;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class WeaponFeedbackSoundBehaviour : MonoBehaviour {
        static readonly HashSet<AudioSource> WEAPON_FEEDBACK_SOUND_FOR_REMOVING_ON_KILL = new();

        [SerializeField] AudioSource source;

        bool alive;

        void OnDestroy() {
            if (alive) {
                WEAPON_FEEDBACK_SOUND_FOR_REMOVING_ON_KILL.Remove(source);
            }
        }

        void OnApplicationQuit() {
            alive = false;
        }

        public void Play(float delay, float volume, bool removeOnKillSound) {
            source.volume = volume;

            if (delay <= 0f) {
                source.Play();
                DestroyObject(source.gameObject, source.clip.length);
            } else {
                source.PlayDelayed(delay);
                DestroyObject(source.gameObject, source.clip.length + delay);
            }

            if (removeOnKillSound) {
                WEAPON_FEEDBACK_SOUND_FOR_REMOVING_ON_KILL.Add(source);
            }

            alive = true;
            enabled = true;
        }

        public static void ClearHitFeedbackSounds() {
            HashSet<AudioSource>.Enumerator enumerator = WEAPON_FEEDBACK_SOUND_FOR_REMOVING_ON_KILL.GetEnumerator();

            while (enumerator.MoveNext()) {
                if ((bool)enumerator.Current) {
                    enumerator.Current.Stop();
                }
            }

            WEAPON_FEEDBACK_SOUND_FOR_REMOVING_ON_KILL.Clear();
        }

        public void Stop() {
            source.Stop();
        }
    }
}