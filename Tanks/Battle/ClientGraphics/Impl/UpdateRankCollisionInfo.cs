using System;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class UpdateRankCollisionInfo : EventArgs {
        public RaycastHit Hit;
    }
}