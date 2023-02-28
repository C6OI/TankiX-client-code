using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class LoginRewardScrollTip : MonoBehaviour {
        [SerializeField] ScrollRect scrollRect;

        Animator _animator;

        void Start() {
            _animator = GetComponent<Animator>();
        }

        void Update() {
            _animator.SetBool("show", scrollRect.horizontalNormalizedPosition < 0.7f);
        }
    }
}