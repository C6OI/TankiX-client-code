using System;
using System.Collections;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientRemoteServer.Impl {
    public class MoveCommandCodec : AbstractMoveCodec {
        static readonly int WEAPON_ROTATION_SIZE = 2;

        static readonly int WEAPON_ROTATION_COMPONENT_BITSIZE = WEAPON_ROTATION_SIZE * 8;

        static readonly float WEAPON_ROTATION_FACTOR = 360f / (1 << WEAPON_ROTATION_COMPONENT_BITSIZE);

        static readonly byte[] bufferForWeaponRotation = new byte[WEAPON_ROTATION_SIZE];

        static readonly byte[] bufferEmpty = new byte[0];

        static readonly BitArray bitsForWeaponRotation = new(bufferForWeaponRotation);

        static readonly BitArray bitsEmpty = new(bufferEmpty);

        Codec movementCodec;

        public override void Init(Protocol protocol) => movementCodec = protocol.GetCodec(typeof(Movement));

        public override void Encode(ProtocolBuffer protocolBuffer, object data) {
            base.Encode(protocolBuffer, data);
            MoveCommand moveCommand = (MoveCommand)data;
            bool hasValue = moveCommand.Movement.HasValue;
            bool hasValue2 = moveCommand.WeaponRotation.HasValue;
            protocolBuffer.OptionalMap.Add(hasValue);
            protocolBuffer.OptionalMap.Add(hasValue2);
            protocolBuffer.Writer.Write(moveCommand.DiscreteTankControl.Control);

            if (hasValue) {
                movementCodec.Encode(protocolBuffer, moveCommand.Movement.Value);
            }

            if (hasValue2) {
                byte[] buffer = GetBuffer(hasValue2);
                BitArray bits = GetBits(hasValue2);
                int position = 0;

                WriteFloat(bits,
                    ref position,
                    moveCommand.WeaponRotation.Value,
                    WEAPON_ROTATION_COMPONENT_BITSIZE,
                    WEAPON_ROTATION_FACTOR);

                bits.CopyTo(buffer, 0);
                protocolBuffer.Writer.Write(buffer);

                if (position != bits.Length) {
                    throw new Exception("Move command pack mismatch");
                }
            }

            ushort value = (ushort)((long)(moveCommand.ClientTime * 1000.0) & 0xFFFF);
            protocolBuffer.Writer.Write(value);
        }

        public override object Decode(ProtocolBuffer protocolBuffer) {
            bool flag = protocolBuffer.OptionalMap.Get();
            bool flag2 = protocolBuffer.OptionalMap.Get();
            MoveCommand moveCommand = default;
            DiscreteTankControl discreteTankControl = default;
            discreteTankControl.Control = protocolBuffer.Reader.ReadByte();
            moveCommand.DiscreteTankControl = discreteTankControl;

            if (flag) {
                moveCommand.Movement = (Movement)movementCodec.Decode(protocolBuffer);
            }

            byte[] buffer = GetBuffer(flag2);
            BitArray bits = GetBits(flag2);
            int position = 0;
            protocolBuffer.Reader.Read(buffer, 0, buffer.Length);
            CopyBits(buffer, bits);

            if (flag2) {
                moveCommand.WeaponRotation =
                    ReadFloat(bits, ref position, WEAPON_ROTATION_COMPONENT_BITSIZE, WEAPON_ROTATION_FACTOR);
            }

            if (position != bits.Length) {
                throw new Exception("Move command unpack mismatch");
            }

            protocolBuffer.Reader.ReadInt16();
            return moveCommand;
        }

        BitArray GetBits(bool hasWeaponRotation) => !hasWeaponRotation ? bitsEmpty : bitsForWeaponRotation;

        byte[] GetBuffer(bool hasWeaponRotation) => !hasWeaponRotation ? bufferEmpty : bufferForWeaponRotation;
    }
}