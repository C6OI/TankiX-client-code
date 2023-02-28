using System.Collections.Generic;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Platform.Library.ClientResources.API {
    public class AssetStorageComponent : Component {
        public AssetStorageComponent() {
            ManagedReferencies = new Dictionary<string, ResourceStorageEntry>();
            StaticReferencies = new Dictionary<string, Object>();
        }

        public Dictionary<string, ResourceStorageEntry> ManagedReferencies { get; set; }

        public Dictionary<string, Object> StaticReferencies { get; set; }

        public bool Contains(string guid) {
            if (StaticReferencies.ContainsKey(guid)) {
                return true;
            }

            ResourceStorageEntry value;

            if (ManagedReferencies.TryGetValue(guid, out value)) {
                value.LastAccessTime = Time.time;
                return true;
            }

            return false;
        }

        public Object Get(string guid) {
            Object value;

            if (StaticReferencies.TryGetValue(guid, out value)) {
                return value;
            }

            ResourceStorageEntry value2;

            if (ManagedReferencies.TryGetValue(guid, out value2)) {
                value2.LastAccessTime = Time.time;
                return value2.Asset;
            }

            throw new ResourceNotInStorageException(guid);
        }

        public void Remove(string guid, AssetStoreLevel level) {
            switch (level) {
                case AssetStoreLevel.STATIC:
                    ManagedReferencies.Remove(guid);
                    StaticReferencies.Remove(guid);
                    break;

                case AssetStoreLevel.MANAGED:
                    ManagedReferencies.Remove(guid);
                    break;
            }
        }

        public void Put(string guid, Object asset, AssetStoreLevel level) {
            switch (level) {
                case AssetStoreLevel.STATIC:
                    if (!StaticReferencies.ContainsKey(guid)) {
                        StaticReferencies.Add(guid, asset);
                    }

                    break;

                case AssetStoreLevel.MANAGED:
                    if (!ManagedReferencies.ContainsKey(guid)) {
                        ManagedReferencies.Add(guid, new ResourceStorageEntry {
                            Asset = asset,
                            LastAccessTime = Time.time
                        });
                    }

                    break;
            }
        }
    }
}