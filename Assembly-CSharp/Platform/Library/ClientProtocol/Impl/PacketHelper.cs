using System;
using Platform.Library.ClientProtocol.API;

namespace Platform.Library.ClientProtocol.Impl {
    public class PacketHelper {
        const int PACKET_HELPER_BUFFER_SIZE = 512;

        static readonly OptionalMap packetHelperOptionalMap = new(null, 0);

        static byte[] packetHelperBuffer = new byte[512];

        public static void WrapPacket(ProtocolBuffer source, StreamData dest) {
            BigEndianBinaryWriter writer = dest.Writer;
            writer.Write(byte.MaxValue);
            writer.Write((byte)0);
            OptionalMap optionalMap = (OptionalMap)source.OptionalMap;
            writer.Write(optionalMap.GetSize());
            writer.Write((int)source.Data.Length);
            byte[] map = optionalMap.GetMap();
            int sizeInBytes = GetSizeInBytes(optionalMap.GetSize());

            for (int i = 0; i < sizeInBytes; i++) {
                writer.Write(map[i]);
            }

            source.Data.CastedStream.WriteTo(dest.Stream);
        }

        public static bool UnwrapPacket(StreamData source, ProtocolBuffer dest) {
            BigEndianBinaryReader reader = source.Reader;
            long num = source.Length - source.Position;
            long position = source.Position;

            if (num < 10) {
                return false;
            }

            byte b = reader.ReadByte();

            if (b != byte.MaxValue) {
                throw new CorruptBufferException();
            }

            if (reader.ReadByte() != 0) {
                throw new CorruptBufferException();
            }

            int size = reader.ReadInt32();
            int num2 = reader.ReadInt32();
            int sizeInBytes = GetSizeInBytes(size);

            if (num < sizeInBytes + num2 + 10) {
                source.Position = position;
                return false;
            }

            UpdatePackeHelperBuffer(sizeInBytes);
            source.Read(packetHelperBuffer, 0, sizeInBytes);
            packetHelperOptionalMap.Fill(packetHelperBuffer, size);
            dest.OptionalMap.Concat(packetHelperOptionalMap);
            UpdatePackeHelperBuffer(num2);
            source.Read(packetHelperBuffer, 0, num2);
            dest.Data.Write(packetHelperBuffer, 0, num2);
            dest.Flip();
            return true;
        }

        static void UpdatePackeHelperBuffer(int size) {
            packetHelperBuffer = BufferUtils.GetBufferWithValidSize(packetHelperBuffer, size);
        }

        static int GetSizeInBytes(int size) => (int)Math.Ceiling(size / 8.0);
    }
}