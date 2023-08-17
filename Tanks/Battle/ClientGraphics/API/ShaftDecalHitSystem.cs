using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientGraphics.API {
    public class ShaftDecalHitSystem : AbstractDecalHitSystem {
        [OnEventFire]
        public void DrawHitDecal(SelfHitEvent evt, SingleNode<ShaftQuickShotDecalProjectorComponent> decalProjectorNode,
            [JoinByTank] SingleNode<MuzzlePointComponent> muzzlePointNode,
            [JoinAll] SingleNode<DecalManagerComponent> decalManagerNode) => DrawHitDecal(evt,
            decalManagerNode.component,
            decalProjectorNode.component,
            muzzlePointNode.component);

        [OnEventFire]
        public void DrawHitDecal(RemoteHitEvent evt, SingleNode<ShaftQuickShotDecalProjectorComponent> decalProjectorNode,
            [JoinByTank] SingleNode<MuzzlePointComponent> muzzlePointNode,
            [JoinAll] SingleNode<DecalManagerComponent> decalManagerNode) => DrawHitDecal(evt,
            decalManagerNode.component,
            decalProjectorNode.component,
            muzzlePointNode.component);

        [OnEventFire]
        public void DrawHitDecalSelf(SelfShaftAimingHitEvent evt,
            SingleNode<ShaftAimingShotDecalProjectorComponent> decalProjectorNode,
            [JoinByTank] SingleNode<MuzzlePointComponent> muzzlePointNode,
            [JoinAll] SingleNode<DecalManagerComponent> decalManagerNode) => DrawHitDecal(evt,
            decalManagerNode.component,
            decalProjectorNode.component,
            muzzlePointNode.component);

        [OnEventFire]
        public void DrawHitDecalRemote(RemoteShaftAimingHitEvent evt,
            SingleNode<ShaftAimingShotDecalProjectorComponent> decalProjectorNode,
            [JoinByTank] SingleNode<MuzzlePointComponent> muzzlePointNode,
            [JoinAll] SingleNode<DecalManagerComponent> decalManagerNode) => DrawHitDecal(evt,
            decalManagerNode.component,
            decalProjectorNode.component,
            muzzlePointNode.component);
    }
}