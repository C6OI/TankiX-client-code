using System;
using System.Collections.Generic;
using WebSocketSharp;

public class WebSocket {
    const int WEBSOCKET_CONNECTING = 0;

    const int WEBSOCKET_OPEN = 1;

    string error;
    bool isActive;

    readonly Queue<byte[]> messages = new();

    WebSocketSharp.WebSocket socket;

    public bool IsConnected { get; private set; }

    public int AvailableLength {
        get {
            if (messages.Count == 0) {
                return 0;
            }

            return messages.Peek().Length;
        }
    }

    public void ConnectAsync(string url, Action completeCallback) {
        if (isActive) {
            throw new Exception("Connection in progress");
        }

        isActive = true;
        socket = new WebSocketSharp.WebSocket(url);

        socket.OnOpen += delegate {
            IsConnected = true;
            completeCallback();
        };

        socket.OnMessage += delegate(object sender, MessageEventArgs e) {
            messages.Enqueue(e.RawData);
        };

        socket.OnError += delegate(object sender, ErrorEventArgs e) {
            error = e.Message;
        };

        socket.OnClose += delegate {
            if (!IsConnected) {
                completeCallback();
            } else {
                IsConnected = false;
            }
        };

        socket.ConnectAsync();
    }

    public void Close() {
        socket.Close();
    }

    public void Send(byte[] buffer) {
        socket.Send(buffer);
    }

    public int Receive(byte[] buffer) {
        if (messages.Count == 0) {
            return 0;
        }

        byte[] array = messages.Dequeue();
        Buffer.BlockCopy(array, 0, buffer, 0, array.Length);
        return array.Length;
    }

    public string GetError() => error;
}