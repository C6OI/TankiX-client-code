using System;
using System.Collections.Generic;
using System.IO;
using Tanks.Battle.ClientGraphics.Impl;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Tool.TankViewer.API {
    public class TextureView : MonoBehaviour {
        public Dropdown textureDropdown;

        public Dropdown alphaModeDropdown;

        public TextureDataSource dataSource;

        bool inited;

        public void Awake() {
            TextureDataSource textureDataSource = dataSource;

            textureDataSource.onStartUpdatingAction = (Action)Delegate.Combine(textureDataSource.onStartUpdatingAction, (Action)delegate {
                inited = false;
                textureDropdown.ClearOptions();
                Disable();
            });

            TextureDataSource textureDataSource2 = dataSource;
            textureDataSource2.onCompleteUpdatingAction = (Action)Delegate.Combine(textureDataSource2.onCompleteUpdatingAction, new Action(UpdateView));

            if (dataSource.TexturesAreReady) {
                UpdateView();
            }
        }

        public void UpdateView() {
            textureDropdown.ClearOptions();
            List<TextureData> data = dataSource.GetData();
            textureDropdown.options.Add(new Dropdown.OptionData("none"));

            for (int i = 0; i < data.Count; i++) {
                TextureData textureData = data[i];
                Texture2D texture2D = textureData.texture2D;
                Sprite image = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
                textureDropdown.options.Add(new Dropdown.OptionData(Path.GetFileName(textureData.filePath), image));
            }

            textureDropdown.value = 0;
            textureDropdown.RefreshShownValue();
            Enable();
            inited = true;
        }

        public TextureData GetSelectedTexture() {
            if (!inited) {
                return null;
            }

            return textureDropdown.value <= 0 ? null : dataSource.GetData()[textureDropdown.value - 1];
        }

        public void SetAlphaMode(ColoringComponent.COLORING_MAP_ALPHA_MODE alphaMode) {
            switch (alphaMode) {
                case ColoringComponent.COLORING_MAP_ALPHA_MODE.NONE:
                    alphaModeDropdown.value = 0;
                    break;

                case ColoringComponent.COLORING_MAP_ALPHA_MODE.AS_EMISSION_MASK:
                    alphaModeDropdown.value = 1;
                    break;

                case ColoringComponent.COLORING_MAP_ALPHA_MODE.AS_SMOOTHNESS:
                    alphaModeDropdown.value = 2;
                    break;
            }
        }

        public ColoringComponent.COLORING_MAP_ALPHA_MODE GetAlphaMode() {
            switch (alphaModeDropdown.value) {
                case 0:
                    return ColoringComponent.COLORING_MAP_ALPHA_MODE.NONE;

                case 1:
                    return ColoringComponent.COLORING_MAP_ALPHA_MODE.AS_EMISSION_MASK;

                case 2:
                    return ColoringComponent.COLORING_MAP_ALPHA_MODE.AS_SMOOTHNESS;

                default:
                    return ColoringComponent.COLORING_MAP_ALPHA_MODE.NONE;
            }
        }

        public void Disable() {
            alphaModeDropdown.interactable = false;
            textureDropdown.interactable = false;
        }

        public void Enable() {
            textureDropdown.interactable = true;
            alphaModeDropdown.interactable = textureDropdown.value != 0;
        }

        public void OnTextureDropdownChanged() {
            Enable();
        }
    }
}