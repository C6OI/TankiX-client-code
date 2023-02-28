using System;
using System.Collections.Generic;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientProtocol.API;

namespace Platform.Library.ClientProtocol.Impl {
    public class ProtocolImpl : Protocol {
        readonly Dictionary<CodecInfo, Codec> codecRegistry;

        readonly List<CodecFactory> factories;

        readonly Dictionary<Type, Codec> hierarchicalCodecRegistry;

        readonly Dictionary<long, Type> uidByType;

        public ProtocolImpl() {
            codecRegistry = new Dictionary<CodecInfo, Codec>(new CodecInfoComparer());
            hierarchicalCodecRegistry = new Dictionary<Type, Codec>();
            uidByType = new Dictionary<long, Type>();
            factories = new List<CodecFactory>();
            RegisterFactory(new ArrayCodecFactory());
            RegisterFactory(new ListCodecFactory());
            RegisterFactory(new SetCodecFactory());
            RegisterFactory(new DictionaryCodecFactory());
            RegisterFactory(new EnumCodecFactory());
            RegisterFactory(new VariedCodecFactory());
            RegisterFactory(new OptionalTypeCodecFactory());
            RegisterFactory(new StructCodecFactory());
            RegisterCodecForType<bool>(new BooleanCodec());
            RegisterCodecForType<sbyte>(new SByteCodec());
            RegisterCodecForType<byte>(new ByteCodec());
            RegisterCodecForType<short>(new ShortCodec());
            RegisterCodecForType<int>(new IntegerCodec());
            RegisterCodecForType<long>(new LongCodec());
            RegisterCodecForType<float>(new FloatCodec());
            RegisterCodecForType<double>(new DoubleCodec());
            RegisterCodecForType<char>(new CharacterCodec());
            RegisterCodecForType<string>(new StringCodec());
            RegisterCodecForType<DateTime>(new DateTimeCodec());
        }

        [Inject] public static ClientProtocolInstancesCache ClientProtocolInstancesCache { get; set; }

        public int ServerProtocolVersion { get; set; }

        public Codec GetCodec(CodecInfoWithAttributes infoWithAttributes) {
            CodecInfo info = infoWithAttributes.Info;
            Codec value;

            if (codecRegistry.TryGetValue(info, out value)) {
                return value;
            }

            foreach (Type key in hierarchicalCodecRegistry.Keys) {
                if (key.IsAssignableFrom(info.type)) {
                    return hierarchicalCodecRegistry[key];
                }
            }

            return CreateCodecIfNecessary(infoWithAttributes);
        }

        public Codec GetCodec(Type type) {
            CodecInfo info = new(type, false, false);
            return GetCodec(info);
        }

        public void RegisterCodecForType<T>(Codec codec) {
            RegisterCodecForType(typeof(T), codec);
        }

        public void RegisterInheritanceCodecForType<T>(Codec codec) {
            hierarchicalCodecRegistry.Add(typeof(T), codec);
            RegisterCodecForType<T>(codec);
        }

        public long GetUidByType(Type cl) => SerializationUidUtils.GetUid(cl);

        public Type GetTypeByUid(long uid) {
            Type value;
            uidByType.TryGetValue(uid, out value);

            if (value == null) {
                throw new TypeByUidNotFoundException(uid);
            }

            return value;
        }

        public void RegisterTypeWithSerialUid(Type type) {
            long uid = SerializationUidUtils.GetUid(type);

            if (!uidByType.ContainsKey(uid)) {
                uidByType.Add(uid, type);
                return;
            }

            Type type2 = uidByType[uid];

            if (type2 == type) {
                return;
            }

            throw new TypeWithSameUidAlreadyRegisteredException(uid, type2, type);
        }

        public Codec GetCodec(long uid) {
            Type typeByUid = GetTypeByUid(uid);
            return GetCodec(typeByUid);
        }

        public void WrapPacket(ProtocolBuffer source, StreamData dest) {
            PacketHelper.WrapPacket(source, dest);
        }

        public bool UnwrapPacket(StreamData source, ProtocolBuffer dest) => PacketHelper.UnwrapPacket(source, dest);

        public ProtocolBuffer NewProtocolBuffer() => ClientProtocolInstancesCache.GetProtocolBufferInstance();

        public void FreeProtocolBuffer(ProtocolBuffer protocolBuffer) {
            ClientProtocolInstancesCache.ReleaseProtocolBufferInstance(protocolBuffer);
        }

        Codec GetCodec(CodecInfo info) {
            Codec value;

            if (codecRegistry.TryGetValue(info, out value)) {
                return value;
            }

            foreach (Type key in hierarchicalCodecRegistry.Keys) {
                if (key.IsAssignableFrom(info.type)) {
                    return hierarchicalCodecRegistry[key];
                }
            }

            CodecInfoWithAttributes codecInfoWithAttributes = new(info);
            return CreateCodecIfNecessary(codecInfoWithAttributes);
        }

        Codec CreateCodecIfNecessary(CodecInfoWithAttributes codecInfoWithAttributes) {
            CodecInfo info = codecInfoWithAttributes.Info;
            Codec value;

            if (ReflectionUtils.IsNullableType(info.type)) {
                Type nullableInnerType = ReflectionUtils.GetNullableInnerType(info.type);
                CodecInfo key = new(nullableInnerType, info.optional, info.varied);

                if (codecRegistry.TryGetValue(key, out value)) {
                    return value;
                }
            }

            for (int i = 0; i < factories.Count; i++) {
                CodecFactory codecFactory = factories[i];
                value = codecFactory.CreateCodec(this, codecInfoWithAttributes);

                if (value != null) {
                    if (info.optional) {
                        value = new OptionalCodec(value);
                    }

                    RegisterCodec(info, value);
                    return value;
                }
            }

            for (Type baseType = info.type.BaseType; baseType != null; baseType = baseType.BaseType) {
                CodecInfo key2 = new(baseType, info.optional, info.varied);

                if (codecRegistry.TryGetValue(key2, out value)) {
                    codecRegistry.Add(info, value);
                    return value;
                }
            }

            throw new CodecNotFoundForRequestException(info);
        }

        protected virtual void RegisterFactory(CodecFactory factory) {
            factories.Add(factory);
        }

        public void RegisterCodecForType(Type type, Codec codec) {
            RegisterCodec(new CodecInfo(type, false, false), codec);
            RegisterCodec(new CodecInfo(type, true, false), new OptionalCodec(codec));
        }

        void RegisterCodec(CodecInfo info, Codec codec) {
            codecRegistry.Add(info, codec);
            codec.Init(this);
        }

        class CodecInfoComparer : IEqualityComparer<CodecInfo> {
            public bool Equals(CodecInfo x, CodecInfo y) => x.Equals(y);

            public int GetHashCode(CodecInfo obj) => obj.GetHashCode();
        }
    }
}