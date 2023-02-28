using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace WebSocketSharp.Net {
    public abstract class SslConfiguration {
        LocalCertificateSelectionCallback _certSelectionCallback;

        RemoteCertificateValidationCallback _certValidationCallback;

        bool _checkCertRevocation;

        SslProtocols _enabledProtocols;

        protected SslConfiguration(SslProtocols enabledSslProtocols, bool checkCertificateRevocation) {
            _enabledProtocols = enabledSslProtocols;
            _checkCertRevocation = checkCertificateRevocation;
        }

        protected LocalCertificateSelectionCallback CertificateSelectionCallback {
            get { return (object sender, string targetHost, X509CertificateCollection localCertificates, X509Certificate remoteCertificate, string[] acceptableIssuers) => null; }
            set => _certSelectionCallback = value;
        }

        protected RemoteCertificateValidationCallback CertificateValidationCallback {
            get { return (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true; }
            set => _certValidationCallback = value;
        }

        public bool CheckCertificateRevocation {
            get => _checkCertRevocation;
            set => _checkCertRevocation = value;
        }

        public SslProtocols EnabledSslProtocols {
            get => _enabledProtocols;
            set => _enabledProtocols = value;
        }
    }
}