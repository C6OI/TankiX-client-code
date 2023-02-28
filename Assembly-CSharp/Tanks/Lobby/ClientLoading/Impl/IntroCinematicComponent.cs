using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.Video;

namespace Tanks.Lobby.ClientLoading.Impl {
    public class IntroCinematicComponent : BehaviourComponent {
        Animator animator;

        bool hintVisible;
        VideoPlayer player;

        GameObject videoPrefab;

        void OnEnable() {
            player = GetComponentInChildren<VideoPlayer>(true);
            player.Prepare();
        }

        void OnGUI() {
            if (!(player == null) && player.isPlaying && (Event.current.type == EventType.KeyDown || Event.current.type == EventType.MouseDown)) {
                if (hintVisible && Event.current.keyCode == KeyCode.Space) {
                    animator.SetTrigger("HideVideo");
                    return;
                }

                animator.SetTrigger("ShowHint");
                hintVisible = true;
            }
        }

        void OnVideoLoaded(Object obj) {
            player.clip = (VideoClip)obj;
        }

        public void OnIntroHide() {
            Destroy(gameObject);
        }

        public void Play() {
            animator = GetComponent<Animator>();
            animator.SetTrigger("ShowVideo");
            player.SetTargetAudioSource(0, player.GetComponent<AudioSource>());
            player.loopPointReached += OnFinishPlay;

            if (player.isPrepared) {
                player.Play();
                return;
            }

            player.prepareCompleted += delegate(VideoPlayer source) {
                source.Play();
            };
        }

        void OnFinishPlay(VideoPlayer _) {
            animator.SetTrigger("HideVideo");
        }
    }
}