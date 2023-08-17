using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientResources.API;
using Tanks.ClientLauncher.API;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Tanks.ClientLauncher.Impl {
    public static class ClientUpdater {
        static readonly int PROCESS_STOP_TIMEOUT = 15000;

        static readonly int FILE_WAIT_TIMEOUT = 60000;

        public static bool IsApplicationRunInUpdateMode() {
            CommandLineParser commandLineParser = new(Environment.GetCommandLineArgs());
            string paramValue;

            if (commandLineParser.TryGetValue(LauncherConstants.UPDATE_PROCESS_COMMAND, out paramValue)) {
                return true;
            }

            return false;
        }

        public static void Update() {
            UpdateReport updateReport = new();
            FileBackupTransaction fileBackupTransaction = new();
            string path = null;
            string text = null;
            string updateVersion = "unknown";
            string text2 = null;

            try {
                string appRootPath = ApplicationUtils.GetAppRootPath();
                CommandLineParser commandLineParser = new(Environment.GetCommandLineArgs());
                int value = Convert.ToInt32(commandLineParser.GetValue(LauncherConstants.UPDATE_PROCESS_COMMAND));
                text = commandLineParser.GetValue(LauncherConstants.PARENT_PATH_COMMAND);
                updateVersion = commandLineParser.GetValue(LauncherConstants.VERSION_COMMAND);
                text2 = text + "/update.lock";

                using (File.Open(text2, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None)) {
                    string processName = Process.GetCurrentProcess().ProcessName;
                    path = text + "/" + ApplicationUtils.GetExecutableRelativePathByName(processName);
                    WaitForProccessStop(Convert.ToInt32(value), PROCESS_STOP_TIMEOUT);
                    WaitForDropParentExecutable(fileBackupTransaction, path);
                    ReplaceProjectFiles(fileBackupTransaction, appRootPath, text, processName);
                    fileBackupTransaction.Commit();
                }
            } catch (Exception ex) {
                updateReport.IsSuccess = false;
                updateReport.Error = ex.Message;
                updateReport.StackTrace = ex.StackTrace;
                fileBackupTransaction.Rollback();
            } finally {
                try {
                    ApplicationUtils.StartProcess(path, LauncherConstants.UPDATE_REPORT_COMMAND);
                    updateReport.UpdateVersion = updateVersion;
                    WriteReport(text + "/" + LauncherConstants.REPORT_FILE_NAME, updateReport);

                    if (!string.IsNullOrEmpty(text2) && File.Exists(text2)) {
                        File.Delete(text2);
                    }
                } finally {
                    Application.Quit();
                }
            }
        }

        static void WriteReport(string path, UpdateReport report) {
            try {
                using (FileStream stream = new(path, FileMode.Create)) {
                    report.Write(stream);
                }
            } catch (Exception message) {
                Debug.LogError(message);
            }
        }

        static bool WaitForProccessStop(int parentProcessId, int timeout) {
            Process processById;

            try {
                processById = Process.GetProcessById(parentProcessId);
            } catch (ArgumentException) {
                return true;
            }

            processById.WaitForExit(timeout);
            return processById.HasExited;
        }

        static void WaitForDropParentExecutable(FileBackupTransaction transaction, string path) {
            if (!File.Exists(path)) {
                return;
            }

            int num = 0;
            bool flag;

            do {
                flag = true;

                try {
                    Console.WriteLine("Try drop executable file " + path);
                    transaction.DeleteFile(path);
                    Console.WriteLine("Executable file droped");
                } catch (Exception) {
                    flag = false;

                    if (num > FILE_WAIT_TIMEOUT) {
                        throw;
                    }

                    num += 1000;
                    Thread.Sleep(1000);
                }
            } while (!flag);
        }

        static void ReplaceProjectFiles(FileBackupTransaction transaction, string from, string to,
            string executableWithoutExtension) {
            string[] files = Directory.GetFiles(from, "*", SearchOption.AllDirectories);
            List<Pair<string, string>> list = new();
            List<Pair<string, string>> list2 = new();
            string[] array = files;

            foreach (string text in array) {
                string text2 = text.Replace(from, to);

                if (Path.GetFileNameWithoutExtension(text2)
                    .Equals(executableWithoutExtension, StringComparison.OrdinalIgnoreCase)) {
                    list2.Add(new Pair<string, string>(text, text2));
                } else {
                    list.Add(new Pair<string, string>(text, text2));
                }
            }

            foreach (Pair<string, string> item in list) {
                Console.WriteLine("Copy project file from " + item.Key + " to " + item.Value);
                transaction.ReplaceFile(item.Key, item.Value);
            }

            foreach (Pair<string, string> item2 in list2) {
                Console.WriteLine("Copy executable file from " + item2.Key + " to " + item2.Value);
                transaction.ReplaceFile(item2.Key, item2.Value);
            }
        }
    }
}