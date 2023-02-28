using System.Collections.Generic;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class BrokenEffectComponent : BehaviourComponent {
        [SerializeField] GameObject brokenEffect;

        [SerializeField] string trackObjectNamePrefix = "track";

        [SerializeField] string trackMaterialNamePrefix = "Track";

        public GameObject effectInstance;

        public float partDetachProbability = 0.7f;

        public float LifeTime = 6f;

        float effectLifeTime;

        float effectStartTime;

        readonly float fadeTime = 2f;

        bool inited;

        float lastAlpha = -1f;

        string[] materialNames;

        List<Material> materials;

        Dictionary<string, Material> nameToMaterial;

        Rigidbody parentRigidbody;

        List<Vector3> partPositions;

        Rigidbody[] partRigidbodies;

        List<Quaternion> partRotations;

        bool[] rendererIsTrack;

        Renderer[] renderers;

        Rigidbody rigidbody;

        void Awake() {
            enabled = false;
        }

        public void Update() {
            if (!FadeAlpha()) {
                Disable();
            }
        }

        void OnDestroy() {
            if ((bool)effectInstance) {
                Destroy(effectInstance);
            }
        }

        public void StartEffect(GameObject root, Rigidbody parentRigidbody, Renderer parentRenderer, Shader overloadShader, float maxDepenetrationVelocity) {
            if (!inited || !effectInstance) {
                Init();
            }

            this.parentRigidbody = parentRigidbody;
            effectInstance.transform.SetParent(root.transform);
            effectInstance.transform.position = rigidbody.transform.position;
            effectInstance.transform.rotation = rigidbody.transform.rotation;
            UpdateMaterialsFromParentMaterials(parentRenderer, overloadShader);
            RecoverTransforms();
            SetVelocityFromParent(maxDepenetrationVelocity);
            effectStartTime = Time.timeSinceLevelLoad;
            effectLifeTime = LifeTime;
            Enable();
        }

        public void Init() {
            rigidbody = GetComponent<Rigidbody>();
            effectInstance = Instantiate(brokenEffect);
            PhysicsUtil.SetGameObjectLayer(effectInstance, Layers.MINOR_VISUAL);
            partRigidbodies = effectInstance.GetComponentsInChildren<Rigidbody>();
            renderers = effectInstance.GetComponentsInChildren<Renderer>();
            nameToMaterial = new Dictionary<string, Material>();
            SaveTransforms();
            Disable();
            inited = true;
        }

        void UpdateMaterialsFromParentMaterials(Renderer parentRenderer, Shader overloadShader) {
            materials = new List<Material>();
            Material[] sharedMaterials = parentRenderer.sharedMaterials;
            CacheMaterials(sharedMaterials);

            for (int i = 0; i < renderers.Length; i++) {
                Renderer renderer = renderers[i];
                Material source = sharedMaterials[0];

                if (nameToMaterial.ContainsKey(materialNames[i])) {
                    source = nameToMaterial[materialNames[i]];
                }

                Material material = new(source);

                if ((bool)overloadShader) {
                    material.shader = overloadShader;
                }

                renderer.material = material;

                if (!materials.Contains(material)) {
                    materials.Add(material);
                }
            }
        }

        void CacheMaterials(Material[] materials) {
            if (nameToMaterial.Count > 0) {
                return;
            }

            foreach (Material material in materials) {
                string key = material.name.Replace("(Instance)", string.Empty).Replace(" ", string.Empty);

                if (!nameToMaterial.ContainsKey(key)) {
                    nameToMaterial.Add(key, material);
                }
            }

            materialNames = new string[renderers.Length];

            for (int j = 0; j < materialNames.Length; j++) {
                Material sharedMaterial = renderers[j].sharedMaterial;
                materialNames[j] = sharedMaterial.name.Replace("(Instance)", string.Empty).Replace(" ", string.Empty);
            }
        }

        void SaveTransforms() {
            partPositions = new List<Vector3>(partRigidbodies.Length);
            partRotations = new List<Quaternion>(partRigidbodies.Length);
            Rigidbody[] array = partRigidbodies;

            foreach (Rigidbody rigidbody in array) {
                partPositions.Add(rigidbody.transform.localPosition);
                partRotations.Add(rigidbody.transform.localRotation);
            }
        }

        void RecoverTransforms() {
            for (int i = 0; i < partRigidbodies.Length; i++) {
                Rigidbody rigidbody = partRigidbodies[i];
                rigidbody.transform.localPosition = partPositions[i];
                rigidbody.transform.localRotation = partRotations[i];
            }
        }

        void SetVelocityFromParent(float maxDepenetrationVelocity) {
            Rigidbody[] array = partRigidbodies;

            foreach (Rigidbody rigidbody in array) {
                rigidbody.isKinematic = false;
                rigidbody.velocity = parentRigidbody.velocity;
                rigidbody.angularVelocity = parentRigidbody.angularVelocity;
                rigidbody.gameObject.SetActive(true);

                if (Random.value < partDetachProbability) {
                    rigidbody.maxDepenetrationVelocity = maxDepenetrationVelocity;
                } else {
                    rigidbody.maxDepenetrationVelocity = 1f;
                }
            }
        }

        bool FadeAlpha() {
            float num = 1f - Mathf.Clamp01((Time.timeSinceLevelLoad - (effectStartTime + effectLifeTime - fadeTime)) / fadeTime);

            if (num != lastAlpha) {
                lastAlpha = num;

                foreach (Material material in materials) {
                    TankMaterialsUtil.SetAlpha(material, num);
                }
            }

            return Time.timeSinceLevelLoad < effectStartTime + effectLifeTime;
        }

        void Enable() {
            if ((bool)effectInstance) {
                effectInstance.gameObject.SetActive(true);
            }

            enabled = true;
        }

        void Disable() {
            if ((bool)effectInstance) {
                effectInstance.SetActive(false);
            }

            enabled = false;
        }
    }
}