using System;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    [Serializable]
    public class InputActionContextId {
        [InputType] [SerializeField] public string contextTypeName;

        [InputName] [SerializeField] public string contextName = BasicContexts.BATTLE_CONTEXT;

        public InputActionContextId(string contextTypeName) => this.contextTypeName = contextTypeName;

        public override bool Equals(object obj) {
            InputActionContextId inputActionContextId = (InputActionContextId)obj;
            return inputActionContextId.contextName.Equals(contextName) && inputActionContextId.contextTypeName.Equals(contextTypeName);
        }

        public override string ToString() => "contextTypeName: " + contextTypeName + ", contextName: " + contextName;
    }
}