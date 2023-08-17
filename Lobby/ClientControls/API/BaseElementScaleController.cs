using System.Collections.Generic;
using Platform.Library.ClientResources.API;
using UnityEngine;

namespace Lobby.ClientControls.API {
    public class BaseElementScaleController : ScriptableObject, SizeController {
        [SerializeField] List<BaseElement> elements = new();

        readonly HashSet<SpriteRequest> requests = new();

        int resolutionIndex = -1;

        public AssetReference LoadingSkin { get; set; }

        public void OnDestroy() {
            foreach (BaseElement element in elements) {
                element.CancelAllRequests();
            }

            resolutionIndex = -1;
        }

        public void RegisterSpriteRequest(SpriteRequest request) {
            if (!requests.Contains(request)) {
                requests.Add(request);
            }

            if (resolutionIndex < 0 && elements.Count > 0) {
                resolutionIndex = 0;
            }

            elements[resolutionIndex].RequestSprite(request);
        }

        public void UnregisterSpriteRequest(SpriteRequest request) {
            requests.Remove(request);

            if (resolutionIndex >= 0) {
                elements[resolutionIndex].CancelRequest(request);
            }
        }

        public void Init() {
            requests.Clear();

            foreach (BaseElement element in elements) {
                element.Init();
            }
        }

        public void Handle(Canvas canvas) {
            if (canvas == null || !canvas.isRootCanvas || elements.Count == 0) {
                return;
            }

            if (canvas.renderMode == RenderMode.WorldSpace) {
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                Debug.LogWarning("BaseElementCanvasScaler is not working in WorldSpace RenderMode");
            }

            int num = int.MaxValue;
            int num2 = (int)canvas.pixelRect.height;
            int num3 = -1;

            for (int i = 0; i < elements.Count; i++) {
                int num4 = num2 - elements[i].CanvasHeight;

                if (num4 > 0 && num4 < num) {
                    num = num4;
                    num3 = i;
                }
            }

            if (num3 >= 0) {
                canvas.scaleFactor = elements[num3].Size / 100f;
                canvas.referencePixelsPerUnit = 100f / canvas.scaleFactor;
            }

            if (num3 != resolutionIndex) {
                ValidateSkin(resolutionIndex, num3);

                if (Application.isPlaying && resolutionIndex != num3) {
                    canvas.BroadcastMessage("OnBaseElementSizeChanged", SendMessageOptions.DontRequireReceiver);
                }

                resolutionIndex = num3;
            }
        }

        void ValidateSkin(int oldResolutionIndex, int newResolutionIndex) {
            if (oldResolutionIndex >= 0) {
                elements[oldResolutionIndex].CancelAllRequests();
            }

            BaseElement baseElement = elements[newResolutionIndex];

            foreach (SpriteRequest request in requests) {
                baseElement.RequestSprite(request);
            }
        }
    }
}