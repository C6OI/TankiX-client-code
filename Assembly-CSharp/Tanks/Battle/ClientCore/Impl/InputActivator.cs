using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class InputActivator : UnityAwareActivator<AutoCompleting> {
        public GameObject[] inputBinding;

        [Inject] public static InputManager InputManager { get; set; }

        protected override void Activate() {
            string key = "DefaultInputsLoaded";

            if (!PlayerPrefs.HasKey(key)) {
                LoadDefaultInputActions();
                PlayerPrefs.SetInt(key, 1);
            } else {
                LoadInputActions();
            }

            InputManager.ActivateContext(BasicContexts.BATTLE_CONTEXT);
            gameObject.AddComponent<InputBehaviour>();
        }

        public void LoadDefaultInputActions() {
            GameObject[] array = inputBinding;

            foreach (GameObject original in array) {
                GameObject gameObject = Instantiate(original, transform);

                gameObject.GetComponents<InputAction>().ForEach(delegate(InputAction a) {
                    InputManager.RegisterDefaultInputAction(a);
                });
            }
        }

        void LoadInputActions() {
            GameObject[] array = inputBinding;

            foreach (GameObject original in array) {
                GameObject gameObject = Instantiate(original, transform);

                gameObject.GetComponents<InputAction>().ForEach(delegate(InputAction a) {
                    InputManager.RegisterInputAction(a);
                });
            }
        }
    }
}