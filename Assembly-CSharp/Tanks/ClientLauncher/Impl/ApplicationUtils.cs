using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Platform.Library.ClientResources.API;
using Tanks.ClientLauncher.API;
using UnityEngine;

namespace Tanks.ClientLauncher.Impl {
    public static class ApplicationUtils {
        public static string GetExecutableRelativePathByName(string executable) {
            if (!executable.ToLower().EndsWith(".exe")) {
                return executable + ".exe";
            }

            return executable;
        }

        public static string GetExecutablePathByName(string executable) => GetExecutableRelativePathByName(executable);

        public static string GetAppRootPath() => Directory.GetParent(Application.dataPath).FullName;

        public static void StartProcessAsAdmin(string path, string args) {
            StartProcess(path, args, true);
        }

        public static void Restart() {
            string processName = Process.GetCurrentProcess().ProcessName;
            string appRootPath = GetAppRootPath();
            StartProcess(appRootPath + "/" + GetExecutablePathByName(processName), string.Empty);
            Application.Quit();
        }

        public static void StartProcess(string path, string args) {
            StartProcess(path, args, false);
        }

        public static string WrapPath(string path) => string.Format("\"{0}\"", path);

        static void StartProcess(string path, string args, bool runAsAdministrator) {
            CommandLineParser commandLineParser = new(Environment.GetCommandLineArgs());
            string subLine = commandLineParser.GetSubLine(LauncherConstants.PASS_THROUGH);
            string text = args + " " + subLine;
            path = WrapPath(path);
            Thread.Sleep(100);
            Console.WriteLine("Run process: " + path + " " + text);
            ProcessStartInfo processStartInfo = new(path, text);
            string appRootPath = GetAppRootPath();
            string fullName = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;

            if (appRootPath.Contains(fullName)) {
                Console.WriteLine("run as administrator is disabled, path {0} contains appData {1}", appRootPath, fullName);
                runAsAdministrator = false;
            } else {
                Console.WriteLine("try run as administrator");
            }

            /*
            if (runAsAdministrator) {
                WindowsPrincipal windowsPrincipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());

                if (!windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator)) {
                    processStartInfo.Verb = "runas";
                } else {
                    Console.WriteLine("run as administrator is disabled, user already have admin rules");
                }
            }
            */

            Process.Start(processStartInfo);
        }
    }
}