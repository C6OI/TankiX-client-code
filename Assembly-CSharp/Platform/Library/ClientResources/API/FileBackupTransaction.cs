using System;
using System.Collections.Generic;
using System.IO;

namespace Platform.Library.ClientResources.API {
    public class FileBackupTransaction : IDisposable {
        readonly Stack<Task> history = new();

        public void Dispose() {
            Rollback();
        }

        public void DeleteFile(string path) {
            DeleteTask deleteTask = new(path);
            deleteTask.Run();
            history.Push(deleteTask);
        }

        public void CopyFile(string fromPath, string toPath) {
            CopyTask copyTask = new(fromPath, toPath);
            copyTask.Run();
            history.Push(copyTask);
        }

        public void ReplaceFile(string fromPath, string toPath) {
            if (File.Exists(toPath)) {
                DeleteFile(toPath);
            } else {
                string directoryName = Path.GetDirectoryName(toPath);

                if (!Directory.Exists(directoryName)) {
                    Directory.CreateDirectory(directoryName);
                }
            }

            CopyFile(fromPath, toPath);
        }

        public void Commit() {
            while (history.Count > 0) {
                try {
                    history.Pop().Commit();
                } catch {
                    history.Clear();
                    throw;
                }
            }
        }

        public void Rollback() {
            while (history.Count > 0) {
                history.Pop().Rollback();
            }
        }

        interface Task {
            Task Run();

            void Rollback();

            void Commit();
        }

        class DeleteTask : Task {
            readonly string backupPath;
            readonly string path;

            public DeleteTask(string path) {
                this.path = path;
                backupPath = string.Format("{0}.bck", path);
            }

            public Task Run() {
                File.SetAttributes(path, FileAttributes.Archive);

                if (File.Exists(backupPath)) {
                    File.SetAttributes(backupPath, FileAttributes.Archive);
                    File.Delete(backupPath);
                }

                File.Copy(path, backupPath);
                File.Delete(path);
                return this;
            }

            public void Rollback() {
                if (File.Exists(path)) {
                    File.SetAttributes(path, FileAttributes.Archive);
                    File.Delete(path);
                }

                File.Copy(backupPath, path);
                File.SetAttributes(backupPath, FileAttributes.Archive);
                File.Delete(backupPath);
            }

            public void Commit() {
                File.SetAttributes(backupPath, FileAttributes.Archive);
                File.Delete(backupPath);
            }
        }

        class CopyTask : Task {
            readonly string formPath;

            readonly string toPath;

            public CopyTask(string formPath, string toPath) {
                this.formPath = formPath;
                this.toPath = toPath;
            }

            public Task Run() {
                File.Copy(formPath, toPath);
                return this;
            }

            public void Rollback() {
                if (File.Exists(toPath)) {
                    File.SetAttributes(toPath, FileAttributes.Archive);
                    File.Delete(toPath);
                }
            }

            public void Commit() { }
        }
    }
}