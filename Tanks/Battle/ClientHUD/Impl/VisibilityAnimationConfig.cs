using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    [RequireComponent(typeof(Animator))]
    public class VisibilityAnimationConfig : MonoBehaviour {
        const string VISIBLE_ANIMATION_PROP = "Visible";

        const string INITIALLY_VISIBLE_ANIMATION_PROP = "InitiallyVisible";

        const string NO_ANIMATION_PROP = "NoAnimation";

        [SerializeField] bool initiallyVisible;

        [SerializeField] bool noAnimation;

        public void OnEnable() {
            Animator component = GetComponent<Animator>();
            component.SetBool("NoAnimation", noAnimation);
            component.SetBool("InitiallyVisible", initiallyVisible);
            component.SetBool("Visible", initiallyVisible);
        }
    }
}