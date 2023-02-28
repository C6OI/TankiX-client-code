using Tanks.Battle.ClientGraphics.Impl;
using UnityEngine;

namespace Tanks.Tool.TankViewer.API {
    public class ColoringEditorUIView : MonoBehaviour {
        public CreatorView creatorView;

        public ViewerView viewerView;

        public void SwitchToEditor(ColoringComponent coloringComponent) {
            creatorView.colorView.SetColor(coloringComponent.color);
            creatorView.textureView.SetAlphaMode(coloringComponent.coloringTextureAlphaMode);
            creatorView.textureView.textureDropdown.value = 0;
            creatorView.normalMapView.SetNormalScale(coloringComponent.coloringNormalScale);
            creatorView.normalMapView.normalMapDropdown.value = 0;
            creatorView.intensityThresholdView.SetUseIntensityThreshold(coloringComponent.useColoringIntensityThreshold);
            creatorView.intensityThresholdView.SetIntensityThreshold(coloringComponent.coloringMaskThreshold);
            creatorView.smoothnessView.SetOverrideSmoothness(coloringComponent.overwriteSmoothness);
            creatorView.smoothnessView.SetSmoothnessStrenght(coloringComponent.smoothnessStrength);
            creatorView.metallicView.SetFloat(coloringComponent.metallic);
            creatorView.gameObject.SetActive(true);
            viewerView.gameObject.SetActive(false);
        }

        public void SwitchToViewer() {
            creatorView.gameObject.SetActive(false);
            viewerView.gameObject.SetActive(true);
        }
    }
}