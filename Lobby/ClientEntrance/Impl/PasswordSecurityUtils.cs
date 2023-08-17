using System;
using System.Security.Cryptography;
using System.Text;

namespace Lobby.ClientEntrance.Impl {
    public class PasswordSecurityUtils {
        static readonly SHA256Managed DIGEST = new();

        public static byte[] RSAEncrypt(string publicKeyBase64, byte[] data) {
            using (RSACryptoServiceProvider rSACryptoServiceProvider = new()) {
                string[] array = publicKeyBase64.Split(new char[1] { ':' }, 2);

                string xmlString = "<RSAKeyValue><Modulus>" +
                                   array[0] +
                                   "</Modulus><Exponent>" +
                                   array[1] +
                                   "</Exponent></RSAKeyValue>";

                rSACryptoServiceProvider.FromXmlString(xmlString);
                return rSACryptoServiceProvider.Encrypt(data, false);
            }
        }

        public static string RSAEncryptAsString(string publicKeyBase64, byte[] data) =>
            Convert.ToBase64String(RSAEncrypt(publicKeyBase64, data));

        public static byte[] GetDigest(string data) => DIGEST.ComputeHash(Encoding.UTF8.GetBytes(data));

        public static string GetDigestAsString(string data) =>
            Convert.ToBase64String(DIGEST.ComputeHash(Encoding.UTF8.GetBytes(data)));

        public static string GetDigestAsString(byte[] data) => Convert.ToBase64String(DIGEST.ComputeHash(data));

        public static string SaltPassword(string passcode, string password) {
            byte[] digest = GetDigest(password);
            byte[] b = Convert.FromBase64String(passcode);
            byte[] a = XorArrays(digest, b);
            return GetDigestAsString(ConcatenateArrays(a, digest));
        }

        static byte[] ConcatenateArrays(byte[] a, byte[] b) {
            byte[] array = new byte[a.Length + b.Length];
            a.CopyTo(array, 0);
            b.CopyTo(array, a.Length);
            return array;
        }

        static byte[] XorArrays(byte[] a, byte[] b) {
            if (a.Length == b.Length) {
                byte[] array = new byte[a.Length];

                for (int i = 0; i < a.Length; i++) {
                    array[i] = (byte)(a[i] ^ b[i]);
                }

                return array;
            }

            throw new ArgumentException();
        }
    }
}