using System;
using System.Collections.Generic;
using System.Threading;

namespace Platform.Kernel.OSGi.ClientCore.API {
    public class ThreadLocal<TValue> {
        readonly object Lock = new();

        Dictionary<long, TValue> map;

        public void Set(TValue value) {
            lock (Lock) {
                if (map == null) {
                    map = new Dictionary<long, TValue>();
                }

                map[Thread.CurrentThread.ManagedThreadId] = value;
            }
        }

        public TValue Get() {
            lock (Lock) {
                if (map == null) {
                    throw new ArgumentException();
                }

                return map[Thread.CurrentThread.ManagedThreadId];
            }
        }

        public bool Exists() {
            lock (Lock) {
                return map != null && map.ContainsKey(Thread.CurrentThread.ManagedThreadId);
            }
        }
    }
}