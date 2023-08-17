using System;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    [Serializable]
    public class InputActionContextId {
        [SerializeField] [InputType] public string contextTypeName;

        [SerializeField] [InputName] public string contextName = BasicContexts.BATTLE_CONTEXT;
    }
}