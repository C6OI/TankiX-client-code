using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.ClientLauncher.Impl {
    public class LauncherDownloadScreenTextComponent : Component {
        public string DownloadText { get; set; }

        public string RebootText { get; set; }
    }
}