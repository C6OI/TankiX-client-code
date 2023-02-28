using System;

namespace SharpCompress.Compressor.Deflate {
    internal sealed class DeflateManager {
        const int MEM_LEVEL_MAX = 9;

        const int MEM_LEVEL_DEFAULT = 8;

        const int PRESET_DICT = 32;

        const int INIT_STATE = 42;

        const int BUSY_STATE = 113;

        const int FINISH_STATE = 666;

        const int Z_DEFLATED = 8;

        const int STORED_BLOCK = 0;

        const int STATIC_TREES = 1;

        const int DYN_TREES = 2;

        const int Z_BINARY = 0;

        const int Z_ASCII = 1;

        const int Z_UNKNOWN = 2;

        const int Buf_size = 16;

        const int MIN_MATCH = 3;

        const int MAX_MATCH = 258;

        const int MIN_LOOKAHEAD = 262;

        const int END_BLOCK = 256;

        internal static readonly int[] ExtraLengthBits = new int[29] {
            0, 0, 0, 0, 0, 0, 0, 0, 1, 1,
            1, 1, 2, 2, 2, 2, 3, 3, 3, 3,
            4, 4, 4, 4, 5, 5, 5, 5, 0
        };

        internal static readonly int[] ExtraDistanceBits = new int[30] {
            0, 0, 0, 0, 1, 1, 2, 2, 3, 3,
            4, 4, 5, 5, 6, 6, 7, 7, 8, 8,
            9, 9, 10, 10, 11, 11, 12, 12, 13, 13
        };

        static readonly string[] _ErrorMessage = new string[10] {
            "need dictionary",
            "stream end",
            string.Empty,
            "file error",
            "stream error",
            "data error",
            "insufficient memory",
            "buffer error",
            "incompatible version",
            string.Empty
        };

        static readonly int HEAP_SIZE = 2 * InternalConstants.L_CODES + 1;

        internal ZlibCodec _codec;

        internal int _distanceOffset;

        int _lengthOffset;

        internal short bi_buf;

        internal int bi_valid;

        readonly short[] bl_count = new short[InternalConstants.MAX_BITS + 1];

        readonly short[] bl_tree;

        int blockStart;

        CompressionLevel compressionLevel;

        CompressionStrategy compressionStrategy;

        Config config;

        internal sbyte data_type;

        CompressFunc DeflateFunction;

        readonly sbyte[] depth = new sbyte[2 * InternalConstants.L_CODES + 1];

        readonly short[] dyn_dtree;

        readonly short[] dyn_ltree;

        int hash_bits;

        int hash_mask;

        int hash_shift;

        int hash_size;

        short[] head;

        readonly int[] heap = new int[2 * InternalConstants.L_CODES + 1];

        int heap_len;

        int heap_max;

        int ins_h;

        internal int last_eob_len;

        internal int last_flush;

        internal int last_lit;

        internal int lit_bufsize;

        int lookahead;

        int match_available;

        int match_length;

        int match_start;

        internal int matches;

        internal int nextPending;

        internal int opt_len;

        internal byte[] pending;

        internal int pendingCount;

        internal short[] prev;

        int prev_length;

        int prev_match;

        bool Rfc1950BytesEmitted;

        internal int static_len;

        internal int status;

        int strstart;

        readonly Tree treeBitLengths = new();

        readonly Tree treeDistances = new();

        readonly Tree treeLiterals = new();

        internal int w_bits;

        internal int w_mask;

        internal int w_size;

        internal byte[] window;

        internal int window_size;

        internal DeflateManager() {
            dyn_ltree = new short[HEAP_SIZE * 2];
            dyn_dtree = new short[(2 * InternalConstants.D_CODES + 1) * 2];
            bl_tree = new short[(2 * InternalConstants.BL_CODES + 1) * 2];
        }

        internal bool WantRfc1950HeaderBytes { get; set; } = true;

        void _InitializeLazyMatch() {
            window_size = 2 * w_size;
            Array.Clear(head, 0, hash_size);
            config = Config.Lookup(compressionLevel);
            SetDeflater();
            strstart = 0;
            blockStart = 0;
            lookahead = 0;
            match_length = prev_length = 2;
            match_available = 0;
            ins_h = 0;
        }

        void _InitializeTreeData() {
            treeLiterals.dyn_tree = dyn_ltree;
            treeLiterals.staticTree = StaticTree.Literals;
            treeDistances.dyn_tree = dyn_dtree;
            treeDistances.staticTree = StaticTree.Distances;
            treeBitLengths.dyn_tree = bl_tree;
            treeBitLengths.staticTree = StaticTree.BitLengths;
            bi_buf = 0;
            bi_valid = 0;
            last_eob_len = 8;
            _InitializeBlocks();
        }

        internal void _InitializeBlocks() {
            for (int i = 0; i < InternalConstants.L_CODES; i++) {
                dyn_ltree[i * 2] = 0;
            }

            for (int j = 0; j < InternalConstants.D_CODES; j++) {
                dyn_dtree[j * 2] = 0;
            }

            for (int k = 0; k < InternalConstants.BL_CODES; k++) {
                bl_tree[k * 2] = 0;
            }

            dyn_ltree[512] = 1;
            opt_len = static_len = 0;
            last_lit = matches = 0;
        }

        internal void pqdownheap(short[] tree, int k) {
            int num = heap[k];

            for (int num2 = k << 1; num2 <= heap_len; num2 <<= 1) {
                if (num2 < heap_len && IsSmaller(tree, heap[num2 + 1], heap[num2], depth)) {
                    num2++;
                }

                if (IsSmaller(tree, num, heap[num2], depth)) {
                    break;
                }

                heap[k] = heap[num2];
                k = num2;
            }

            heap[k] = num;
        }

        internal static bool IsSmaller(short[] tree, int n, int m, sbyte[] depth) {
            short num = tree[n * 2];
            short num2 = tree[m * 2];
            return num < num2 || num == num2 && depth[n] <= depth[m];
        }

        internal void ScanTree(short[] tree, int maxCode) {
            int num = -1;
            int num2 = tree[1];
            int num3 = 0;
            int num4 = 7;
            int num5 = 4;

            if (num2 == 0) {
                num4 = 138;
                num5 = 3;
            }

            tree[(maxCode + 1) * 2 + 1] = short.MaxValue;

            for (int i = 0; i <= maxCode; i++) {
                int num6 = num2;
                num2 = tree[(i + 1) * 2 + 1];

                if (++num3 < num4 && num6 == num2) {
                    continue;
                }

                if (num3 < num5) {
                    bl_tree[num6 * 2] = (short)(bl_tree[num6 * 2] + num3);
                } else if (num6 != 0) {
                    if (num6 != num) {
                        bl_tree[num6 * 2]++;
                    }

                    bl_tree[InternalConstants.REP_3_6 * 2]++;
                } else if (num3 <= 10) {
                    bl_tree[InternalConstants.REPZ_3_10 * 2]++;
                } else {
                    bl_tree[InternalConstants.REPZ_11_138 * 2]++;
                }

                num3 = 0;
                num = num6;

                if (num2 == 0) {
                    num4 = 138;
                    num5 = 3;
                } else if (num6 == num2) {
                    num4 = 6;
                    num5 = 3;
                } else {
                    num4 = 7;
                    num5 = 4;
                }
            }
        }

        internal int BuildBlTree() {
            ScanTree(dyn_ltree, treeLiterals.max_code);
            ScanTree(dyn_dtree, treeDistances.max_code);
            treeBitLengths.build_tree(this);
            int num = InternalConstants.BL_CODES - 1;

            while (num >= 3 && bl_tree[Tree.bl_order[num] * 2 + 1] == 0) {
                num--;
            }

            opt_len += 3 * (num + 1) + 5 + 5 + 4;
            return num;
        }

        internal void send_all_trees(int lcodes, int dcodes, int blcodes) {
            send_bits(lcodes - 257, 5);
            send_bits(dcodes - 1, 5);
            send_bits(blcodes - 4, 4);

            for (int i = 0; i < blcodes; i++) {
                send_bits(bl_tree[Tree.bl_order[i] * 2 + 1], 3);
            }

            send_tree(dyn_ltree, lcodes - 1);
            send_tree(dyn_dtree, dcodes - 1);
        }

        internal void send_tree(short[] tree, int max_code) {
            int num = -1;
            int num2 = tree[1];
            int num3 = 0;
            int num4 = 7;
            int num5 = 4;

            if (num2 == 0) {
                num4 = 138;
                num5 = 3;
            }

            for (int i = 0; i <= max_code; i++) {
                int num6 = num2;
                num2 = tree[(i + 1) * 2 + 1];

                if (++num3 < num4 && num6 == num2) {
                    continue;
                }

                if (num3 < num5) {
                    do {
                        send_code(num6, bl_tree);
                    } while (--num3 != 0);
                } else if (num6 != 0) {
                    if (num6 != num) {
                        send_code(num6, bl_tree);
                        num3--;
                    }

                    send_code(InternalConstants.REP_3_6, bl_tree);
                    send_bits(num3 - 3, 2);
                } else if (num3 <= 10) {
                    send_code(InternalConstants.REPZ_3_10, bl_tree);
                    send_bits(num3 - 3, 3);
                } else {
                    send_code(InternalConstants.REPZ_11_138, bl_tree);
                    send_bits(num3 - 11, 7);
                }

                num3 = 0;
                num = num6;

                if (num2 == 0) {
                    num4 = 138;
                    num5 = 3;
                } else if (num6 == num2) {
                    num4 = 6;
                    num5 = 3;
                } else {
                    num4 = 7;
                    num5 = 4;
                }
            }
        }

        void put_bytes(byte[] p, int start, int len) {
            Array.Copy(p, start, pending, pendingCount, len);
            pendingCount += len;
        }

        internal void send_code(int c, short[] tree) {
            int num = c * 2;
            send_bits(tree[num] & 0xFFFF, tree[num + 1] & 0xFFFF);
        }

        internal void send_bits(int value, int length) {
            if (bi_valid > 16 - length) {
                bi_buf |= (short)(value << bi_valid & 0xFFFF);
                pending[pendingCount++] = (byte)bi_buf;
                pending[pendingCount++] = (byte)(bi_buf >> 8);
                bi_buf = (short)((uint)value >> 16 - bi_valid);
                bi_valid += length - 16;
            } else {
                bi_buf |= (short)(value << bi_valid & 0xFFFF);
                bi_valid += length;
            }
        }

        internal void _tr_align() {
            send_bits(2, 3);
            send_code(256, StaticTree.lengthAndLiteralsTreeCodes);
            bi_flush();

            if (1 + last_eob_len + 10 - bi_valid < 9) {
                send_bits(2, 3);
                send_code(256, StaticTree.lengthAndLiteralsTreeCodes);
                bi_flush();
            }

            last_eob_len = 7;
        }

        internal bool _tr_tally(int dist, int lc) {
            pending[_distanceOffset + last_lit * 2] = (byte)((uint)dist >> 8);
            pending[_distanceOffset + last_lit * 2 + 1] = (byte)dist;
            pending[_lengthOffset + last_lit] = (byte)lc;
            last_lit++;

            if (dist == 0) {
                dyn_ltree[lc * 2]++;
            } else {
                matches++;
                dist--;
                dyn_ltree[(Tree.LengthCode[lc] + InternalConstants.LITERALS + 1) * 2]++;
                dyn_dtree[Tree.DistanceCode(dist) * 2]++;
            }

            if ((last_lit & 0x1FFF) == 0 && compressionLevel > CompressionLevel.Level2) {
                int num = last_lit << 3;
                int num2 = strstart - blockStart;

                for (int i = 0; i < InternalConstants.D_CODES; i++) {
                    num = (int)(num + dyn_dtree[i * 2] * (5L + ExtraDistanceBits[i]));
                }

                num >>= 3;

                if (matches < last_lit / 2 && num < num2 / 2) {
                    return true;
                }
            }

            return last_lit == lit_bufsize - 1 || last_lit == lit_bufsize;
        }

        internal void send_compressed_block(short[] ltree, short[] dtree) {
            int num = 0;

            if (last_lit != 0) {
                do {
                    int num2 = _distanceOffset + num * 2;
                    int num3 = pending[num2] << 8 & 0xFF00 | pending[num2 + 1] & 0xFF;
                    int num4 = pending[_lengthOffset + num] & 0xFF;
                    num++;

                    if (num3 == 0) {
                        send_code(num4, ltree);
                        continue;
                    }

                    int num5 = Tree.LengthCode[num4];
                    send_code(num5 + InternalConstants.LITERALS + 1, ltree);
                    int num6 = ExtraLengthBits[num5];

                    if (num6 != 0) {
                        num4 -= Tree.LengthBase[num5];
                        send_bits(num4, num6);
                    }

                    num3--;
                    num5 = Tree.DistanceCode(num3);
                    send_code(num5, dtree);
                    num6 = ExtraDistanceBits[num5];

                    if (num6 != 0) {
                        num3 -= Tree.DistanceBase[num5];
                        send_bits(num3, num6);
                    }
                } while (num < last_lit);
            }

            send_code(256, ltree);
            last_eob_len = ltree[513];
        }

        internal void set_data_type() {
            int i = 0;
            int num = 0;
            int num2 = 0;

            for (; i < 7; i++) {
                num2 += dyn_ltree[i * 2];
            }

            for (; i < 128; i++) {
                num += dyn_ltree[i * 2];
            }

            for (; i < InternalConstants.LITERALS; i++) {
                num2 += dyn_ltree[i * 2];
            }

            data_type = (sbyte)(num2 <= num >> 2 ? 1 : 0);
        }

        internal void bi_flush() {
            if (bi_valid == 16) {
                pending[pendingCount++] = (byte)bi_buf;
                pending[pendingCount++] = (byte)(bi_buf >> 8);
                bi_buf = 0;
                bi_valid = 0;
            } else if (bi_valid >= 8) {
                pending[pendingCount++] = (byte)bi_buf;
                bi_buf >>= 8;
                bi_valid -= 8;
            }
        }

        internal void bi_windup() {
            if (bi_valid > 8) {
                pending[pendingCount++] = (byte)bi_buf;
                pending[pendingCount++] = (byte)(bi_buf >> 8);
            } else if (bi_valid > 0) {
                pending[pendingCount++] = (byte)bi_buf;
            }

            bi_buf = 0;
            bi_valid = 0;
        }

        internal void copy_block(int buf, int len, bool header) {
            bi_windup();
            last_eob_len = 8;

            if (header) {
                pending[pendingCount++] = (byte)len;
                pending[pendingCount++] = (byte)(len >> 8);
                pending[pendingCount++] = (byte)~len;
                pending[pendingCount++] = (byte)(~len >> 8);
            }

            put_bytes(window, buf, len);
        }

        internal void flush_block_only(bool eof) {
            _tr_flush_block(blockStart < 0 ? -1 : blockStart, strstart - blockStart, eof);
            blockStart = strstart;
            _codec.flush_pending();
        }

        internal BlockState DeflateNone(FlushType flush) {
            int num = 65535;

            if (num > pending.Length - 5) {
                num = pending.Length - 5;
            }

            while (true) {
                if (lookahead <= 1) {
                    _fillWindow();

                    if (lookahead == 0 && flush == FlushType.None) {
                        return BlockState.NeedMore;
                    }

                    if (lookahead == 0) {
                        break;
                    }
                }

                strstart += lookahead;
                lookahead = 0;
                int num2 = blockStart + num;

                if (strstart == 0 || strstart >= num2) {
                    lookahead = strstart - num2;
                    strstart = num2;
                    flush_block_only(false);

                    if (_codec.AvailableBytesOut == 0) {
                        return BlockState.NeedMore;
                    }
                }

                if (strstart - blockStart >= w_size - 262) {
                    flush_block_only(false);

                    if (_codec.AvailableBytesOut == 0) {
                        return BlockState.NeedMore;
                    }
                }
            }

            flush_block_only(flush == FlushType.Finish);

            if (_codec.AvailableBytesOut == 0) {
                return flush == FlushType.Finish ? BlockState.FinishStarted : BlockState.NeedMore;
            }

            return flush != FlushType.Finish ? BlockState.BlockDone : BlockState.FinishDone;
        }

        internal void _tr_stored_block(int buf, int stored_len, bool eof) {
            send_bits(eof ? 1 : 0, 3);
            copy_block(buf, stored_len, true);
        }

        internal void _tr_flush_block(int buf, int stored_len, bool eof) {
            int num = 0;
            int num2;
            int num3;

            if (compressionLevel > CompressionLevel.None) {
                if (data_type == 2) {
                    set_data_type();
                }

                treeLiterals.build_tree(this);
                treeDistances.build_tree(this);
                num = BuildBlTree();
                num2 = opt_len + 3 + 7 >> 3;
                num3 = static_len + 3 + 7 >> 3;

                if (num3 <= num2) {
                    num2 = num3;
                }
            } else {
                num2 = num3 = stored_len + 5;
            }

            if (stored_len + 4 <= num2 && buf != -1) {
                _tr_stored_block(buf, stored_len, eof);
            } else if (num3 == num2) {
                send_bits(2 + (eof ? 1 : 0), 3);
                send_compressed_block(StaticTree.lengthAndLiteralsTreeCodes, StaticTree.distTreeCodes);
            } else {
                send_bits(4 + (eof ? 1 : 0), 3);
                send_all_trees(treeLiterals.max_code + 1, treeDistances.max_code + 1, num + 1);
                send_compressed_block(dyn_ltree, dyn_dtree);
            }

            _InitializeBlocks();

            if (eof) {
                bi_windup();
            }
        }

        void _fillWindow() {
            do {
                int num = window_size - lookahead - strstart;
                int num2;

                if (num == 0 && strstart == 0 && lookahead == 0) {
                    num = w_size;
                } else if (num == -1) {
                    num--;
                } else if (strstart >= w_size + w_size - 262) {
                    Array.Copy(window, w_size, window, 0, w_size);
                    match_start -= w_size;
                    strstart -= w_size;
                    blockStart -= w_size;
                    num2 = hash_size;
                    int num3 = num2;

                    do {
                        int num4 = head[--num3] & 0xFFFF;
                        head[num3] = (short)(num4 >= w_size ? num4 - w_size : 0);
                    } while (--num2 != 0);

                    num2 = w_size;
                    num3 = num2;

                    do {
                        int num4 = prev[--num3] & 0xFFFF;
                        prev[num3] = (short)(num4 >= w_size ? num4 - w_size : 0);
                    } while (--num2 != 0);

                    num += w_size;
                }

                if (_codec.AvailableBytesIn == 0) {
                    break;
                }

                num2 = _codec.read_buf(window, strstart + lookahead, num);
                lookahead += num2;

                if (lookahead >= 3) {
                    ins_h = window[strstart] & 0xFF;
                    ins_h = (ins_h << hash_shift ^ window[strstart + 1] & 0xFF) & hash_mask;
                }
            } while (lookahead < 262 && _codec.AvailableBytesIn != 0);
        }

        internal BlockState DeflateFast(FlushType flush) {
            int num = 0;

            while (true) {
                if (lookahead < 262) {
                    _fillWindow();

                    if (lookahead < 262 && flush == FlushType.None) {
                        return BlockState.NeedMore;
                    }

                    if (lookahead == 0) {
                        break;
                    }
                }

                if (lookahead >= 3) {
                    ins_h = (ins_h << hash_shift ^ window[strstart + 2] & 0xFF) & hash_mask;
                    num = head[ins_h] & 0xFFFF;
                    prev[strstart & w_mask] = head[ins_h];
                    head[ins_h] = (short)strstart;
                }

                if ((long)num != 0 && (strstart - num & 0xFFFF) <= w_size - 262 && compressionStrategy != CompressionStrategy.HuffmanOnly) {
                    match_length = longest_match(num);
                }

                bool flag;

                if (match_length >= 3) {
                    flag = _tr_tally(strstart - match_start, match_length - 3);
                    lookahead -= match_length;

                    if (match_length <= config.MaxLazy && lookahead >= 3) {
                        match_length--;

                        do {
                            strstart++;
                            ins_h = (ins_h << hash_shift ^ window[strstart + 2] & 0xFF) & hash_mask;
                            num = head[ins_h] & 0xFFFF;
                            prev[strstart & w_mask] = head[ins_h];
                            head[ins_h] = (short)strstart;
                        } while (--match_length != 0);

                        strstart++;
                    } else {
                        strstart += match_length;
                        match_length = 0;
                        ins_h = window[strstart] & 0xFF;
                        ins_h = (ins_h << hash_shift ^ window[strstart + 1] & 0xFF) & hash_mask;
                    }
                } else {
                    flag = _tr_tally(0, window[strstart] & 0xFF);
                    lookahead--;
                    strstart++;
                }

                if (flag) {
                    flush_block_only(false);

                    if (_codec.AvailableBytesOut == 0) {
                        return BlockState.NeedMore;
                    }
                }
            }

            flush_block_only(flush == FlushType.Finish);

            if (_codec.AvailableBytesOut == 0) {
                if (flush == FlushType.Finish) {
                    return BlockState.FinishStarted;
                }

                return BlockState.NeedMore;
            }

            return flush != FlushType.Finish ? BlockState.BlockDone : BlockState.FinishDone;
        }

        internal BlockState DeflateSlow(FlushType flush) {
            int num = 0;

            while (true) {
                if (lookahead < 262) {
                    _fillWindow();

                    if (lookahead < 262 && flush == FlushType.None) {
                        return BlockState.NeedMore;
                    }

                    if (lookahead == 0) {
                        break;
                    }
                }

                if (lookahead >= 3) {
                    ins_h = (ins_h << hash_shift ^ window[strstart + 2] & 0xFF) & hash_mask;
                    num = head[ins_h] & 0xFFFF;
                    prev[strstart & w_mask] = head[ins_h];
                    head[ins_h] = (short)strstart;
                }

                prev_length = match_length;
                prev_match = match_start;
                match_length = 2;

                if (num != 0 && prev_length < config.MaxLazy && (strstart - num & 0xFFFF) <= w_size - 262) {
                    if (compressionStrategy != CompressionStrategy.HuffmanOnly) {
                        match_length = longest_match(num);
                    }

                    if (match_length <= 5 && (compressionStrategy == CompressionStrategy.Filtered || match_length == 3 && strstart - match_start > 4096)) {
                        match_length = 2;
                    }
                }

                if (prev_length >= 3 && match_length <= prev_length) {
                    int num2 = strstart + lookahead - 3;
                    bool flag = _tr_tally(strstart - 1 - prev_match, prev_length - 3);
                    lookahead -= prev_length - 1;
                    prev_length -= 2;

                    do {
                        if (++strstart <= num2) {
                            ins_h = (ins_h << hash_shift ^ window[strstart + 2] & 0xFF) & hash_mask;
                            num = head[ins_h] & 0xFFFF;
                            prev[strstart & w_mask] = head[ins_h];
                            head[ins_h] = (short)strstart;
                        }
                    } while (--prev_length != 0);

                    match_available = 0;
                    match_length = 2;
                    strstart++;

                    if (flag) {
                        flush_block_only(false);

                        if (_codec.AvailableBytesOut == 0) {
                            return BlockState.NeedMore;
                        }
                    }
                } else if (match_available != 0) {
                    if (_tr_tally(0, window[strstart - 1] & 0xFF)) {
                        flush_block_only(false);
                    }

                    strstart++;
                    lookahead--;

                    if (_codec.AvailableBytesOut == 0) {
                        return BlockState.NeedMore;
                    }
                } else {
                    match_available = 1;
                    strstart++;
                    lookahead--;
                }
            }

            if (match_available != 0) {
                bool flag = _tr_tally(0, window[strstart - 1] & 0xFF);
                match_available = 0;
            }

            flush_block_only(flush == FlushType.Finish);

            if (_codec.AvailableBytesOut == 0) {
                if (flush == FlushType.Finish) {
                    return BlockState.FinishStarted;
                }

                return BlockState.NeedMore;
            }

            return flush != FlushType.Finish ? BlockState.BlockDone : BlockState.FinishDone;
        }

        internal int longest_match(int cur_match) {
            int num = config.MaxChainLength;
            int num2 = strstart;
            int num3 = prev_length;
            int num4 = strstart > w_size - 262 ? strstart - (w_size - 262) : 0;
            int niceLength = config.NiceLength;
            int num5 = w_mask;
            int num6 = strstart + 258;
            byte b = window[num2 + num3 - 1];
            byte b2 = window[num2 + num3];

            if (prev_length >= config.GoodLength) {
                num >>= 2;
            }

            if (niceLength > lookahead) {
                niceLength = lookahead;
            }

            do {
                int num7 = cur_match;

                if (window[num7 + num3] != b2 || window[num7 + num3 - 1] != b || window[num7] != window[num2] || window[++num7] != window[num2 + 1]) {
                    continue;
                }

                num2 += 2;
                num7++;

                while (window[++num2] == window[++num7] && window[++num2] == window[++num7] && window[++num2] == window[++num7] && window[++num2] == window[++num7] &&
                       window[++num2] == window[++num7] && window[++num2] == window[++num7] && window[++num2] == window[++num7] && window[++num2] == window[++num7] && num2 < num6) { }

                int num8 = 258 - (num6 - num2);
                num2 = num6 - 258;

                if (num8 > num3) {
                    match_start = cur_match;
                    num3 = num8;

                    if (num8 >= niceLength) {
                        break;
                    }

                    b = window[num2 + num3 - 1];
                    b2 = window[num2 + num3];
                }
            } while ((cur_match = prev[cur_match & num5] & 0xFFFF) > num4 && --num != 0);

            if (num3 <= lookahead) {
                return num3;
            }

            return lookahead;
        }

        internal int Initialize(ZlibCodec codec, CompressionLevel level) => Initialize(codec, level, 15);

        internal int Initialize(ZlibCodec codec, CompressionLevel level, int bits) => Initialize(codec, level, bits, 8, CompressionStrategy.Default);

        internal int Initialize(ZlibCodec codec, CompressionLevel level, int bits, CompressionStrategy compressionStrategy) => Initialize(codec, level, bits, 8, compressionStrategy);

        internal int Initialize(ZlibCodec codec, CompressionLevel level, int windowBits, int memLevel, CompressionStrategy strategy) {
            _codec = codec;
            _codec.Message = null;

            if (windowBits < 9 || windowBits > 15) {
                throw new ZlibException("windowBits must be in the range 9..15.");
            }

            if (memLevel < 1 || memLevel > 9) {
                throw new ZlibException(string.Format("memLevel must be in the range 1.. {0}", 9));
            }

            _codec.dstate = this;
            w_bits = windowBits;
            w_size = 1 << w_bits;
            w_mask = w_size - 1;
            hash_bits = memLevel + 7;
            hash_size = 1 << hash_bits;
            hash_mask = hash_size - 1;
            hash_shift = (hash_bits + 3 - 1) / 3;
            window = new byte[w_size * 2];
            prev = new short[w_size];
            head = new short[hash_size];
            lit_bufsize = 1 << memLevel + 6;
            pending = new byte[lit_bufsize * 4];
            _distanceOffset = lit_bufsize;
            _lengthOffset = 3 * lit_bufsize;
            compressionLevel = level;
            compressionStrategy = strategy;
            Reset();
            return 0;
        }

        internal void Reset() {
            _codec.TotalBytesIn = _codec.TotalBytesOut = 0L;
            _codec.Message = null;
            pendingCount = 0;
            nextPending = 0;
            Rfc1950BytesEmitted = false;
            status = !WantRfc1950HeaderBytes ? 113 : 42;
            _codec._Adler32 = Adler.Adler32(0u, null, 0, 0);
            last_flush = 0;
            _InitializeTreeData();
            _InitializeLazyMatch();
        }

        internal int End() {
            if (status != 42 && status != 113 && status != 666) {
                return -2;
            }

            pending = null;
            head = null;
            prev = null;
            window = null;
            return status == 113 ? -3 : 0;
        }

        void SetDeflater() {
            switch (config.Flavor) {
                case DeflateFlavor.Store:
                    DeflateFunction = DeflateNone;
                    break;

                case DeflateFlavor.Fast:
                    DeflateFunction = DeflateFast;
                    break;

                case DeflateFlavor.Slow:
                    DeflateFunction = DeflateSlow;
                    break;
            }
        }

        internal int SetParams(CompressionLevel level, CompressionStrategy strategy) {
            int result = 0;

            if (compressionLevel != level) {
                Config config = Config.Lookup(level);

                if (config.Flavor != this.config.Flavor && _codec.TotalBytesIn != 0) {
                    result = _codec.Deflate(FlushType.Partial);
                }

                compressionLevel = level;
                this.config = config;
                SetDeflater();
            }

            compressionStrategy = strategy;
            return result;
        }

        internal int SetDictionary(byte[] dictionary) {
            int num = dictionary.Length;
            int sourceIndex = 0;

            if (dictionary == null || status != 42) {
                throw new ZlibException("Stream error.");
            }

            _codec._Adler32 = Adler.Adler32(_codec._Adler32, dictionary, 0, dictionary.Length);

            if (num < 3) {
                return 0;
            }

            if (num > w_size - 262) {
                num = w_size - 262;
                sourceIndex = dictionary.Length - num;
            }

            Array.Copy(dictionary, sourceIndex, window, 0, num);
            strstart = num;
            blockStart = num;
            ins_h = window[0] & 0xFF;
            ins_h = (ins_h << hash_shift ^ window[1] & 0xFF) & hash_mask;

            for (int i = 0; i <= num - 3; i++) {
                ins_h = (ins_h << hash_shift ^ window[i + 2] & 0xFF) & hash_mask;
                prev[i & w_mask] = head[ins_h];
                head[ins_h] = (short)i;
            }

            return 0;
        }

        internal int Deflate(FlushType flush) {
            if (_codec.OutputBuffer == null || _codec.InputBuffer == null && _codec.AvailableBytesIn != 0 || status == 666 && flush != FlushType.Finish) {
                _codec.Message = _ErrorMessage[4];
                throw new ZlibException(string.Format("Something is fishy. [{0}]", _codec.Message));
            }

            if (_codec.AvailableBytesOut == 0) {
                _codec.Message = _ErrorMessage[7];
                throw new ZlibException("OutputBuffer is full (AvailableBytesOut == 0)");
            }

            int num = last_flush;
            last_flush = (int)flush;

            if (status == 42) {
                int num2 = 8 + (w_bits - 8 << 4) << 8;
                int num3 = (int)(compressionLevel - 1 & (CompressionLevel)255) >> 1;

                if (num3 > 3) {
                    num3 = 3;
                }

                num2 |= num3 << 6;

                if (strstart != 0) {
                    num2 |= 0x20;
                }

                num2 += 31 - num2 % 31;
                status = 113;
                pending[pendingCount++] = (byte)(num2 >> 8);
                pending[pendingCount++] = (byte)num2;

                if (strstart != 0) {
                    pending[pendingCount++] = (byte)((_codec._Adler32 & 0xFF000000u) >> 24);
                    pending[pendingCount++] = (byte)((_codec._Adler32 & 0xFF0000) >> 16);
                    pending[pendingCount++] = (byte)((_codec._Adler32 & 0xFF00) >> 8);
                    pending[pendingCount++] = (byte)(_codec._Adler32 & 0xFFu);
                }

                _codec._Adler32 = Adler.Adler32(0u, null, 0, 0);
            }

            if (pendingCount != 0) {
                _codec.flush_pending();

                if (_codec.AvailableBytesOut == 0) {
                    last_flush = -1;
                    return 0;
                }
            } else if (_codec.AvailableBytesIn == 0 && (int)flush <= num && flush != FlushType.Finish) {
                return 0;
            }

            if (status == 666 && _codec.AvailableBytesIn != 0) {
                _codec.Message = _ErrorMessage[7];
                throw new ZlibException("status == FINISH_STATE && _codec.AvailableBytesIn != 0");
            }

            if (_codec.AvailableBytesIn != 0 || lookahead != 0 || flush != 0 && status != 666) {
                BlockState blockState = DeflateFunction(flush);

                if (blockState == BlockState.FinishStarted || blockState == BlockState.FinishDone) {
                    status = 666;
                }

                switch (blockState) {
                    case BlockState.NeedMore:
                    case BlockState.FinishStarted:
                        if (_codec.AvailableBytesOut == 0) {
                            last_flush = -1;
                        }

                        return 0;

                    case BlockState.BlockDone:
                        if (flush == FlushType.Partial) {
                            _tr_align();
                        } else {
                            _tr_stored_block(0, 0, false);

                            if (flush == FlushType.Full) {
                                for (int i = 0; i < hash_size; i++) {
                                    head[i] = 0;
                                }
                            }
                        }

                        _codec.flush_pending();

                        if (_codec.AvailableBytesOut == 0) {
                            last_flush = -1;
                            return 0;
                        }

                        break;
                }
            }

            if (flush != FlushType.Finish) {
                return 0;
            }

            if (!WantRfc1950HeaderBytes || Rfc1950BytesEmitted) {
                return 1;
            }

            pending[pendingCount++] = (byte)((_codec._Adler32 & 0xFF000000u) >> 24);
            pending[pendingCount++] = (byte)((_codec._Adler32 & 0xFF0000) >> 16);
            pending[pendingCount++] = (byte)((_codec._Adler32 & 0xFF00) >> 8);
            pending[pendingCount++] = (byte)(_codec._Adler32 & 0xFFu);
            _codec.flush_pending();
            Rfc1950BytesEmitted = true;
            return pendingCount == 0 ? 1 : 0;
        }

        internal enum BlockState {
            NeedMore = 0,
            BlockDone = 1,
            FinishStarted = 2,
            FinishDone = 3
        }

        internal enum DeflateFlavor {
            Store = 0,
            Fast = 1,
            Slow = 2
        }

        internal delegate BlockState CompressFunc(FlushType flush);

        internal class Config {
            static readonly Config[] Table;

            internal DeflateFlavor Flavor;
            internal int GoodLength;

            internal int MaxChainLength;

            internal int MaxLazy;

            internal int NiceLength;

            static Config() {
                Table = new Config[10] {
                    new(0, 0, 0, 0, DeflateFlavor.Store),
                    new(4, 4, 8, 4, DeflateFlavor.Fast),
                    new(4, 5, 16, 8, DeflateFlavor.Fast),
                    new(4, 6, 32, 32, DeflateFlavor.Fast),
                    new(4, 4, 16, 16, DeflateFlavor.Slow),
                    new(8, 16, 32, 32, DeflateFlavor.Slow),
                    new(8, 16, 128, 128, DeflateFlavor.Slow),
                    new(8, 32, 128, 256, DeflateFlavor.Slow),
                    new(32, 128, 258, 1024, DeflateFlavor.Slow),
                    new(32, 258, 258, 4096, DeflateFlavor.Slow)
                };
            }

            Config(int goodLength, int maxLazy, int niceLength, int maxChainLength, DeflateFlavor flavor) {
                GoodLength = goodLength;
                MaxLazy = maxLazy;
                NiceLength = niceLength;
                MaxChainLength = maxChainLength;
                Flavor = flavor;
            }

            public static Config Lookup(CompressionLevel level) => Table[(int)level];
        }

        sealed class Tree {
            internal const int Buf_size = 16;

            static readonly int HEAP_SIZE = 2 * InternalConstants.L_CODES + 1;

            internal static readonly sbyte[] bl_order = new sbyte[19] {
                16, 17, 18, 0, 8, 7, 9, 6, 10, 5,
                11, 4, 12, 3, 13, 2, 14, 1, 15
            };

            static readonly sbyte[] _dist_code = new sbyte[512] {
                0, 1, 2, 3, 4, 4, 5, 5, 6, 6,
                6, 6, 7, 7, 7, 7, 8, 8, 8, 8,
                8, 8, 8, 8, 9, 9, 9, 9, 9, 9,
                9, 9, 10, 10, 10, 10, 10, 10, 10, 10,
                10, 10, 10, 10, 10, 10, 10, 10, 11, 11,
                11, 11, 11, 11, 11, 11, 11, 11, 11, 11,
                11, 11, 11, 11, 12, 12, 12, 12, 12, 12,
                12, 12, 12, 12, 12, 12, 12, 12, 12, 12,
                12, 12, 12, 12, 12, 12, 12, 12, 12, 12,
                12, 12, 12, 12, 12, 12, 13, 13, 13, 13,
                13, 13, 13, 13, 13, 13, 13, 13, 13, 13,
                13, 13, 13, 13, 13, 13, 13, 13, 13, 13,
                13, 13, 13, 13, 13, 13, 13, 13, 14, 14,
                14, 14, 14, 14, 14, 14, 14, 14, 14, 14,
                14, 14, 14, 14, 14, 14, 14, 14, 14, 14,
                14, 14, 14, 14, 14, 14, 14, 14, 14, 14,
                14, 14, 14, 14, 14, 14, 14, 14, 14, 14,
                14, 14, 14, 14, 14, 14, 14, 14, 14, 14,
                14, 14, 14, 14, 14, 14, 14, 14, 14, 14,
                14, 14, 15, 15, 15, 15, 15, 15, 15, 15,
                15, 15, 15, 15, 15, 15, 15, 15, 15, 15,
                15, 15, 15, 15, 15, 15, 15, 15, 15, 15,
                15, 15, 15, 15, 15, 15, 15, 15, 15, 15,
                15, 15, 15, 15, 15, 15, 15, 15, 15, 15,
                15, 15, 15, 15, 15, 15, 15, 15, 15, 15,
                15, 15, 15, 15, 15, 15, 0, 0, 16, 17,
                18, 18, 19, 19, 20, 20, 20, 20, 21, 21,
                21, 21, 22, 22, 22, 22, 22, 22, 22, 22,
                23, 23, 23, 23, 23, 23, 23, 23, 24, 24,
                24, 24, 24, 24, 24, 24, 24, 24, 24, 24,
                24, 24, 24, 24, 25, 25, 25, 25, 25, 25,
                25, 25, 25, 25, 25, 25, 25, 25, 25, 25,
                26, 26, 26, 26, 26, 26, 26, 26, 26, 26,
                26, 26, 26, 26, 26, 26, 26, 26, 26, 26,
                26, 26, 26, 26, 26, 26, 26, 26, 26, 26,
                26, 26, 27, 27, 27, 27, 27, 27, 27, 27,
                27, 27, 27, 27, 27, 27, 27, 27, 27, 27,
                27, 27, 27, 27, 27, 27, 27, 27, 27, 27,
                27, 27, 27, 27, 28, 28, 28, 28, 28, 28,
                28, 28, 28, 28, 28, 28, 28, 28, 28, 28,
                28, 28, 28, 28, 28, 28, 28, 28, 28, 28,
                28, 28, 28, 28, 28, 28, 28, 28, 28, 28,
                28, 28, 28, 28, 28, 28, 28, 28, 28, 28,
                28, 28, 28, 28, 28, 28, 28, 28, 28, 28,
                28, 28, 28, 28, 28, 28, 28, 28, 29, 29,
                29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
                29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
                29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
                29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
                29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
                29, 29, 29, 29, 29, 29, 29, 29, 29, 29,
                29, 29
            };

            internal static readonly sbyte[] LengthCode = new sbyte[256] {
                0, 1, 2, 3, 4, 5, 6, 7, 8, 8,
                9, 9, 10, 10, 11, 11, 12, 12, 12, 12,
                13, 13, 13, 13, 14, 14, 14, 14, 15, 15,
                15, 15, 16, 16, 16, 16, 16, 16, 16, 16,
                17, 17, 17, 17, 17, 17, 17, 17, 18, 18,
                18, 18, 18, 18, 18, 18, 19, 19, 19, 19,
                19, 19, 19, 19, 20, 20, 20, 20, 20, 20,
                20, 20, 20, 20, 20, 20, 20, 20, 20, 20,
                21, 21, 21, 21, 21, 21, 21, 21, 21, 21,
                21, 21, 21, 21, 21, 21, 22, 22, 22, 22,
                22, 22, 22, 22, 22, 22, 22, 22, 22, 22,
                22, 22, 23, 23, 23, 23, 23, 23, 23, 23,
                23, 23, 23, 23, 23, 23, 23, 23, 24, 24,
                24, 24, 24, 24, 24, 24, 24, 24, 24, 24,
                24, 24, 24, 24, 24, 24, 24, 24, 24, 24,
                24, 24, 24, 24, 24, 24, 24, 24, 24, 24,
                25, 25, 25, 25, 25, 25, 25, 25, 25, 25,
                25, 25, 25, 25, 25, 25, 25, 25, 25, 25,
                25, 25, 25, 25, 25, 25, 25, 25, 25, 25,
                25, 25, 26, 26, 26, 26, 26, 26, 26, 26,
                26, 26, 26, 26, 26, 26, 26, 26, 26, 26,
                26, 26, 26, 26, 26, 26, 26, 26, 26, 26,
                26, 26, 26, 26, 27, 27, 27, 27, 27, 27,
                27, 27, 27, 27, 27, 27, 27, 27, 27, 27,
                27, 27, 27, 27, 27, 27, 27, 27, 27, 27,
                27, 27, 27, 27, 27, 28
            };

            internal static readonly int[] LengthBase = new int[29] {
                0, 1, 2, 3, 4, 5, 6, 7, 8, 10,
                12, 14, 16, 20, 24, 28, 32, 40, 48, 56,
                64, 80, 96, 112, 128, 160, 192, 224, 0
            };

            internal static readonly int[] DistanceBase = new int[30] {
                0, 1, 2, 3, 4, 6, 8, 12, 16, 24,
                32, 48, 64, 96, 128, 192, 256, 384, 512, 768,
                1024, 1536, 2048, 3072, 4096, 6144, 8192, 12288, 16384, 24576
            };

            internal short[] dyn_tree;

            internal int max_code;

            internal StaticTree staticTree;

            internal static int DistanceCode(int dist) => dist >= 256 ? _dist_code[256 + SharedUtils.URShift(dist, 7)] : _dist_code[dist];

            internal void gen_bitlen(DeflateManager s) {
                short[] array = dyn_tree;
                short[] treeCodes = staticTree.treeCodes;
                int[] extraBits = staticTree.extraBits;
                int extraBase = staticTree.extraBase;
                int maxLength = staticTree.maxLength;
                int num = 0;

                for (int i = 0; i <= InternalConstants.MAX_BITS; i++) {
                    s.bl_count[i] = 0;
                }

                array[s.heap[s.heap_max] * 2 + 1] = 0;
                int j;

                for (j = s.heap_max + 1; j < HEAP_SIZE; j++) {
                    int num2 = s.heap[j];
                    int i = array[array[num2 * 2 + 1] * 2 + 1] + 1;

                    if (i > maxLength) {
                        i = maxLength;
                        num++;
                    }

                    array[num2 * 2 + 1] = (short)i;

                    if (num2 <= max_code) {
                        s.bl_count[i]++;
                        int num3 = 0;

                        if (num2 >= extraBase) {
                            num3 = extraBits[num2 - extraBase];
                        }

                        short num4 = array[num2 * 2];
                        s.opt_len += num4 * (i + num3);

                        if (treeCodes != null) {
                            s.static_len += num4 * (treeCodes[num2 * 2 + 1] + num3);
                        }
                    }
                }

                if (num == 0) {
                    return;
                }

                do {
                    int i = maxLength - 1;

                    while (s.bl_count[i] == 0) {
                        i--;
                    }

                    s.bl_count[i]--;
                    s.bl_count[i + 1] = (short)(s.bl_count[i + 1] + 2);
                    s.bl_count[maxLength]--;
                    num -= 2;
                } while (num > 0);

                for (int i = maxLength; i != 0; i--) {
                    int num2 = s.bl_count[i];

                    while (num2 != 0) {
                        int num5 = s.heap[--j];

                        if (num5 <= max_code) {
                            if (array[num5 * 2 + 1] != i) {
                                s.opt_len = (int)(s.opt_len + (i - (long)array[num5 * 2 + 1]) * array[num5 * 2]);
                                array[num5 * 2 + 1] = (short)i;
                            }

                            num2--;
                        }
                    }
                }
            }

            internal void build_tree(DeflateManager s) {
                short[] array = dyn_tree;
                short[] treeCodes = staticTree.treeCodes;
                int elems = staticTree.elems;
                int num = -1;
                s.heap_len = 0;
                s.heap_max = HEAP_SIZE;

                for (int i = 0; i < elems; i++) {
                    if (array[i * 2] != 0) {
                        num = s.heap[++s.heap_len] = i;
                        s.depth[i] = 0;
                    } else {
                        array[i * 2 + 1] = 0;
                    }
                }

                int num2;

                while (s.heap_len < 2) {
                    num2 = s.heap[++s.heap_len] = num < 2 ? ++num : 0;
                    array[num2 * 2] = 1;
                    s.depth[num2] = 0;
                    s.opt_len--;

                    if (treeCodes != null) {
                        s.static_len -= treeCodes[num2 * 2 + 1];
                    }
                }

                max_code = num;

                for (int i = s.heap_len / 2; i >= 1; i--) {
                    s.pqdownheap(array, i);
                }

                num2 = elems;

                do {
                    int i = s.heap[1];
                    s.heap[1] = s.heap[s.heap_len--];
                    s.pqdownheap(array, 1);
                    int num3 = s.heap[1];
                    s.heap[--s.heap_max] = i;
                    s.heap[--s.heap_max] = num3;
                    array[num2 * 2] = (short)(array[i * 2] + array[num3 * 2]);
                    s.depth[num2] = (sbyte)(Math.Max((byte)s.depth[i], (byte)s.depth[num3]) + 1);
                    array[i * 2 + 1] = array[num3 * 2 + 1] = (short)num2;
                    s.heap[1] = num2++;
                    s.pqdownheap(array, 1);
                } while (s.heap_len >= 2);

                s.heap[--s.heap_max] = s.heap[1];
                gen_bitlen(s);
                gen_codes(array, num, s.bl_count);
            }

            internal static void gen_codes(short[] tree, int max_code, short[] bl_count) {
                short[] array = new short[InternalConstants.MAX_BITS + 1];
                short num = 0;

                for (int i = 1; i <= InternalConstants.MAX_BITS; i++) {
                    num = array[i] = (short)(num + bl_count[i - 1] << 1);
                }

                for (int j = 0; j <= max_code; j++) {
                    int num2 = tree[j * 2 + 1];

                    if (num2 != 0) {
                        tree[j * 2] = (short)bi_reverse(array[num2]++, num2);
                    }
                }
            }

            internal static int bi_reverse(int code, int len) {
                int num = 0;

                do {
                    num |= code & 1;
                    code >>= 1;
                    num <<= 1;
                } while (--len > 0);

                return num >> 1;
            }
        }
    }
}