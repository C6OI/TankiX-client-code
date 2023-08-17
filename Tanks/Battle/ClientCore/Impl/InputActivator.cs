using System.Linq;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class InputActivator : UnityAwareActivator<AutoCompleting> {
        public GameObject[] inputBinding;

        [Inject] public static InputManager InputManager { get; set; }

        protected override void Activate() {
            inputBinding.SelectMany(ib => ib.GetComponents<InputAction>()).ForEach(delegate(InputAction a) {
                InputManager.RegisterInputAction(a);
            });

            InputManager.ActivateContext(BasicContexts.BATTLE_CONTEXT);
            gameObject.AddComponent<InputBehaviour>();
        }
    }
}