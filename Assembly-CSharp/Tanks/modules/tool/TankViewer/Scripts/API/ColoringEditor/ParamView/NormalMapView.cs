using System;
using System.Collections.Generic;
using System.IO;
using Tanks.Tool.TankViewer.API;
using UnityEngine;
using UnityEngine.UI;

namespace tanks.modules.tool.TankViewer.Scripts.API.ColoringEditor.ParamView {
    public class NormalMapView : MonoBehaviour {
        public Dropdown normalMapDropdown;

        public InputField normalScaleInput;

        public TextureDataSource dataSource;

        bool inited;

        public void Awake() {
            TextureDataSource textureDataSource = dataSource;

            textureDataSource.onStartUpdatingAction = (Action)Delegate.Combine(textureDataSource.onStartUpdatingAction, (Action)delegate {
                inited = false;
                normalMapDropdown.ClearOptions();
                Disable();
            });

            TextureDataSource textureDataSource2 = dataSource;
            textureDataSource2.onCompleteUpdatingAction = (Action)Delegate.Combine(textureDataSource2.onCompleteUpdatingAction, new Action(UpdateView));

            if (dataSource.TexturesAreReady) {
                UpdateView();
            }
        }

        public void UpdateView() {
            normalMapDropdown.ClearOptions();
            List<TextureData> data = dataSource.GetData();
            normalMapDropdown.options.Add(new Dropdown.OptionData("none"));

            for (int i = 0; i < data.Count; i++) {
                TextureData textureData = data[i];
                Texture2D texture2D = textureData.texture2D;
                Sprite image = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
                normalMapDropdown.options.Add(new Dropdown.OptionData(Path.GetFileName(textureData.filePath), image));
            }

            normalMapDropdown.value = 0;
            normalMapDropdown.RefreshShownValue();
            Enable();
            inited = true;
        }

        public TextureData GetSelectedNormalMap() {
            if (!inited) {
                return null;
            }

            if (normalMapDropdown.value <= 0) {
                return null;
            }

            int index = normalMapDropdown.value - 1;
            return dataSource.GetNormalMapsData()[index];
        }

        public void SetNormalScale(float scale) {
            normalScaleInput.text = scale.ToString();
        }

        public float GetNormalScale() {
            if (string.IsNullOrEmpty(normalScaleInput.text)) {
                normalScaleInput.text = "1";
            }

            return float.Parse(normalScaleInput.text);
        }

        public void OnNormalDropdownChanged() {
            Enable();
        }

        public void Disable() {
            normalScaleInput.interactable = false;
            normalMapDropdown.interactable = false;
        }

        public void Enable() {
            normalMapDropdown.interactable = true;
            normalScaleInput.interactable = normalMapDropdown.value != 0;
        }
    }
}