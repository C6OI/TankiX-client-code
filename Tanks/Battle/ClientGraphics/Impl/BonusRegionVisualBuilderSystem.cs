using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class BonusRegionVisualBuilderSystem : ECSSystem {
        const string TERRAIN_TAG = "Terrain";

        [OnEventFire]
        public void SetAsset(NodeAddedEvent e, [Combine] SingleNode<BonusRegionComponent> region,
            SingleNode<BonusRegionAssetsComponent> regionAssets) {
            BonusRegionAssetsComponent component = regionAssets.component;
            string assetGuid;

            switch (region.component.Type) {
                case BonusType.ARMOR:
                    assetGuid = component.DoubleArmorAssetGuid;
                    break;

                case BonusType.DAMAGE:
                    assetGuid = component.DoubleDamageAssetGuid;
                    break;

                case BonusType.REPAIR:
                    assetGuid = component.RepairKitAssetGuid;
                    break;

                case BonusType.SPEED:
                    assetGuid = component.SpeedBoostAssetGuid;
                    break;

                case BonusType.GOLD:
                    assetGuid = component.GoldAssetGuid;
                    break;

                default:
                    throw new UnknownRegionTypeException(region.component.Type);
            }

            region.Entity.AddComponent(new BonusRegionAssetComponent(assetGuid));

            if (region.component.Type != BonusType.GOLD) {
                region.Entity.AddComponent<VisibleBonusRegionComponent>();
            }
        }

        [OnEventFire]
        public void RequestRegionPrefabs(NodeAddedEvent e, SingleNode<BonusRegionAssetComponent> region) {
            region.Entity.AddComponent(new AssetReferenceComponent(new AssetReference(region.component.AssetGuid)));
            region.Entity.AddComponent<AssetRequestComponent>();
        }

        [OnEventFire]
        public void PlaceRegions(NodeAddedEvent e, [Combine] BonusRegionBuildNode region,
            SingleNode<MapInstanceComponent> map) {
            if (!(region.resourceData.Data == null)) {
                GameObject gameObject = (GameObject)Object.Instantiate(region.resourceData.Data,
                    region.spatialGeometry.Position,
                    Quaternion.Euler(region.spatialGeometry.Rotation));

                Animation component = gameObject.GetComponent<Animation>();

                if (component != null) {
                    component[component.clip.name].normalizedTime = Random.value;
                }

                region.Entity.AddComponent(new BonusRegionInstanceComponent(gameObject));
            }
        }

        [OnEventFire]
        public void Destroy(NodeRemoveEvent e, InstantiatedBonusRegionNode bonusRegion) =>
            Object.Destroy(bonusRegion.bonusRegionInstance.BonusRegionInstance);

        public class BonusRegionBuildNode : Node {
            public BonusRegionComponent bonusRegion;

            public ResourceDataComponent resourceData;

            public SpatialGeometryComponent spatialGeometry;
        }

        public class InstantiatedBonusRegionNode : Node {
            public BonusRegionComponent bonusRegion;

            public BonusRegionInstanceComponent bonusRegionInstance;
        }
    }
}