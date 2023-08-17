using System.Collections.Generic;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.System.Data.Statics.ClientConfigurator.API;
using Platform.System.Data.Statics.ClientYaml.API;
using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientLoading.Impl {
    public class BattleHintsComponent : MonoBehaviour, Component {
        public string hintsConfig;

        int currentHintIndex;

        List<string> hints;

        float lastChangeHintTime;

        Text text;

        int updateTimeInSec;

        [Inject] public static ConfigurationService ConfigurationService { get; set; }

        void Awake() {
            text = GetComponent<Text>();
            ParseConfig();
            currentHintIndex = -1;
            SetNextHintText();
        }

        void Update() {
            if (hints != null && hints.Count > 1 && Time.realtimeSinceStartup - lastChangeHintTime >= updateTimeInSec) {
                SetNextHintText();
                lastChangeHintTime = Time.realtimeSinceStartup;
            }
        }

        void OnEnable() => lastChangeHintTime = Time.realtimeSinceStartup;

        void OnDisable() {
            if (Time.realtimeSinceStartup - lastChangeHintTime > 2f) {
                SetNextHintText();
            }
        }

        void SetNextHintText() {
            currentHintIndex = currentHintIndex < hints.Count - 1 ? currentHintIndex + 1 : 0;
            text.text = hints[currentHintIndex];
        }

        void ParseConfig() {
            YamlNode config = ConfigurationService.GetConfig(hintsConfig);
            YamlNode childNode = config.GetChildNode("battleHints");
            hints = childNode.GetChildListValues("collection");

            for (int i = 0; i < hints.Count; i++) {
                hints[i] = hints[i].TrimEnd('\n');
            }

            updateTimeInSec = int.Parse(childNode.GetStringValue("updateTimeInSec"));
        }
    }
}