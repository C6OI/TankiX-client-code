using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(635986693367977480L)]
    public class LocalizedTimerComponent : FromConfigBehaviour, Component {
        public string Second { get; set; }

        public string Minute { get; set; }

        public override string YamlKey => "localizedText";

        public override string ConfigPath => "ui/element/timer";

        public string GenerateTimerString(float time) {
            TimeSpan timeSpan = new(0, 0, 0, (int)time);
            int num = (int)timeSpan.TotalMinutes;

            if (num > 0) {
                return num + Minute + " " + AddLeadingZero(timeSpan.Seconds) + Second;
            }

            return AddLeadingZero(timeSpan.Seconds) + Second;
        }

        string AddLeadingZero(int seconds) => (seconds >= 10 ? string.Empty : "0") + seconds;
    }
}