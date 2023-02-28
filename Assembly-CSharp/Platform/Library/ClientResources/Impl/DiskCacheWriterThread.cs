using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Platform.Library.ClientResources.Impl {
    public class DiskCacheWriterThread {
        bool running;

        readonly Queue<DiskCacheWriterRequest> tasks = new();

        public void Stop() {
            running = false;
        }

        public DiskCacheWriterRequest Write(string path, byte[] data) {
            DiskCacheWriterRequest diskCacheWriterRequest = new();
            diskCacheWriterRequest.Data = data;
            diskCacheWriterRequest.Path = path;
            DiskCacheWriterRequest diskCacheWriterRequest2 = diskCacheWriterRequest;

            lock (tasks) {
                tasks.Enqueue(diskCacheWriterRequest2);
                return diskCacheWriterRequest2;
            }
        }

        public void Run() {
            running = true;

            while (running) {
                DiskCacheWriterRequest diskCacheWriterRequest;

                do {
                    diskCacheWriterRequest = null;

                    lock (tasks) {
                        if (tasks.Count > 0) {
                            diskCacheWriterRequest = tasks.Dequeue();
                        }
                    }

                    if (diskCacheWriterRequest != null) {
                        Write(diskCacheWriterRequest);
                    }
                } while (diskCacheWriterRequest != null);

                Thread.Sleep(50);
            }
        }

        void Write(DiskCacheWriterRequest task) {
            int num = 3;
            bool flag = false;
            FileStream fileStream = null;

            try {
                while (!File.Exists(task.Path)) {
                    try {
                        fileStream = File.Open(task.Path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                        fileStream.Write(task.Data, 0, task.Data.Length);
                        flag = true;
                    } catch (IOException ex) {
                        if (num-- <= 0) {
                            throw ex;
                        }

                        Thread.Sleep(100);
                    } finally {
                        if (fileStream != null) {
                            fileStream.Close();
                            fileStream = null;
                        }
                    }

                    if (flag) {
                        break;
                    }
                }
            } catch (Exception ex2) {
                task.Error = ex2.Message;
            } finally {
                task.IsDone = true;
            }
        }
    }
}