using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using log4net.Core;
using log4net.Util;

namespace log4net.Appender {
    public class UdpAppender : AppenderSkeleton {
        int m_localPort;

        int m_remotePort;

        public IPAddress RemoteAddress { get; set; }

        public int RemotePort {
            get => m_remotePort;
            set {
                if (value < 0 || value > 65535) {
                    throw SystemInfo.CreateArgumentOutOfRangeException("value",
                        value,
                        "The value specified is less than " +
                        0.ToString(NumberFormatInfo.InvariantInfo) +
                        " or greater than " +
                        65535.ToString(NumberFormatInfo.InvariantInfo) +
                        ".");
                }

                m_remotePort = value;
            }
        }

        public int LocalPort {
            get => m_localPort;
            set {
                if (value != 0 && (value < 0 || value > 65535)) {
                    throw SystemInfo.CreateArgumentOutOfRangeException("value",
                        value,
                        "The value specified is less than " +
                        0.ToString(NumberFormatInfo.InvariantInfo) +
                        " or greater than " +
                        65535.ToString(NumberFormatInfo.InvariantInfo) +
                        ".");
                }

                m_localPort = value;
            }
        }

        public Encoding Encoding { get; set; } = Encoding.Default;

        protected UdpClient Client { get; set; }

        protected IPEndPoint RemoteEndPoint { get; set; }

        protected override bool RequiresLayout => true;

        public override void ActivateOptions() {
            base.ActivateOptions();

            if (RemoteAddress == null) {
                throw new ArgumentNullException("The required property 'Address' was not specified.");
            }

            if (RemotePort < 0 || RemotePort > 65535) {
                throw SystemInfo.CreateArgumentOutOfRangeException("this.RemotePort",
                    RemotePort,
                    "The RemotePort is less than " +
                    0.ToString(NumberFormatInfo.InvariantInfo) +
                    " or greater than " +
                    65535.ToString(NumberFormatInfo.InvariantInfo) +
                    ".");
            }

            if (LocalPort != 0 && (LocalPort < 0 || LocalPort > 65535)) {
                throw SystemInfo.CreateArgumentOutOfRangeException("this.LocalPort",
                    LocalPort,
                    "The LocalPort is less than " +
                    0.ToString(NumberFormatInfo.InvariantInfo) +
                    " or greater than " +
                    65535.ToString(NumberFormatInfo.InvariantInfo) +
                    ".");
            }

            RemoteEndPoint = new IPEndPoint(RemoteAddress, RemotePort);
            InitializeClientConnection();
        }

        protected override void Append(LoggingEvent loggingEvent) {
            try {
                byte[] bytes = Encoding.GetBytes(RenderLoggingEvent(loggingEvent).ToCharArray());
                Client.Send(bytes, bytes.Length, RemoteEndPoint);
            } catch (Exception e) {
                ErrorHandler.Error(
                    "Unable to send logging event to remote host " + RemoteAddress + " on port " + RemotePort + ".",
                    e,
                    ErrorCode.WriteFailure);
            }
        }

        protected override void OnClose() {
            base.OnClose();

            if (Client != null) {
                Client.Close();
                Client = null;
            }
        }

        protected virtual void InitializeClientConnection() {
            try {
                if (LocalPort == 0) {
                    Client = new UdpClient(RemoteAddress.AddressFamily);
                } else {
                    Client = new UdpClient(LocalPort, RemoteAddress.AddressFamily);
                }
            } catch (Exception e) {
                ErrorHandler.Error(
                    "Could not initialize the UdpClient connection on port " +
                    LocalPort.ToString(NumberFormatInfo.InvariantInfo) +
                    ".",
                    e,
                    ErrorCode.GenericFailure);

                Client = null;
            }
        }
    }
}