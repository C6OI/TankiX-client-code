using System;
using Tanks.Battle.ClientGraphics.Impl;
using UnityEngine;

namespace Tanks.Tool.TankViewer.API {
    public class ColoringEditorUIController : MonoBehaviour {
        public ColoringEditorUIView view;

        public ColoringCreationLogic logic;

        public TextureDataSource textureDataSource;

        public ResultsDataSource resultsDataSource;

        public void Awake() {
            view.SwitchToViewer();
            TextureDataSource obj = textureDataSource;
            obj.onStartUpdatingAction = (Action)Delegate.Combine(obj.onStartUpdatingAction, new Action(view.creatorView.Disable));
            TextureDataSource obj2 = textureDataSource;
            obj2.onStartUpdatingAction = (Action)Delegate.Combine(obj2.onStartUpdatingAction, new Action(logic.CleanTextures));
            TextureDataSource obj3 = textureDataSource;
            obj3.onCompleteUpdatingAction = (Action)Delegate.Combine(obj3.onCompleteUpdatingAction, new Action(EnableCreatorView));
            textureDataSource.UpdateData();
            view.viewerView.resultsDropdownView.dropdown.interactable = false;

            if (resultsDataSource.IsReady) {
                UpdateResultsDropdown();
            }

            ResultsDataSource obj4 = resultsDataSource;
            obj4.onChange = (Action)Delegate.Combine(obj4.onChange, new Action(UpdateResultsDropdown));
        }

        void UpdateResultsDropdown() {
            view.viewerView.resultsDropdownView.dropdown.interactable = true;
        }

        public void OnCreateColoringButtonClick() {
            ColoringComponent coloringComponent = logic.CreateNewColoring();
            view.SwitchToEditor(coloringComponent);
        }

        public void OnSaveClick() {
            logic.Save();
            view.SwitchToViewer();
            view.viewerView.resultsDropdownView.SelectLastOption();
        }

        public void OnCancelClick() {
            view.SwitchToViewer();
        }

        public void OnSomeParamChange() {
            if (view.creatorView.gameObject.activeSelf) {
                CreatorView creatorView = view.creatorView;

                logic.UpdateColoring(creatorView.colorView.GetColor(), creatorView.textureView.GetSelectedTexture(), creatorView.textureView.GetAlphaMode(),
                    creatorView.normalMapView.GetSelectedNormalMap(), creatorView.normalMapView.GetNormalScale(), creatorView.metallicView.GetFloat(),
                    creatorView.smoothnessView.GetOverrideSmoothness(), creatorView.smoothnessView.GetSmoothnessStrenght(), creatorView.intensityThresholdView.GetUseIntensityThreshold(),
                    creatorView.intensityThresholdView.GetIntensityThreshold());
            }
        }

        public void OnUpdateTexturesButtonClick() {
            textureDataSource.UpdateData();
        }

        public void EnableCreatorView() {
            view.creatorView.Enable();
        }

        public void OnResultsDropdownChanged() {
            int num = view.viewerView.resultsDropdownView.dropdown.value - 1;

            if (num >= 0) {
                ColoringComponent coloringComponent = resultsDataSource.GetColoringComponents()[num];
                logic.UpdateColoring(coloringComponent);
            }
        }
    }
}