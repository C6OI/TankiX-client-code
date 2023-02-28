using System.Collections.Generic;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class CreateBattleScreenComponent : UIBehaviour, Component {
        [SerializeField] public CanvasGroup canvasGroup;

        [SerializeField] public RawImage mapPreviewRawImage;

        [SerializeField] public TextMeshProUGUI mapName;

        [SerializeField] public DefaultDropDownList battleModeDropdown;

        [SerializeField] public DefaultDropDownList mapDropdown;

        [SerializeField] public DefaultDropDownList maxPlayersDropdown;

        [SerializeField] public TextMeshProUGUI maxPlayerText;

        [SerializeField] public DefaultDropDownList timeLimitDropdown;

        [SerializeField] public DefaultDropDownList scoreLimitDropdown;

        [SerializeField] public DefaultDropDownList friendlyFireDropdown;

        [SerializeField] public DefaultDropDownList gravityDropdown;

        [SerializeField] public DefaultDropDownList killZoneDropdown;

        public DefaultDropDownList enabledModulesDropdown;

        [SerializeField] public Button createBattleButton;

        [SerializeField] public Button updateBattleParamsButton;

        [SerializeField] public LocalizedField minutesText;

        [SerializeField] public LocalizedField onText;

        [SerializeField] public LocalizedField offText;

        [SerializeField] public LocalizedField noLimitText;

        [SerializeField] public LocalizedField maxDmPlayerText;

        [SerializeField] public LocalizedField maxTeamPlayerText;

        public Dictionary<BattleMode, List<string>> ModesToMapsDict = new();
    }
}