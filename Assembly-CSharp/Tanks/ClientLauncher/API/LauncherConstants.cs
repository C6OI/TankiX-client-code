namespace Tanks.ClientLauncher.API {
    public static class LauncherConstants {
        public static readonly string CONFIG_PATH = "config";

        public static readonly string CLIENT_CONFIG_PATH = CONFIG_PATH + "/clientlocal";

        public static readonly string SCENE_PATH = "Assets/tanks/ClientResources/Content/Scenes/Launcher.unity";

        public static readonly string UPDATE_PROCESS_COMMAND = "-doupdate";

        public static readonly string NO_UPDATE_COMMAND = "-noupdate";

        public static readonly string PARENT_PATH_COMMAND = "-parentPath";

        public static readonly string UPDATE_REPORT_COMMAND = "-updateReport";

        public static readonly string VERSION_COMMAND = "-version";

        public static readonly string CLEAN_PREFS = "-clean";

        public static readonly string UPDATE_PATH = "Tankix/Update";

        public static readonly string REPORT_FILE_NAME = "updatereport.xml";

        public static readonly string VERSION_CONFIG_FILE_NAME = "version.xml";

        public static readonly string TEST_SERVER = "-server";

        public static readonly string[] PASS_THROUGH = new string[1] { TEST_SERVER };
    }
}