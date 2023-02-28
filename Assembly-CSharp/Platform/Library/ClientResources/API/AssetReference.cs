using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Platform.Library.ClientResources.API {
    [Serializable]
    public class AssetReference {
        public static readonly string GUID_FIELD_SERIALIZED_NAME = "assetGuid";

        [SerializeField] string assetGuid;

        [SerializeField] Object embededReference;

        Object hardReference;

        public Action<Object> OnLoaded;

        public AssetReference() { }

        public AssetReference(string assetGuid)
            : this() => this.assetGuid = assetGuid;

        [Inject] public static EngineService EngineService { get; set; }

        public bool IsDefined => !string.IsNullOrEmpty(assetGuid);

        public string AssetGuid {
            get => assetGuid;
            set {
                if (assetGuid != value) {
                    assetGuid = value;
                    hardReference = null;
                    embededReference = null;
                }
            }
        }

        public Object Reference => !(embededReference != null) ? hardReference : embededReference;

        public void SetReference(Object reference) {
            hardReference = reference;

            if (OnLoaded != null) {
                OnLoaded(reference);
            }
        }

        public void Load() {
            Load(0);
        }

        public void Load(int priority) {
            Entity entity = EngineService.Engine.CreateEntity("Load " + assetGuid);
            entity.AddComponent(new AssetReferenceComponent(this));
            entity.AddComponent(new AssetRequestComponent(priority));
        }

        public override bool Equals(object obj) => assetGuid == ((AssetReference)obj).assetGuid;

        public override int GetHashCode() => assetGuid.GetHashCode();

        public override string ToString() => "AssetReference [assetGuid=" + assetGuid + "]";
    }
}