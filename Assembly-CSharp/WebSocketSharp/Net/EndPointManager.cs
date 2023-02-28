using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace WebSocketSharp.Net {
    internal sealed class EndPointManager {
        static readonly Dictionary<IPAddress, Dictionary<int, EndPointListener>> _addressToEndpoints;

        static EndPointManager() => _addressToEndpoints = new Dictionary<IPAddress, Dictionary<int, EndPointListener>>();

        EndPointManager() { }

        static void addPrefix(string uriPrefix, HttpListener listener) {
            HttpListenerPrefix httpListenerPrefix = new(uriPrefix);
            string path = httpListenerPrefix.Path;

            if (path.IndexOf('%') != -1) {
                throw new HttpListenerException(87, "Includes an invalid path.");
            }

            if (path.IndexOf("//", StringComparison.Ordinal) != -1) {
                throw new HttpListenerException(87, "Includes an invalid path.");
            }

            getEndPointListener(httpListenerPrefix, listener).AddPrefix(httpListenerPrefix, listener);
        }

        static IPAddress convertToIPAddress(string hostname) {
            if (hostname == "*" || hostname == "+") {
                return IPAddress.Any;
            }

            IPAddress address;

            if (IPAddress.TryParse(hostname, out address)) {
                return address;
            }

            try {
                IPHostEntry hostEntry = Dns.GetHostEntry(hostname);
                return hostEntry == null ? IPAddress.Any : hostEntry.AddressList[0];
            } catch {
                return IPAddress.Any;
            }
        }

        static EndPointListener getEndPointListener(HttpListenerPrefix prefix, HttpListener listener) {
            IPAddress iPAddress = convertToIPAddress(prefix.Host);
            Dictionary<int, EndPointListener> dictionary = null;

            if (_addressToEndpoints.ContainsKey(iPAddress)) {
                dictionary = _addressToEndpoints[iPAddress];
            } else {
                dictionary = new Dictionary<int, EndPointListener>();
                _addressToEndpoints[iPAddress] = dictionary;
            }

            int port = prefix.Port;
            EndPointListener endPointListener = null;

            return dictionary.ContainsKey(port) ? dictionary[port]
                       : dictionary[port] = new EndPointListener(iPAddress, port, listener.ReuseAddress, prefix.IsSecure, listener.CertificateFolderPath, listener.SslConfiguration);
        }

        static void removePrefix(string uriPrefix, HttpListener listener) {
            HttpListenerPrefix httpListenerPrefix = new(uriPrefix);
            string path = httpListenerPrefix.Path;

            if (path.IndexOf('%') == -1 && path.IndexOf("//", StringComparison.Ordinal) == -1) {
                getEndPointListener(httpListenerPrefix, listener).RemovePrefix(httpListenerPrefix, listener);
            }
        }

        internal static void RemoveEndPoint(EndPointListener listener) {
            lock (((ICollection)_addressToEndpoints).SyncRoot) {
                IPAddress address = listener.Address;
                Dictionary<int, EndPointListener> dictionary = _addressToEndpoints[address];
                dictionary.Remove(listener.Port);

                if (dictionary.Count == 0) {
                    _addressToEndpoints.Remove(address);
                }

                listener.Close();
            }
        }

        public static void AddListener(HttpListener listener) {
            List<string> list = new();

            lock (((ICollection)_addressToEndpoints).SyncRoot) {
                try {
                    foreach (string prefix in listener.Prefixes) {
                        addPrefix(prefix, listener);
                        list.Add(prefix);
                    }
                } catch {
                    foreach (string item in list) {
                        removePrefix(item, listener);
                    }

                    throw;
                }
            }
        }

        public static void AddPrefix(string uriPrefix, HttpListener listener) {
            lock (((ICollection)_addressToEndpoints).SyncRoot) {
                addPrefix(uriPrefix, listener);
            }
        }

        public static void RemoveListener(HttpListener listener) {
            lock (((ICollection)_addressToEndpoints).SyncRoot) {
                foreach (string prefix in listener.Prefixes) {
                    removePrefix(prefix, listener);
                }
            }
        }

        public static void RemovePrefix(string uriPrefix, HttpListener listener) {
            lock (((ICollection)_addressToEndpoints).SyncRoot) {
                removePrefix(uriPrefix, listener);
            }
        }
    }
}