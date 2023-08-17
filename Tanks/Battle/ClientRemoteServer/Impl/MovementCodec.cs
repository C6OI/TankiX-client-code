using System;
using System.Collections;
using Platform.Library.ClientProtocol.API;
using UnityEngine;

namespace Tanks.Battle.ClientRemoteServer.Impl {
    public class MovementCodec : AbstractMoveCodec {
        static readonly int MOVEMENT_SIZE = 21;

        static readonly int POSITION_COMPONENT_BITSIZE = 17;

        static readonly int ORIENTATION_COMPONENT_BITSIZE = 13;

        static readonly int LINEAR_VELOCITY_COMPONENT_BITSIZE = 13;

        static readonly int ANGULAR_VELOCITY_COMPONENT_BITSIZE = 13;

        static readonly float POSITION_FACTOR = 0.01f;

        static readonly float VELOCITY_FACTOR = 0.01f;

        static readonly float ANGULAR_VELOCITY_FACTOR = 0.005f;

        static readonly float ORIENTATION_PRECISION = 2f / (1 << ORIENTATION_COMPONENT_BITSIZE);

        public override void Init(Protocol protocol) { }

        public override void Encode(ProtocolBuffer protocolBuffer, object data) {
            base.Encode(protocolBuffer, data);
            Movement movement = (Movement)data;
            byte[] array = new byte[MOVEMENT_SIZE];
            BitArray bitArray = new(array);
            int position = 0;
            WriteVector3(bitArray, ref position, movement.Position, POSITION_COMPONENT_BITSIZE, POSITION_FACTOR);

            WriteQuaternion(bitArray,
                ref position,
                movement.Orientation,
                ORIENTATION_COMPONENT_BITSIZE,
                ORIENTATION_PRECISION);

            WriteVector3(bitArray, ref position, movement.Velocity, LINEAR_VELOCITY_COMPONENT_BITSIZE, VELOCITY_FACTOR);

            WriteVector3(bitArray,
                ref position,
                movement.AngularVelocity,
                ANGULAR_VELOCITY_COMPONENT_BITSIZE,
                ANGULAR_VELOCITY_FACTOR);

            bitArray.CopyTo(array, 0);
            protocolBuffer.Writer.Write(array);

            if (position != bitArray.Length) {
                throw new Exception("Movement pack mismatch");
            }
        }

        public override object Decode(ProtocolBuffer protocolBuffer) {
            Movement movement = default;
            byte[] array = new byte[MOVEMENT_SIZE];
            BitArray bitArray = new(array);
            int position = 0;
            protocolBuffer.Reader.Read(array, 0, array.Length);
            CopyBits(array, bitArray);
            movement.Position = ReadVector3(bitArray, ref position, POSITION_COMPONENT_BITSIZE, POSITION_FACTOR);

            movement.Orientation =
                ReadQuaternion(bitArray, ref position, ORIENTATION_COMPONENT_BITSIZE, ORIENTATION_PRECISION);

            movement.Velocity = ReadVector3(bitArray, ref position, LINEAR_VELOCITY_COMPONENT_BITSIZE, VELOCITY_FACTOR);

            movement.AngularVelocity = ReadVector3(bitArray,
                ref position,
                ANGULAR_VELOCITY_COMPONENT_BITSIZE,
                ANGULAR_VELOCITY_FACTOR);

            if (position != bitArray.Length) {
                throw new Exception("Movement unpack mismatch");
            }

            return movement;
        }

        static Vector3 ReadVector3(BitArray bits, ref int position, int size, float factor) {
            Vector3 result = default;
            result.x = ReadFloat(bits, ref position, size, factor);
            result.y = ReadFloat(bits, ref position, size, factor);
            result.z = ReadFloat(bits, ref position, size, factor);
            return result;
        }

        static Quaternion ReadQuaternion(BitArray bits, ref int position, int size, float factor) {
            Quaternion result = default;
            result.x = ReadFloat(bits, ref position, size, factor);
            result.y = ReadFloat(bits, ref position, size, factor);
            result.z = ReadFloat(bits, ref position, size, factor);
            result.w = Mathf.Sqrt(1f - (result.x * result.x + result.y * result.y + result.z * result.z));

            if (double.IsNaN(result.w)) {
                result.w = 0f;
            }

            return result;
        }

        static void WriteVector3(BitArray bits, ref int position, Vector3 value, int size, float factor) {
            WriteFloat(bits, ref position, value.x, size, factor);
            WriteFloat(bits, ref position, value.y, size, factor);
            WriteFloat(bits, ref position, value.z, size, factor);
        }

        static void WriteQuaternion(BitArray bits, ref int position, Quaternion value, int size, float factor) {
            int num = value.w >= 0f ? 1 : -1;
            WriteFloat(bits, ref position, value.x * num, size, factor);
            WriteFloat(bits, ref position, value.y * num, size, factor);
            WriteFloat(bits, ref position, value.z * num, size, factor);
        }
    }
}