using System.IO;

namespace SharpCompress.Compressor.BZip2 {
    internal class CBZip2InputStream : Stream {
        const int START_BLOCK_STATE = 1;

        const int RAND_PART_A_STATE = 2;

        const int RAND_PART_B_STATE = 3;

        const int RAND_PART_C_STATE = 4;

        const int NO_RAND_PART_A_STATE = 5;

        const int NO_RAND_PART_B_STATE = 6;

        const int NO_RAND_PART_C_STATE = 7;

        readonly int[][] basev = InitIntArray(6, 258);

        bool blockRandomised;

        int blockSize100k;

        int bsBuff;

        int bsLive;

        Stream bsStream;

        int ch2;

        int chPrev;

        int computedBlockCRC;

        int computedCombinedCRC;

        int count;

        int currentChar = -1;

        int currentState = 1;

        readonly bool decompressConcatenated;

        int i;

        int i2;

        readonly bool[] inUse = new bool[256];

        int j2;
        int last;

        bool leaveOpen;

        readonly int[][] limit = InitIntArray(6, 258);

        char[] ll8;

        readonly CRC mCrc = new();

        readonly int[] minLens = new int[6];

        int nInUse;

        int origPtr;

        readonly int[][] perm = InitIntArray(6, 258);

        int rNToGo;

        int rTPos;

        readonly char[] selector = new char[18002];

        readonly char[] selectorMtf = new char[18002];

        readonly char[] seqToUnseq = new char[256];

        int storedBlockCRC;

        int storedCombinedCRC;

        bool streamEnd;

        int tPos;

        int[] tt;

        readonly char[] unseqToSeq = new char[256];

        readonly int[] unzftab = new int[256];

        char z;

        public CBZip2InputStream(Stream zStream, bool decompressConcatenated, bool leaveOpen) {
            this.decompressConcatenated = decompressConcatenated;
            ll8 = null;
            tt = null;
            BsSetStream(zStream, leaveOpen);
            Initialize(true);
            InitBlock();
            SetupBlock();
        }

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override long Length => 0L;

        public override long Position {
            get => 0L;
            set { }
        }

        static void Cadvise() { }

        static void BadBGLengths() {
            Cadvise();
        }

        static void BitStreamEOF() {
            Cadvise();
        }

        static void CompressedStreamEOF() {
            Cadvise();
        }

        void MakeMaps() {
            nInUse = 0;

            for (int i = 0; i < 256; i++) {
                if (inUse[i]) {
                    seqToUnseq[nInUse] = (char)i;
                    unseqToSeq[i] = (char)nInUse;
                    nInUse++;
                }
            }
        }

        internal static int[][] InitIntArray(int n1, int n2) {
            int[][] array = new int[n1][];

            for (int i = 0; i < n1; i++) {
                array[i] = new int[n2];
            }

            return array;
        }

        internal static char[][] InitCharArray(int n1, int n2) {
            char[][] array = new char[n1][];

            for (int i = 0; i < n1; i++) {
                array[i] = new char[n2];
            }

            return array;
        }

        public override int ReadByte() {
            if (streamEnd) {
                return -1;
            }

            int result = currentChar;

            switch (currentState) {
                case 3:
                    SetupRandPartB();
                    break;

                case 4:
                    SetupRandPartC();
                    break;

                case 6:
                    SetupNoRandPartB();
                    break;

                case 7:
                    SetupNoRandPartC();
                    break;
            }

            return result;
        }

        bool Initialize(bool isFirstStream) {
            int num = bsStream.ReadByte();
            int num2 = bsStream.ReadByte();
            int num3 = bsStream.ReadByte();

            if (num == -1 && !isFirstStream) {
                return false;
            }

            if (num != 66 || num2 != 90 || num3 != 104) {
                throw new IOException("Not a BZIP2 marked stream");
            }

            int num4 = bsStream.ReadByte();

            if (num4 < 49 || num4 > 57) {
                BsFinishedWithStream();
                streamEnd = true;
                return false;
            }

            SetDecompressStructureSizes(num4 - 48);
            bsLive = 0;
            computedCombinedCRC = 0;
            return true;
        }

        void InitBlock() {
            char c;
            char c2;
            char c3;
            char c4;
            char c5;
            char c6;

            while (true) {
                c = BsGetUChar();
                c2 = BsGetUChar();
                c3 = BsGetUChar();
                c4 = BsGetUChar();
                c5 = BsGetUChar();
                c6 = BsGetUChar();

                if (c != '\u0017' || c2 != 'r' || c3 != 'E' || c4 != '8' || c5 != 'P' || c6 != '\u0090') {
                    break;
                }

                if (Complete()) {
                    return;
                }
            }

            if (c != '1' || c2 != 'A' || c3 != 'Y' || c4 != '&' || c5 != 'S' || c6 != 'Y') {
                BadBlockHeader();
                streamEnd = true;
                return;
            }

            storedBlockCRC = BsGetInt32();

            if (BsR(1) == 1) {
                blockRandomised = true;
            } else {
                blockRandomised = false;
            }

            GetAndMoveToFrontDecode();
            mCrc.InitialiseCRC();
            currentState = 1;
        }

        void EndBlock() {
            computedBlockCRC = mCrc.GetFinalCRC();

            if (storedBlockCRC != computedBlockCRC) {
                CrcError();
            }

            computedCombinedCRC = computedCombinedCRC << 1 | (int)((uint)computedCombinedCRC >> 31);
            computedCombinedCRC ^= computedBlockCRC;
        }

        bool Complete() {
            storedCombinedCRC = BsGetInt32();

            if (storedCombinedCRC != computedCombinedCRC) {
                CrcError();
            }

            bool flag = !decompressConcatenated || !Initialize(false);

            if (flag) {
                BsFinishedWithStream();
                streamEnd = true;
            }

            return flag;
        }

        static void BlockOverrun() {
            Cadvise();
        }

        static void BadBlockHeader() {
            Cadvise();
        }

        static void CrcError() {
            Cadvise();
        }

        void BsFinishedWithStream() {
            try {
                if (bsStream != null) {
                    if (!leaveOpen) {
                        bsStream.Dispose();
                    }

                    bsStream = null;
                }
            } catch { }
        }

        void BsSetStream(Stream f, bool leaveOpen) {
            bsStream = f;
            bsLive = 0;
            bsBuff = 0;
            this.leaveOpen = leaveOpen;
        }

        int BsR(int n) {
            while (bsLive < n) {
                int num = 0;

                try {
                    num = (ushort)bsStream.ReadByte();
                } catch (IOException) {
                    CompressedStreamEOF();
                }

                if (num == 65535) {
                    CompressedStreamEOF();
                }

                int num2 = num;
                bsBuff = bsBuff << 8 | num2 & 0xFF;
                bsLive += 8;
            }

            int result = bsBuff >> bsLive - n & (1 << n) - 1;
            bsLive -= n;
            return result;
        }

        char BsGetUChar() => (char)BsR(8);

        int BsGetint() {
            int num = 0;
            num = num << 8 | BsR(8);
            num = num << 8 | BsR(8);
            num = num << 8 | BsR(8);
            return num << 8 | BsR(8);
        }

        int BsGetIntVS(int numBits) => BsR(numBits);

        int BsGetInt32() => BsGetint();

        void HbCreateDecodeTables(int[] limit, int[] basev, int[] perm, char[] length, int minLen, int maxLen, int alphaSize) {
            int num = 0;

            for (int i = minLen; i <= maxLen; i++) {
                for (int j = 0; j < alphaSize; j++) {
                    if (length[j] == i) {
                        perm[num] = j;
                        num++;
                    }
                }
            }

            for (int i = 0; i < 23; i++) {
                basev[i] = 0;
            }

            for (int i = 0; i < alphaSize; i++) {
                basev[length[i] + 1]++;
            }

            for (int i = 1; i < 23; i++) {
                basev[i] += basev[i - 1];
            }

            for (int i = 0; i < 23; i++) {
                limit[i] = 0;
            }

            int num2 = 0;

            for (int i = minLen; i <= maxLen; i++) {
                num2 += basev[i + 1] - basev[i];
                limit[i] = num2 - 1;
                num2 <<= 1;
            }

            for (int i = minLen + 1; i <= maxLen; i++) {
                basev[i] = (limit[i - 1] + 1 << 1) - basev[i];
            }
        }

        void RecvDecodingTables() {
            char[][] array = InitCharArray(6, 258);
            bool[] array2 = new bool[16];

            for (int i = 0; i < 16; i++) {
                if (BsR(1) == 1) {
                    array2[i] = true;
                } else {
                    array2[i] = false;
                }
            }

            for (int i = 0; i < 256; i++) {
                inUse[i] = false;
            }

            for (int i = 0; i < 16; i++) {
                if (!array2[i]) {
                    continue;
                }

                for (int j = 0; j < 16; j++) {
                    if (BsR(1) == 1) {
                        inUse[i * 16 + j] = true;
                    }
                }
            }

            MakeMaps();
            int num = nInUse + 2;
            int num2 = BsR(3);
            int num3 = BsR(15);

            for (int i = 0; i < num3; i++) {
                int j = 0;

                while (BsR(1) == 1) {
                    j++;
                }

                selectorMtf[i] = (char)j;
            }

            char[] array3 = new char[6];

            for (char c = '\0'; c < num2; c = (char)(c + 1)) {
                array3[(uint)c] = c;
            }

            for (int i = 0; i < num3; i++) {
                char c = selectorMtf[i];
                char c2 = array3[(uint)c];

                while (c > '\0') {
                    array3[(uint)c] = array3[c - 1];
                    c = (char)(c - 1);
                }

                array3[0] = c2;
                selector[i] = c2;
            }

            for (int k = 0; k < num2; k++) {
                int num4 = BsR(5);

                for (int i = 0; i < num; i++) {
                    while (BsR(1) == 1) {
                        num4 = BsR(1) != 0 ? num4 - 1 : num4 + 1;
                    }

                    array[k][i] = (char)num4;
                }
            }

            for (int k = 0; k < num2; k++) {
                int num5 = 32;
                int num6 = 0;

                for (int i = 0; i < num; i++) {
                    if (array[k][i] > num6) {
                        num6 = array[k][i];
                    }

                    if (array[k][i] < num5) {
                        num5 = array[k][i];
                    }
                }

                HbCreateDecodeTables(limit[k], basev[k], perm[k], array[k], num5, num6, num);
                minLens[k] = num5;
            }
        }

        void GetAndMoveToFrontDecode() {
            //Discarded unreachable code: IL_04f8
            char[] array = new char[256];
            int num = 100000 * blockSize100k;
            origPtr = BsGetIntVS(24);
            RecvDecodingTables();
            int num2 = nInUse + 1;
            int num3 = -1;
            int num4 = 0;

            for (int i = 0; i <= 255; i++) {
                unzftab[i] = 0;
            }

            for (int i = 0; i <= 255; i++) {
                array[i] = (char)i;
            }

            last = -1;

            if (num4 == 0) {
                num3++;
                num4 = 50;
            }

            num4--;
            int num5 = selector[num3];
            int num6 = minLens[num5];
            int num7 = BsR(num6);

            while (num7 > limit[num5][num6]) {
                num6++;

                while (bsLive < 1) {
                    char c = '\0';

                    try {
                        c = (char)bsStream.ReadByte();
                    } catch (IOException) {
                        CompressedStreamEOF();
                    }

                    if (c == '\uffff') {
                        CompressedStreamEOF();
                    }

                    int num8 = c;
                    bsBuff = bsBuff << 8 | num8 & 0xFF;
                    bsLive += 8;
                }

                int num9 = bsBuff >> bsLive - 1 & 1;
                bsLive--;
                num7 = num7 << 1 | num9;
            }

            int num10 = perm[num5][num7 - basev[num5][num6]];

            while (num10 != num2) {
                if (num10 == 0 || num10 == 1) {
                    int num11 = -1;
                    int num12 = 1;

                    do {
                        switch (num10) {
                            case 0:
                                num11 += num12;
                                break;

                            case 1:
                                num11 += 2 * num12;
                                break;
                        }

                        num12 *= 2;

                        if (num4 == 0) {
                            num3++;
                            num4 = 50;
                        }

                        num4--;
                        int num13 = selector[num3];
                        int num14 = minLens[num13];
                        int num15 = BsR(num14);

                        while (num15 > limit[num13][num14]) {
                            num14++;

                            while (bsLive < 1) {
                                char c2 = '\0';

                                try {
                                    c2 = (char)bsStream.ReadByte();
                                } catch (IOException) {
                                    CompressedStreamEOF();
                                }

                                if (c2 == '\uffff') {
                                    CompressedStreamEOF();
                                }

                                int num16 = c2;
                                bsBuff = bsBuff << 8 | num16 & 0xFF;
                                bsLive += 8;
                            }

                            int num17 = bsBuff >> bsLive - 1 & 1;
                            bsLive--;
                            num15 = num15 << 1 | num17;
                        }

                        num10 = perm[num13][num15 - basev[num13][num14]];
                    } while (num10 == 0 || num10 == 1);

                    num11++;
                    char c3 = seqToUnseq[(uint)array[0]];
                    unzftab[(uint)c3] += num11;

                    while (num11 > 0) {
                        last++;
                        ll8[last] = c3;
                        num11--;
                    }

                    if (last >= num) {
                        BlockOverrun();
                    }

                    continue;
                }

                last++;

                if (last >= num) {
                    BlockOverrun();
                }

                char c4 = array[num10 - 1];
                unzftab[(uint)seqToUnseq[(uint)c4]]++;
                ll8[last] = seqToUnseq[(uint)c4];
                int num18;

                for (num18 = num10 - 1; num18 > 3; num18 -= 4) {
                    array[num18] = array[num18 - 1];
                    array[num18 - 1] = array[num18 - 2];
                    array[num18 - 2] = array[num18 - 3];
                    array[num18 - 3] = array[num18 - 4];
                }

                while (num18 > 0) {
                    array[num18] = array[num18 - 1];
                    num18--;
                }

                array[0] = c4;

                if (num4 == 0) {
                    num3++;
                    num4 = 50;
                }

                num4--;
                int num19 = selector[num3];
                int num20 = minLens[num19];
                int num21 = BsR(num20);

                while (num21 > limit[num19][num20]) {
                    num20++;

                    while (bsLive < 1) {
                        char c5 = '\0';

                        try {
                            c5 = (char)bsStream.ReadByte();
                        } catch (IOException) {
                            CompressedStreamEOF();
                        }

                        int num22 = c5;
                        bsBuff = bsBuff << 8 | num22 & 0xFF;
                        bsLive += 8;
                    }

                    int num23 = bsBuff >> bsLive - 1 & 1;
                    bsLive--;
                    num21 = num21 << 1 | num23;
                }

                num10 = perm[num19][num21 - basev[num19][num20]];
            }
        }

        void SetupBlock() {
            int[] array = new int[257];
            array[0] = 0;

            for (i = 1; i <= 256; i++) {
                array[i] = unzftab[i - 1];
            }

            for (i = 1; i <= 256; i++) {
                array[i] += array[i - 1];
            }

            for (i = 0; i <= last; i++) {
                char c = ll8[i];
                tt[array[(uint)c]] = i;
                array[(uint)c]++;
            }

            array = null;
            tPos = tt[origPtr];
            count = 0;
            i2 = 0;
            ch2 = 256;

            if (blockRandomised) {
                rNToGo = 0;
                rTPos = 0;
                SetupRandPartA();
            } else {
                SetupNoRandPartA();
            }
        }

        void SetupRandPartA() {
            if (i2 <= last) {
                chPrev = ch2;
                ch2 = ll8[tPos];
                tPos = tt[tPos];

                if (rNToGo == 0) {
                    rNToGo = BZip2Constants.rNums[rTPos];
                    rTPos++;

                    if (rTPos == 512) {
                        rTPos = 0;
                    }
                }

                rNToGo--;
                ch2 ^= rNToGo == 1 ? 1 : 0;
                i2++;
                currentChar = ch2;
                currentState = 3;
                mCrc.UpdateCRC(ch2);
            } else {
                EndBlock();
                InitBlock();
                SetupBlock();
            }
        }

        void SetupNoRandPartA() {
            if (i2 <= last) {
                chPrev = ch2;
                ch2 = ll8[tPos];
                tPos = tt[tPos];
                i2++;
                currentChar = ch2;
                currentState = 6;
                mCrc.UpdateCRC(ch2);
            } else {
                EndBlock();
                InitBlock();
                SetupBlock();
            }
        }

        void SetupRandPartB() {
            if (ch2 != chPrev) {
                currentState = 2;
                count = 1;
                SetupRandPartA();
                return;
            }

            count++;

            if (count >= 4) {
                z = ll8[tPos];
                tPos = tt[tPos];

                if (rNToGo == 0) {
                    rNToGo = BZip2Constants.rNums[rTPos];
                    rTPos++;

                    if (rTPos == 512) {
                        rTPos = 0;
                    }
                }

                rNToGo--;
                z ^= rNToGo == 1 ? '\u0001' : '\0';
                j2 = 0;
                currentState = 4;
                SetupRandPartC();
            } else {
                currentState = 2;
                SetupRandPartA();
            }
        }

        void SetupRandPartC() {
            if (j2 < z) {
                currentChar = ch2;
                mCrc.UpdateCRC(ch2);
                j2++;
            } else {
                currentState = 2;
                i2++;
                count = 0;
                SetupRandPartA();
            }
        }

        void SetupNoRandPartB() {
            if (ch2 != chPrev) {
                currentState = 5;
                count = 1;
                SetupNoRandPartA();
                return;
            }

            count++;

            if (count >= 4) {
                z = ll8[tPos];
                tPos = tt[tPos];
                currentState = 7;
                j2 = 0;
                SetupNoRandPartC();
            } else {
                currentState = 5;
                SetupNoRandPartA();
            }
        }

        void SetupNoRandPartC() {
            if (j2 < z) {
                currentChar = ch2;
                mCrc.UpdateCRC(ch2);
                j2++;
            } else {
                currentState = 5;
                i2++;
                count = 0;
                SetupNoRandPartA();
            }
        }

        void SetDecompressStructureSizes(int newSize100k) {
            if (0 > newSize100k || newSize100k > 9 || 0 > blockSize100k || blockSize100k > 9) { }

            blockSize100k = newSize100k;

            if (newSize100k != 0) {
                int num = 100000 * newSize100k;
                ll8 = new char[num];
                tt = new int[num];
            }
        }

        public override void Flush() { }

        public override int Read(byte[] buffer, int offset, int count) {
            int num = -1;
            int i;

            for (i = 0; i < count; i++) {
                num = ReadByte();

                if (num == -1) {
                    break;
                }

                buffer[i + offset] = (byte)num;
            }

            return i;
        }

        public override long Seek(long offset, SeekOrigin origin) => 0L;

        public override void SetLength(long value) { }

        public override void Write(byte[] buffer, int offset, int count) { }
    }
}