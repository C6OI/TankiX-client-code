using System;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    [Serializable]
    public class InputActionContainer {
        [SerializeField] public InputActionContextId contextId;

        [SerializeField] public InputActionId actionId;
    }
}