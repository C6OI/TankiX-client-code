using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using WebSocketSharp.Net;

namespace WebSocketSharp.Server {
    public class WebSocketServiceManager {
        volatile bool _clean;

        readonly Dictionary<string, WebSocketServiceHost> _hosts;

        readonly Logger _logger;

        volatile ServerState _state;

        readonly object _sync;

        TimeSpan _waitTime;

        internal WebSocketServiceManager()
            : this(new Logger()) { }

        internal WebSocketServiceManager(Logger logger) {
            _logger = logger;
            _clean = true;
            _hosts = new Dictionary<string, WebSocketServiceHost>();
            _state = ServerState.Ready;
            _sync = ((ICollection)_hosts).SyncRoot;
            _waitTime = TimeSpan.FromSeconds(1.0);
        }

        public int Count {
            get {
                lock (_sync) {
                    return _hosts.Count;
                }
            }
        }

        public IEnumerable<WebSocketServiceHost> Hosts {
            get {
                lock (_sync) {
                    return _hosts.Values.ToList();
                }
            }
        }

        public WebSocketServiceHost this[string path] {
            get {
                WebSocketServiceHost host;
                TryGetServiceHost(path, out host);
                return host;
            }
        }

        public bool KeepClean {
            get => _clean;
            internal set {
                lock (_sync) {
                    if (!(value ^ _clean)) {
                        return;
                    }

                    _clean = value;

                    foreach (WebSocketServiceHost value2 in _hosts.Values) {
                        value2.KeepClean = value;
                    }
                }
            }
        }

        public IEnumerable<string> Paths {
            get {
                lock (_sync) {
                    return _hosts.Keys.ToList();
                }
            }
        }

        public int SessionCount {
            get {
                int num = 0;

                foreach (WebSocketServiceHost host in Hosts) {
                    if (_state == ServerState.Start) {
                        num += host.Sessions.Count;
                        continue;
                    }

                    return num;
                }

                return num;
            }
        }

        public TimeSpan WaitTime {
            get => _waitTime;
            internal set {
                lock (_sync) {
                    if (value == _waitTime) {
                        return;
                    }

                    _waitTime = value;

                    foreach (WebSocketServiceHost value2 in _hosts.Values) {
                        value2.WaitTime = value;
                    }
                }
            }
        }

        void broadcast(Opcode opcode, byte[] data, Action completed) {
            Dictionary<CompressionMethod, byte[]> dictionary = new();

            try {
                foreach (WebSocketServiceHost host in Hosts) {
                    if (_state != ServerState.Start) {
                        break;
                    }

                    host.Sessions.Broadcast(opcode, data, dictionary);
                }

                if (completed != null) {
                    completed();
                }
            } catch (Exception ex) {
                _logger.Fatal(ex.ToString());
            } finally {
                dictionary.Clear();
            }
        }

        void broadcast(Opcode opcode, Stream stream, Action completed) {
            Dictionary<CompressionMethod, Stream> dictionary = new();

            try {
                foreach (WebSocketServiceHost host in Hosts) {
                    if (_state != ServerState.Start) {
                        break;
                    }

                    host.Sessions.Broadcast(opcode, stream, dictionary);
                }

                if (completed != null) {
                    completed();
                }
            } catch (Exception ex) {
                _logger.Fatal(ex.ToString());
            } finally {
                foreach (Stream value in dictionary.Values) {
                    value.Dispose();
                }

                dictionary.Clear();
            }
        }

        void broadcastAsync(Opcode opcode, byte[] data, Action completed) {
            ThreadPool.QueueUserWorkItem(delegate {
                broadcast(opcode, data, completed);
            });
        }

        void broadcastAsync(Opcode opcode, Stream stream, Action completed) {
            ThreadPool.QueueUserWorkItem(delegate {
                broadcast(opcode, stream, completed);
            });
        }

        Dictionary<string, Dictionary<string, bool>> broadping(byte[] frameAsBytes, TimeSpan timeout) {
            Dictionary<string, Dictionary<string, bool>> dictionary = new();

            foreach (WebSocketServiceHost host in Hosts) {
                if (_state == ServerState.Start) {
                    dictionary.Add(host.Path, host.Sessions.Broadping(frameAsBytes, timeout));
                    continue;
                }

                return dictionary;
            }

            return dictionary;
        }

        internal void Add<TBehavior>(string path, Func<TBehavior> initializer) where TBehavior : WebSocketBehavior {
            lock (_sync) {
                path = HttpUtility.UrlDecode(path).TrimEndSlash();
                WebSocketServiceHost value;

                if (_hosts.TryGetValue(path, out value)) {
                    _logger.Error("A WebSocket service with the specified path already exists:\n  path: " + path);
                    return;
                }

                value = new WebSocketServiceHost<TBehavior>(path, initializer, _logger);

                if (!_clean) {
                    value.KeepClean = false;
                }

                if (_waitTime != value.WaitTime) {
                    value.WaitTime = _waitTime;
                }

                if (_state == ServerState.Start) {
                    value.Start();
                }

                _hosts.Add(path, value);
            }
        }

        internal bool InternalTryGetServiceHost(string path, out WebSocketServiceHost host) {
            bool flag;

            lock (_sync) {
                path = HttpUtility.UrlDecode(path).TrimEndSlash();
                flag = _hosts.TryGetValue(path, out host);
            }

            if (!flag) {
                _logger.Error("A WebSocket service with the specified path isn't found:\n  path: " + path);
            }

            return flag;
        }

        internal bool Remove(string path) {
            WebSocketServiceHost value;

            lock (_sync) {
                path = HttpUtility.UrlDecode(path).TrimEndSlash();

                if (!_hosts.TryGetValue(path, out value)) {
                    _logger.Error("A WebSocket service with the specified path isn't found:\n  path: " + path);
                    return false;
                }

                _hosts.Remove(path);
            }

            if (value.State == ServerState.Start) {
                value.Stop(1001, null);
            }

            return true;
        }

        internal void Start() {
            lock (_sync) {
                foreach (WebSocketServiceHost value in _hosts.Values) {
                    value.Start();
                }

                _state = ServerState.Start;
            }
        }

        internal void Stop(CloseEventArgs e, bool send, bool receive) {
            lock (_sync) {
                _state = ServerState.ShuttingDown;
                byte[] frameAsBytes = !send ? null : WebSocketFrame.CreateCloseFrame(e.PayloadData, false).ToArray();

                foreach (WebSocketServiceHost value in _hosts.Values) {
                    value.Sessions.Stop(e, frameAsBytes, receive);
                }

                _hosts.Clear();
                _state = ServerState.Stop;
            }
        }

        public void Broadcast(byte[] data) {
            string text = _state.CheckIfAvailable(false, true, false) ?? WebSocket.CheckSendParameter(data);

            if (text != null) {
                _logger.Error(text);
            } else if (data.LongLength <= WebSocket.FragmentLength) {
                broadcast(Opcode.Binary, data, null);
            } else {
                broadcast(Opcode.Binary, new MemoryStream(data), null);
            }
        }

        public void Broadcast(string data) {
            string text = _state.CheckIfAvailable(false, true, false) ?? WebSocket.CheckSendParameter(data);

            if (text != null) {
                _logger.Error(text);
                return;
            }

            byte[] array = data.UTF8Encode();

            if (array.LongLength <= WebSocket.FragmentLength) {
                broadcast(Opcode.Text, array, null);
            } else {
                broadcast(Opcode.Text, new MemoryStream(array), null);
            }
        }

        public void BroadcastAsync(byte[] data, Action completed) {
            string text = _state.CheckIfAvailable(false, true, false) ?? WebSocket.CheckSendParameter(data);

            if (text != null) {
                _logger.Error(text);
            } else if (data.LongLength <= WebSocket.FragmentLength) {
                broadcastAsync(Opcode.Binary, data, completed);
            } else {
                broadcastAsync(Opcode.Binary, new MemoryStream(data), completed);
            }
        }

        public void BroadcastAsync(string data, Action completed) {
            string text = _state.CheckIfAvailable(false, true, false) ?? WebSocket.CheckSendParameter(data);

            if (text != null) {
                _logger.Error(text);
                return;
            }

            byte[] array = data.UTF8Encode();

            if (array.LongLength <= WebSocket.FragmentLength) {
                broadcastAsync(Opcode.Text, array, completed);
            } else {
                broadcastAsync(Opcode.Text, new MemoryStream(array), completed);
            }
        }

        public void BroadcastAsync(Stream stream, int length, Action completed) {
            string text = _state.CheckIfAvailable(false, true, false) ?? WebSocket.CheckSendParameters(stream, length);

            if (text != null) {
                _logger.Error(text);
                return;
            }

            stream.ReadBytesAsync(length, delegate(byte[] data) {
                int num = data.Length;

                if (num == 0) {
                    _logger.Error("The data cannot be read from 'stream'.");
                } else {
                    if (num < length) {
                        _logger.Warn(string.Format("The data with 'length' cannot be read from 'stream':\n  expected: {0}\n  actual: {1}", length, num));
                    }

                    if (num <= WebSocket.FragmentLength) {
                        broadcast(Opcode.Binary, data, completed);
                    } else {
                        broadcast(Opcode.Binary, new MemoryStream(data), completed);
                    }
                }
            }, delegate(Exception ex) {
                _logger.Fatal(ex.ToString());
            });
        }

        public Dictionary<string, Dictionary<string, bool>> Broadping() {
            string text = _state.CheckIfAvailable(false, true, false);

            if (text != null) {
                _logger.Error(text);
                return null;
            }

            return broadping(WebSocketFrame.EmptyPingBytes, _waitTime);
        }

        public Dictionary<string, Dictionary<string, bool>> Broadping(string message) {
            if (message == null || message.Length == 0) {
                return Broadping();
            }

            byte[] bytes = null;
            string text = _state.CheckIfAvailable(false, true, false) ?? WebSocket.CheckPingParameter(message, out bytes);

            if (text != null) {
                _logger.Error(text);
                return null;
            }

            return broadping(WebSocketFrame.CreatePingFrame(bytes, false).ToArray(), _waitTime);
        }

        public bool TryGetServiceHost(string path, out WebSocketServiceHost host) {
            string text = _state.CheckIfAvailable(false, true, false) ?? path.CheckIfValidServicePath();

            if (text != null) {
                _logger.Error(text);
                host = null;
                return false;
            }

            return InternalTryGetServiceHost(path, out host);
        }
    }
}