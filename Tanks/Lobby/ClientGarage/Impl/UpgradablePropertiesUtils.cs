namespace Tanks.Lobby.ClientGarage.Impl {
    public static class UpgradablePropertiesUtils {
        const float MAX_LEVEL = 100f;

        public static float GetValue(this UpdateItemPropertiesEvent e, UpgradablePropertyComponent property) =>
            GetValue(property, e.Level);

        public static float GetNextValue(this UpdateItemPropertiesEvent e, UpgradablePropertyComponent property) =>
            GetValue(property, e.Level + 1);

        static float GetValue(UpgradablePropertyComponent property, long level) =>
            property.InitialValue + (property.FinalValue - property.InitialValue) * level / 100f;
    }
}