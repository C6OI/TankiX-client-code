using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Tool.BakedTrees.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class MapHidingGeometryCollectorSystem : ECSSystem {
        [OnEventFire]
        public void CollectHidingGeometry(NodeAddedEvent evt, SingleNode<MapInstanceComponent> map) {
            HidingGeomentryRootBehaviour[] array = Object.FindObjectsOfType<HidingGeomentryRootBehaviour>();
            HidingGeomentryRootBehaviour[] array2 = array;

            foreach (HidingGeomentryRootBehaviour hidingGeomentryRootBehaviour in array2) {
                Renderer[] hidingRenderers;

                if (hidingGeomentryRootBehaviour != null) {
                    GameObject gameObject = hidingGeomentryRootBehaviour.gameObject;

                    hidingRenderers = gameObject.GetComponentsInChildren<Renderer>(true).Where(IsBillboardRendererNotShadow)
                        .ToArray();
                } else {
                    hidingRenderers = new Renderer[0];
                }

                Entity entity = CreateEntity("Foliage hider");
                entity.AddComponent(new MapHidingGeometryComponent(hidingRenderers));
            }
        }

        [OnEventFire]
        public void InitializeShadowsSettingsOnBillboardTrees(NodeAddedEvent evt, SingleNode<MapInstanceComponent> map,
            SingleNode<VegetationSettingsComponent> settings) {
            BillboardTreeMarkerBehaviour[] componentsInChildren =
                map.component.SceneRoot.GetComponentsInChildren<BillboardTreeMarkerBehaviour>(true);

            VegetationSettingsComponent component = settings.component;
            BillboardTreeMarkerBehaviour[] array = componentsInChildren;

            foreach (BillboardTreeMarkerBehaviour billboardTreeMarkerBehaviour in array) {
                billboardTreeMarkerBehaviour.billboardRenderer.receiveShadows = component.BillboardTreesShadowReceiving;

                billboardTreeMarkerBehaviour.billboardTreeShadowMarker.gameObject.SetActive(component
                    .BillboardTreesShadowCasting);
            }
        }

        [OnEventFire]
        public void Clean(NodeRemoveEvent evt, SingleNode<MapInstanceComponent> map,
            [JoinAll] [Combine] SingleNode<MapHidingGeometryComponent> hider) => DeleteEntity(hider.Entity);

        [OnEventFire]
        public void SetFarFoliageVisible(NodeAddedEvent e, SingleNode<MapInstanceComponent> map,
            [JoinAll] SingleNode<VegetationSettingsComponent> vegetationSettings) {
            FarFoliageRootBehaviour farFoliageRootBehaviour = Object.FindObjectOfType<FarFoliageRootBehaviour>();

            if (farFoliageRootBehaviour != null) {
                farFoliageRootBehaviour.gameObject.SetActive(vegetationSettings.component.FarFoliageEnabled);
            }
        }

        bool IsBillboardRendererNotShadow(Renderer renderer) =>
            renderer.gameObject.GetComponent<BillboardTreeShadowMarkerBehaviour>() == null;
    }
}