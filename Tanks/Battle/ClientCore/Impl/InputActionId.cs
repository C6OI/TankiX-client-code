using System;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    [Serializable]
    public class InputActionId {
        [SerializeField] [InputType] public string actionTypeName;

        [InputName] [SerializeField] public string actionName;
    }
}