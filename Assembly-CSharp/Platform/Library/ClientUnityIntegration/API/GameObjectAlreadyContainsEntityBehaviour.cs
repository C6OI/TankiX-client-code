using System;
using UnityEngine;

namespace Platform.Library.ClientUnityIntegration.API {
    public class GameObjectAlreadyContainsEntityBehaviour : Exception {
        public GameObjectAlreadyContainsEntityBehaviour(GameObject gameObject)
            : base(string.Format("GameObject {0} already contains EntityBehaviour", gameObject)) { }
    }
}