using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientLoading.API {
    public class LoadScreenLocalizedTextComponent : Component {
        public string LoadFromDiskText { get; set; }

        public string NetworkLoadText { get; set; }

        public string InitializationText { get; set; }

        public string SkipLoadingButton { get; set; }
    }
}