using System.Collections.Generic;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Lobby.ClientBattleSelect.API {
    public class GameModesDescriptionData {
        public const string configPath = "localization/battle_mode";

        public Dictionary<BattleMode, string> battleModeLocalization { get; set; }
    }
}