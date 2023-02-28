using System.Collections.Generic;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.System.Data.Statics.ClientConfigurator.API;
using Tanks.Lobby.ClientBattleSelect.API;

namespace Tanks.Battle.ClientCore.API {
    public class BattleModeLocalizationUtil {
        static Dictionary<BattleMode, string> modeToName;

        [Inject] public static ConfigurationService ConfiguratorService { get; set; }

        public static string GetLocalization(BattleMode mode) {
            CheckAndCreate();
            return modeToName[mode];
        }

        public static Dictionary<BattleMode, string> GetModeToNameDict() {
            CheckAndCreate();
            return modeToName;
        }

        static void CheckAndCreate() {
            if (modeToName == null) {
                modeToName = ConfiguratorService.GetConfig("localization/battle_mode").ConvertTo<GameModesDescriptionData>().battleModeLocalization;
            }
        }
    }
}