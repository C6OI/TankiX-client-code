using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    public struct SelfTargetHitEffectHUDData {
        public Vector2 BoundsPosition { get; }

        public Vector3 UpwardsNrm { get; }

        public float Length { get; }

        public Vector2 BoundsPosCanvas { get; }

        public Vector3 EnemyWeaponWorldSpace { get; }

        public Vector2 CnvSize { get; }

        public SelfTargetHitEffectHUDData(Vector3 enemyWeaponWorldSpace, Vector2 boundsPosition, Vector2 boundsPosCanvas, Vector3 upwardsNRM, Vector2 cnvSize, float length) {
            EnemyWeaponWorldSpace = enemyWeaponWorldSpace;
            BoundsPosition = boundsPosition;
            BoundsPosCanvas = boundsPosCanvas;
            CnvSize = cnvSize;
            UpwardsNrm = upwardsNRM;
            Length = length;
        }
    }
}