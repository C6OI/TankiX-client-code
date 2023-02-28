namespace Steamworks {
    public static class SteamScreenshots {
        public static ScreenshotHandle WriteScreenshot(byte[] pubRGB, uint cubRGB, int nWidth, int nHeight) {
            InteropHelp.TestIfAvailableClient();
            return (ScreenshotHandle)NativeMethods.ISteamScreenshots_WriteScreenshot(pubRGB, cubRGB, nWidth, nHeight);
        }

        public static ScreenshotHandle AddScreenshotToLibrary(string pchFilename, string pchThumbnailFilename, int nWidth, int nHeight) {
            InteropHelp.TestIfAvailableClient();

            using (InteropHelp.UTF8StringHandle pchFilename2 = new(pchFilename)) {
                using (InteropHelp.UTF8StringHandle pchThumbnailFilename2 = new(pchThumbnailFilename)) {
                    return (ScreenshotHandle)NativeMethods.ISteamScreenshots_AddScreenshotToLibrary(pchFilename2, pchThumbnailFilename2, nWidth, nHeight);
                }
            }
        }

        public static void TriggerScreenshot() {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamScreenshots_TriggerScreenshot();
        }

        public static void HookScreenshots(bool bHook) {
            InteropHelp.TestIfAvailableClient();
            NativeMethods.ISteamScreenshots_HookScreenshots(bHook);
        }

        public static bool SetLocation(ScreenshotHandle hScreenshot, string pchLocation) {
            InteropHelp.TestIfAvailableClient();

            using (InteropHelp.UTF8StringHandle pchLocation2 = new(pchLocation)) {
                return NativeMethods.ISteamScreenshots_SetLocation(hScreenshot, pchLocation2);
            }
        }

        public static bool TagUser(ScreenshotHandle hScreenshot, CSteamID steamID) {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamScreenshots_TagUser(hScreenshot, steamID);
        }

        public static bool TagPublishedFile(ScreenshotHandle hScreenshot, PublishedFileId_t unPublishedFileID) {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamScreenshots_TagPublishedFile(hScreenshot, unPublishedFileID);
        }

        public static bool IsScreenshotsHooked() {
            InteropHelp.TestIfAvailableClient();
            return NativeMethods.ISteamScreenshots_IsScreenshotsHooked();
        }

        public static ScreenshotHandle AddVRScreenshotToLibrary(EVRScreenshotType eType, string pchFilename, string pchVRFilename) {
            InteropHelp.TestIfAvailableClient();

            using (InteropHelp.UTF8StringHandle pchFilename2 = new(pchFilename)) {
                using (InteropHelp.UTF8StringHandle pchVRFilename2 = new(pchVRFilename)) {
                    return (ScreenshotHandle)NativeMethods.ISteamScreenshots_AddVRScreenshotToLibrary(eType, pchFilename2, pchVRFilename2);
                }
            }
        }
    }
}