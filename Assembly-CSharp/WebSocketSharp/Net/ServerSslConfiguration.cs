using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace WebSocketSharp.Net {
    public class ServerSslConfiguration : SslConfiguration {
        public ServerSslConfiguration(X509Certificate2 serverCertificate)
            : this(serverCertificate, false, SslProtocols.Default, false) { }

        public ServerSslConfiguration(X509Certificate2 serverCertificate, bool clientCertificateRequired, SslProtocols enabledSslProtocols, bool checkCertificateRevocation)
            : base(enabledSslProtocols, checkCertificateRevocation) {
            ServerCertificate = serverCertificate;
            ClientCertificateRequired = clientCertificateRequired;
        }

        public bool ClientCertificateRequired { get; set; }

        public RemoteCertificateValidationCallback ClientCertificateValidationCallback {
            get => CertificateValidationCallback;
            set => CertificateValidationCallback = value;
        }

        public X509Certificate2 ServerCertificate { get; set; }
    }
}