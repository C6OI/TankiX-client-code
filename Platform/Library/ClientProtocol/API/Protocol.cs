using System;
using Platform.Library.ClientProtocol.Impl;

namespace Platform.Library.ClientProtocol.API {
    public interface Protocol {
        Codec GetCodec(CodecInfoWithAttributes infoWithAttributes);

        Codec GetCodec(Type type);

        Codec GetCodec(long uid);

        void RegisterCodecForType<T>(Codec codec);

        void RegisterInheritanceCodecForType<T>(Codec codec);

        long GetUidByType(Type cl);

        Type GetTypeByUid(long uid);

        void RegisterTypeWithSerialUid(Type type);

        void WrapPacket(ProtocolBuffer source, StreamData dest);

        bool UnwrapPacket(StreamData source, ProtocolBuffer dest);

        ProtocolBuffer NewProtocolBuffer();

        void FreeProtocolBuffer(ProtocolBuffer protocolBuffer);
    }
}