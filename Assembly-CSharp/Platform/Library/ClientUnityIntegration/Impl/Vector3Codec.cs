using System.IO;
using Platform.Library.ClientProtocol.API;
using UnityEngine;

namespace Platform.Library.ClientUnityIntegration.Impl {
    public class Vector3Codec : NotOptionalCodec {
        public override void Init(Protocol protocol) { }

        public override void Encode(ProtocolBuffer protocolBuffer, object data) {
            base.Encode(protocolBuffer, data);
            Vector3 vector = (Vector3)data;
            BinaryWriter writer = protocolBuffer.Writer;
            writer.Write(vector.x);
            writer.Write(vector.y);
            writer.Write(vector.z);
        }

        public override object Decode(ProtocolBuffer protocolBuffer) {
            BinaryReader reader = protocolBuffer.Reader;
            return new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }
    }
}