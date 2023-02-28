using System;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    [Serializable]
    public class InputActionId {
        [InputType] [SerializeField] public string actionTypeName;

        [InputName] [SerializeField] public string actionName;

        public InputActionId(string actionTypeName, string actionName) {
            this.actionTypeName = actionTypeName;
            this.actionName = actionName;
        }

        public override string ToString() => "actionTypeName: " + actionTypeName + ", actionName: " + actionName;
    }
}