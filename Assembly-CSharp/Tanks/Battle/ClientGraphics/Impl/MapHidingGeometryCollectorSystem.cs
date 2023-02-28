using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Tool.BakedTrees.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientSettings.API;
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
                    hidingRenderers = gameObject.GetComponentsInChildren<Renderer>(true).Where(IsBillboardRendererNotShadow).ToArray();
                } else {
                    hidingRenderers = new Renderer[0];
                }

                Entity entity = CreateEntity("Foliage hider");
                entity.AddComponent(new MapHidingGeometryComponent(hidingRenderers));
            }
        }

        [OnEventFire]
        public void InitializeShadowsSettingsOnBillboardTrees(NodeAddedEvent e, SingleNode<MapInstanceComponent> map, SingleNode<CameraComponent> cameraNode) {
            BillboardTreeMarkerBehaviour[] componentsInChildren = map.component.SceneRoot.GetComponentsInChildren<BillboardTreeMarkerBehaviour>(true);
            BillboardTreeMarkerBehaviour[] array = componentsInChildren;

            foreach (BillboardTreeMarkerBehaviour billboardTreeMarkerBehaviour in array) {
                billboardTreeMarkerBehaviour.billboardRenderer.receiveShadows = GraphicsSettings.INSTANCE.CurrentTreesShadowRecieving;
                billboardTreeMarkerBehaviour.billboardTreeShadowMarker.gameObject.SetActive(GraphicsSettings.INSTANCE.CurrentBillboardTreesShadowCasting);
            }
        }

        [OnEventFire]
        public void Clean(NodeRemoveEvent evt, SingleNode<MapInstanceComponent> map, [Combine] [JoinAll] SingleNode<MapHidingGeometryComponent> hider) {
            DeleteEntity(hider.Entity);
        }

        [OnEventFire]
        public void SetFarFoliageVisible(NodeAddedEvent e, SingleNode<MapInstanceComponent> map, SingleNode<CameraComponent> cameraNode) {
            FarFoliageRootBehaviour farFoliageRootBehaviour = Object.FindObjectOfType<FarFoliageRootBehaviour>();

            if (farFoliageRootBehaviour != null) {
                farFoliageRootBehaviour.gameObject.SetActive(GraphicsSettings.INSTANCE.CurrentFarFoliageEnabled);
            }
        }

        bool IsBillboardRendererNotShadow(Renderer renderer) => renderer.gameObject.GetComponent<BillboardTreeShadowMarkerBehaviour>() == null;
    }
}