using System;
using System.IO;
using SharpCompress.Compressor.LZMA.LZ;
using SharpCompress.Compressor.LZMA.RangeCoder;

namespace SharpCompress.Compressor.LZMA {
    internal class Decoder : ICoder, ISetDecoderProperties {
        int m_DictionarySize;

        readonly BitDecoder[] m_IsMatchDecoders = new BitDecoder[192];

        readonly BitDecoder[] m_IsRep0LongDecoders = new BitDecoder[192];

        readonly BitDecoder[] m_IsRepDecoders = new BitDecoder[12];

        readonly BitDecoder[] m_IsRepG0Decoders = new BitDecoder[12];

        readonly BitDecoder[] m_IsRepG1Decoders = new BitDecoder[12];

        readonly BitDecoder[] m_IsRepG2Decoders = new BitDecoder[12];

        readonly LenDecoder m_LenDecoder = new();

        readonly LiteralDecoder m_LiteralDecoder = new();

        OutWindow m_OutWindow;

        BitTreeDecoder m_PosAlignDecoder = new(4);

        readonly BitDecoder[] m_PosDecoders = new BitDecoder[114];

        readonly BitTreeDecoder[] m_PosSlotDecoder = new BitTreeDecoder[4];

        uint m_PosStateMask;

        readonly LenDecoder m_RepLenDecoder = new();

        uint rep0;

        uint rep1;

        uint rep2;

        uint rep3;

        Base.State state = default;

        public Decoder() {
            m_DictionarySize = -1;

            for (int i = 0; i < 4L; i++) {
                m_PosSlotDecoder[i] = new BitTreeDecoder(6);
            }
        }

        public void Code(Stream inStream, Stream outStream, long inSize, long outSize, ICodeProgress progress) {
            if (m_OutWindow == null) {
                CreateDictionary();
            }

            m_OutWindow.Init(outStream);

            if (outSize > 0) {
                m_OutWindow.SetLimit(outSize);
            } else {
                m_OutWindow.SetLimit(long.MaxValue - m_OutWindow.Total);
            }

            RangeCoder.Decoder decoder = new();
            decoder.Init(inStream);
            Code(m_DictionarySize, m_OutWindow, decoder);
            m_OutWindow.ReleaseStream();
            decoder.ReleaseStream();

            if (!decoder.IsFinished || inSize > 0 && decoder.Total != inSize) {
                throw new DataErrorException();
            }

            if (m_OutWindow.HasPending) {
                throw new DataErrorException();
            }

            m_OutWindow = null;
        }

        public void SetDecoderProperties(byte[] properties) {
            if (properties.Length < 1) {
                throw new InvalidParamException();
            }

            int lc = properties[0] % 9;
            int num = properties[0] / 9;
            int lp = num % 5;
            int num2 = num / 5;

            if (num2 > 4) {
                throw new InvalidParamException();
            }

            SetLiteralProperties(lp, lc);
            SetPosBitsProperties(num2);
            Init();

            if (properties.Length >= 5) {
                m_DictionarySize = 0;

                for (int i = 0; i < 4; i++) {
                    m_DictionarySize += properties[1 + i] << i * 8;
                }
            }
        }

        void CreateDictionary() {
            if (m_DictionarySize < 0) {
                throw new InvalidParamException();
            }

            m_OutWindow = new OutWindow();
            int windowSize = Math.Max(m_DictionarySize, 4096);
            m_OutWindow.Create(windowSize);
        }

        void SetLiteralProperties(int lp, int lc) {
            if (lp > 8) {
                throw new InvalidParamException();
            }

            if (lc > 8) {
                throw new InvalidParamException();
            }

            m_LiteralDecoder.Create(lp, lc);
        }

        void SetPosBitsProperties(int pb) {
            if (pb > 4) {
                throw new InvalidParamException();
            }

            uint num = (uint)(1 << pb);
            m_LenDecoder.Create(num);
            m_RepLenDecoder.Create(num);
            m_PosStateMask = num - 1;
        }

        void Init() {
            for (uint num = 0u; num < 12; num++) {
                for (uint num2 = 0u; num2 <= m_PosStateMask; num2++) {
                    uint num3 = (num << 4) + num2;
                    m_IsMatchDecoders[num3].Init();
                    m_IsRep0LongDecoders[num3].Init();
                }

                m_IsRepDecoders[num].Init();
                m_IsRepG0Decoders[num].Init();
                m_IsRepG1Decoders[num].Init();
                m_IsRepG2Decoders[num].Init();
            }

            m_LiteralDecoder.Init();

            for (uint num = 0u; num < 4; num++) {
                m_PosSlotDecoder[num].Init();
            }

            for (uint num = 0u; num < 114; num++) {
                m_PosDecoders[num].Init();
            }

            m_LenDecoder.Init();
            m_RepLenDecoder.Init();
            m_PosAlignDecoder.Init();
            state.Init();
            rep0 = 0u;
            rep1 = 0u;
            rep2 = 0u;
            rep3 = 0u;
        }

        internal bool Code(int dictionarySize, OutWindow outWindow, RangeCoder.Decoder rangeDecoder) {
            int num = Math.Max(dictionarySize, 1);
            outWindow.CopyPending();

            while (outWindow.HasSpace) {
                uint num2 = (uint)(int)outWindow.Total & m_PosStateMask;

                if (m_IsMatchDecoders[(state.Index << 4) + num2].Decode(rangeDecoder) == 0) {
                    byte @byte = outWindow.GetByte(0);

                    byte b = state.IsCharState() ? m_LiteralDecoder.DecodeNormal(rangeDecoder, (uint)outWindow.Total, @byte)
                                 : m_LiteralDecoder.DecodeWithMatchByte(rangeDecoder, (uint)outWindow.Total, @byte, outWindow.GetByte((int)rep0));

                    outWindow.PutByte(b);
                    state.UpdateChar();
                    continue;
                }

                uint len;

                if (m_IsRepDecoders[state.Index].Decode(rangeDecoder) == 1) {
                    if (m_IsRepG0Decoders[state.Index].Decode(rangeDecoder) == 0) {
                        if (m_IsRep0LongDecoders[(state.Index << 4) + num2].Decode(rangeDecoder) == 0) {
                            state.UpdateShortRep();
                            outWindow.PutByte(outWindow.GetByte((int)rep0));
                            continue;
                        }
                    } else {
                        uint num3;

                        if (m_IsRepG1Decoders[state.Index].Decode(rangeDecoder) == 0) {
                            num3 = rep1;
                        } else {
                            if (m_IsRepG2Decoders[state.Index].Decode(rangeDecoder) == 0) {
                                num3 = rep2;
                            } else {
                                num3 = rep3;
                                rep3 = rep2;
                            }

                            rep2 = rep1;
                        }

                        rep1 = rep0;
                        rep0 = num3;
                    }

                    len = m_RepLenDecoder.Decode(rangeDecoder, num2) + 2;
                    state.UpdateRep();
                } else {
                    rep3 = rep2;
                    rep2 = rep1;
                    rep1 = rep0;
                    len = 2 + m_LenDecoder.Decode(rangeDecoder, num2);
                    state.UpdateMatch();
                    uint num4 = m_PosSlotDecoder[Base.GetLenToPosState(len)].Decode(rangeDecoder);

                    if (num4 >= 4) {
                        int num5 = (int)((num4 >> 1) - 1);
                        rep0 = (2 | num4 & 1) << num5;

                        if (num4 < 14) {
                            rep0 += BitTreeDecoder.ReverseDecode(m_PosDecoders, rep0 - num4 - 1, rangeDecoder, num5);
                        } else {
                            rep0 += rangeDecoder.DecodeDirectBits(num5 - 4) << 4;
                            rep0 += m_PosAlignDecoder.ReverseDecode(rangeDecoder);
                        }
                    } else {
                        rep0 = num4;
                    }
                }

                if (rep0 >= outWindow.Total || rep0 >= num) {
                    if (rep0 == uint.MaxValue) {
                        return true;
                    }

                    throw new DataErrorException();
                }

                outWindow.CopyBlock((int)rep0, (int)len);
            }

            return false;
        }

        public void Train(Stream stream) {
            if (m_OutWindow == null) {
                CreateDictionary();
            }

            m_OutWindow.Train(stream);
        }

        class LenDecoder {
            BitDecoder m_Choice = default;

            BitDecoder m_Choice2 = default;

            BitTreeDecoder m_HighCoder = new(8);

            readonly BitTreeDecoder[] m_LowCoder = new BitTreeDecoder[16];

            readonly BitTreeDecoder[] m_MidCoder = new BitTreeDecoder[16];

            uint m_NumPosStates;

            public void Create(uint numPosStates) {
                for (uint num = m_NumPosStates; num < numPosStates; num++) {
                    m_LowCoder[num] = new BitTreeDecoder(3);
                    m_MidCoder[num] = new BitTreeDecoder(3);
                }

                m_NumPosStates = numPosStates;
            }

            public void Init() {
                m_Choice.Init();

                for (uint num = 0u; num < m_NumPosStates; num++) {
                    m_LowCoder[num].Init();
                    m_MidCoder[num].Init();
                }

                m_Choice2.Init();
                m_HighCoder.Init();
            }

            public uint Decode(RangeCoder.Decoder rangeDecoder, uint posState) {
                if (m_Choice.Decode(rangeDecoder) == 0) {
                    return m_LowCoder[posState].Decode(rangeDecoder);
                }

                uint num = 8u;

                if (m_Choice2.Decode(rangeDecoder) == 0) {
                    return num + m_MidCoder[posState].Decode(rangeDecoder);
                }

                num += 8;
                return num + m_HighCoder.Decode(rangeDecoder);
            }
        }

        class LiteralDecoder {
            Decoder2[] m_Coders;

            int m_NumPosBits;

            int m_NumPrevBits;

            uint m_PosMask;

            public void Create(int numPosBits, int numPrevBits) {
                if (m_Coders == null || m_NumPrevBits != numPrevBits || m_NumPosBits != numPosBits) {
                    m_NumPosBits = numPosBits;
                    m_PosMask = (uint)((1 << numPosBits) - 1);
                    m_NumPrevBits = numPrevBits;
                    uint num = (uint)(1 << m_NumPrevBits + m_NumPosBits);
                    m_Coders = new Decoder2[num];

                    for (uint num2 = 0u; num2 < num; num2++) {
                        m_Coders[num2].Create();
                    }
                }
            }

            public void Init() {
                uint num = (uint)(1 << m_NumPrevBits + m_NumPosBits);

                for (uint num2 = 0u; num2 < num; num2++) {
                    m_Coders[num2].Init();
                }
            }

            uint GetState(uint pos, byte prevByte) => ((pos & m_PosMask) << m_NumPrevBits) + (uint)(prevByte >> 8 - m_NumPrevBits);

            public byte DecodeNormal(RangeCoder.Decoder rangeDecoder, uint pos, byte prevByte) => m_Coders[GetState(pos, prevByte)].DecodeNormal(rangeDecoder);

            public byte DecodeWithMatchByte(RangeCoder.Decoder rangeDecoder, uint pos, byte prevByte, byte matchByte) =>
                m_Coders[GetState(pos, prevByte)].DecodeWithMatchByte(rangeDecoder, matchByte);

            struct Decoder2 {
                BitDecoder[] m_Decoders;

                public void Create() {
                    m_Decoders = new BitDecoder[768];
                }

                public void Init() {
                    for (int i = 0; i < 768; i++) {
                        m_Decoders[i].Init();
                    }
                }

                public byte DecodeNormal(RangeCoder.Decoder rangeDecoder) {
                    uint num = 1u;

                    do {
                        num = num << 1 | m_Decoders[num].Decode(rangeDecoder);
                    } while (num < 256);

                    return (byte)num;
                }

                public byte DecodeWithMatchByte(RangeCoder.Decoder rangeDecoder, byte matchByte) {
                    uint num = 1u;

                    do {
                        uint num2 = (uint)(matchByte >> 7) & 1u;
                        matchByte = (byte)(matchByte << 1);
                        uint num3 = m_Decoders[(1 + num2 << 8) + num].Decode(rangeDecoder);
                        num = num << 1 | num3;

                        if (num2 != num3) {
                            while (num < 256) {
                                num = num << 1 | m_Decoders[num].Decode(rangeDecoder);
                            }

                            break;
                        }
                    } while (num < 256);

                    return (byte)num;
                }
            }
        }
    }
}