using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class ForceFieldEffect : MonoBehaviour {
        public float alpha = 1f;

        public MeshCollider outerMeshCollider;

        public MeshCollider innerMeshCollider;

        public MeshRenderer meshRenderer;

        public DomeWaveGenerator waveGenerator;

        public Material enemyMaterial;

        [SerializeField] AudioSource hitSourceAsset;

        [SerializeField] AudioClip[] hitClips;

        Animator _animator;

        Camera _cachedCamera;

        public Camera CachedCamera {
            get {
                if (!_cachedCamera) {
                    _cachedCamera = Camera.main;
                }

                return _cachedCamera;
            }
        }

        void Awake() {
            _animator = GetComponent<Animator>();
            waveGenerator.Init();
        }

        public void LateUpdate() {
            meshRenderer.material.SetFloat("_MainAlpha", alpha);
        }

        public void SetLayer(int layer) {
            gameObject.layer = layer;
            outerMeshCollider.gameObject.layer = layer;
            innerMeshCollider.gameObject.layer = layer;
        }

        public void SwitchToEnemyView() {
            meshRenderer.material = enemyMaterial;
            waveGenerator.Init();
        }

        public void Show() {
            meshRenderer.material.SetFloat("_MainAlpha", 0f);

            if (CachedCamera != null) {
                meshRenderer.material.shader.maximumLOD = CachedCamera.depthTextureMode != 0 ? 300 : 150;
            }

            _animator.SetTrigger("show");
            outerMeshCollider.enabled = true;
            innerMeshCollider.enabled = true;
        }

        public void Hide() {
            _animator.SetTrigger("hide");
            outerMeshCollider.enabled = false;
            innerMeshCollider.enabled = false;
        }

        public void DrawWave(Vector3 hitPoint, bool playSound) {
            waveGenerator.GenerateWave(hitPoint);

            if (playSound) {
                InstantiateSound(hitPoint);
            }
        }

        void InstantiateSound(Vector3 point) {
            AudioSource audioSource = Instantiate(hitSourceAsset);
            int num = Random.Range(0, hitClips.Length);
            AudioClip audioClip2 = audioSource.clip = hitClips[num];
            audioSource.transform.position = point;
            audioSource.transform.rotation = Quaternion.identity;
            audioSource.Play();
            DestroyObject(audioSource.gameObject, audioClip2.length + 0.3f);
        }

        void OnEffectHidden() {
            DestroyObject(gameObject);
        }
    }
}