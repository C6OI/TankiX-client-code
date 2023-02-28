using System.Collections.Generic;
using tanks.modules.tool.TankViewer.Scripts.API.ColoringEditor.ParamView;
using Tanks.Tool.TankViewer.API.Params;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Tool.TankViewer.API {
    public class CreatorView : MonoBehaviour {
        public ColorView colorView;

        public TextureView textureView;

        public NormalMapView normalMapView;

        public MetallicView metallicView;

        public SmoothnessView smoothnessView;

        public IntensityThresholdView intensityThresholdView;

        public Button saveButton;

        public Button cancelButton;

        public Button updateTexturesButton;

        public void Disable() {
            foreach (Selectable uIElement in GetUIElements()) {
                uIElement.interactable = false;
            }

            normalMapView.Disable();
            textureView.Disable();
            smoothnessView.Disable();
            intensityThresholdView.Disable();
        }

        public void Enable() {
            foreach (Selectable uIElement in GetUIElements()) {
                uIElement.interactable = true;
            }

            normalMapView.Enable();
            textureView.Enable();
            smoothnessView.Enable();
            intensityThresholdView.Enable();
        }

        List<Selectable> GetUIElements() {
            List<Selectable> list = new();
            list.Add(colorView.colorInput);
            list.Add(metallicView.slider);
            list.Add(saveButton);
            list.Add(cancelButton);
            list.Add(updateTexturesButton);
            return list;
        }
    }
}