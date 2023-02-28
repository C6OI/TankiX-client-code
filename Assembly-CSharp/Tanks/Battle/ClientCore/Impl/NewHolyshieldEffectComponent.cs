using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class NewHolyshieldEffectComponent : BehaviourComponent {
        const float UP_OFFSET = 0.5f;

        const float SIZE_TO_EFFECT_SCALE_RELATION = 5f / 9f;

        [SerializeField] Animator hollyShieldEffect;

        int alphaHash;

        Animator animator;

        Transform cameraTransform;

        readonly int hideHash = Animator.StringToHash("hide");

        readonly int invisHash = Animator.StringToHash("invisbility");

        Material mat;

        Vector3 previousCamPos;

        Transform root;

        readonly int showHash = Animator.StringToHash("show");

        public Animator HollyShieldEffect => hollyShieldEffect;

        public SphereCollider Collider { get; set; }

        void Update() {
            if (animator.IsInTransition(0) && animator.GetNextAnimatorStateInfo(0).shortNameHash == invisHash) {
                animator.gameObject.SetActive(false);
                enabled = false;
            }
        }

        public GameObject InitEffect(Transform root, SkinnedMeshRenderer renderer, int colliderLayer) {
            this.root = root;
            alphaHash = Shader.PropertyToID("_Visibility");
            Vector3 size = renderer.localBounds.size;
            float num = Mathf.Max(size.x, size.y, size.z);
            animator = Instantiate(hollyShieldEffect, root.position, root.rotation, root);
            animator.transform.localPosition = new Vector3(0f, 0.5f, 0f);
            Vector3 one = Vector3.one;
            one.z = one.y = one.x = 5f / 9f * num;
            animator.transform.localScale = one;
            animator.gameObject.SetActive(false);
            enabled = false;
            Collider = animator.GetComponentInChildren<SphereCollider>();
            Collider.gameObject.layer = colliderLayer;
            mat = animator.GetComponent<Renderer>().material;
            return animator.gameObject;
        }

        public void Play() {
            enabled = true;
            animator.gameObject.SetActive(true);
            animator.Play(showHash, 0);
        }

        public void Stop() {
            animator.Play(hideHash, 0);
        }

        public void UpdateAlpha(float alpha) {
            mat.SetFloat(alphaHash, alpha);
        }
    }
}