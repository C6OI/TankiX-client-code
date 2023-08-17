using System;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace Lobby.ClientPayment.Impl {
    public class Encrypter {
        public const string PREFIX = "adyenc#";

        public const string VERSION = "0_1_15";

        public const string SEPARATOR = "$";

        readonly string publicKey;

        CcmBlockCipher aesCipher;

        IBufferedCipher rsaCipher;

        public Encrypter(string publicKey) {
            this.publicKey = publicKey;
            InitializeRSA();
        }

        void InitializeRSA() {
            string[] array = publicKey.Split('|');
            BigInteger modulus = new(array[1].ToLower(), 16);
            BigInteger exponent = new(array[0].ToLower(), 16);
            RsaKeyParameters parameters = new(false, modulus, exponent);
            rsaCipher = CipherUtilities.GetCipher("RSA/None/PKCS1Padding");
            rsaCipher.Init(true, parameters);
        }

        public string Encrypt(string data) {
            SecureRandom secureRandom = new();
            byte[] array = new byte[32];
            secureRandom.NextBytes(array);
            byte[] array2 = new byte[12];
            secureRandom.NextBytes(array2);
            byte[] inArray = rsaCipher.DoFinal(array);
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            AeadParameters parameters = new(new KeyParameter(array), 64, array2, new byte[0]);
            aesCipher = new CcmBlockCipher(new AesFastEngine());
            aesCipher.Init(true, parameters);
            byte[] array3 = new byte[aesCipher.GetOutputSize(bytes.Length)];
            int outOff = aesCipher.ProcessBytes(bytes, 0, bytes.Length, array3, 0);
            aesCipher.DoFinal(array3, outOff);
            byte[] array4 = new byte[array2.Length + array3.Length];
            Buffer.BlockCopy(array2, 0, array4, 0, array2.Length);
            Buffer.BlockCopy(array3, 0, array4, array2.Length, array3.Length);
            return "adyenc#0_1_15$" + Convert.ToBase64String(inArray) + "$" + Convert.ToBase64String(array4);
        }
    }
}