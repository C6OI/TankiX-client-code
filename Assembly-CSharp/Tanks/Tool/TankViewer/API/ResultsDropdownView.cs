using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Tool.TankViewer.API {
    public class ResultsDropdownView : MonoBehaviour {
        public ResultsDataSource resultsDataSource;

        public Dropdown dropdown;

        public void Start() {
            if (resultsDataSource.IsReady) {
                UpdateView();
            }

            ResultsDataSource obj = resultsDataSource;
            obj.onChange = (Action)Delegate.Combine(obj.onChange, new Action(UpdateView));
        }

        public void UpdateView() {
            List<string> folderNames = resultsDataSource.GetFolderNames();
            dropdown.ClearOptions();
            dropdown.options.Add(new Dropdown.OptionData("none"));

            foreach (string item in folderNames) {
                dropdown.options.Add(new Dropdown.OptionData(item));
            }

            dropdown.value = 0;
            dropdown.RefreshShownValue();
        }

        public void SelectLastOption() {
            dropdown.value = dropdown.options.Count - 1;
            dropdown.RefreshShownValue();
        }
    }
}