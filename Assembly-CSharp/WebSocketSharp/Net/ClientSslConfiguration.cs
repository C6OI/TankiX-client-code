using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace WebSocketSharp.Net {
    public class ClientSslConfiguration : SslConfiguration {
        public ClientSslConfiguration(string targetHost)
            : this(targetHost, null, SslProtocols.Default, false) { }

        public ClientSslConfiguration(string targetHost, X509CertificateCollection clientCertificates, SslProtocols enabledSslProtocols, bool checkCertificateRevocation)
            : base(enabledSslProtocols, checkCertificateRevocation) {
            TargetHost = targetHost;
            ClientCertificates = clientCertificates;
        }

        public X509CertificateCollection ClientCertificates { get; set; }

        public LocalCertificateSelectionCallback ClientCertificateSelectionCallback {
            get => CertificateSelectionCallback;
            set => CertificateSelectionCallback = value;
        }

        public RemoteCertificateValidationCallback ServerCertificateValidationCallback {
            get => CertificateValidationCallback;
            set => CertificateValidationCallback = value;
        }

        public string TargetHost { get; set; }
    }
}