using UnityEngine;

namespace Lobby.ClientControls.API {
    [ExecuteInEditMode]
    [RequireComponent(typeof(Canvas))]
    public class BaseElementCanvasScaler : MonoBehaviour, BaseElementScaleControllerProvider {
        static bool initialized;

        [SerializeField] BaseElementScaleController baseElementScaleController;

        bool resizing;

        protected void Awake() {
            if (baseElementScaleController != null && !initialized) {
                initialized = true;
                baseElementScaleController.Init();
            }
        }

        protected void OnEnable() { }

        protected void OnRectTransformDimensionsChange() {
            if (!resizing && (enabled || !Application.isPlaying) && baseElementScaleController != null) {
                resizing = true;
                baseElementScaleController.Handle(GetComponent<Canvas>());
                resizing = false;
            }
        }

        public BaseElementScaleController BaseElementScaleController {
            get => baseElementScaleController;
            set => baseElementScaleController = value;
        }

        public static void MarkNeedInitialize() => initialized = false;
    }
}