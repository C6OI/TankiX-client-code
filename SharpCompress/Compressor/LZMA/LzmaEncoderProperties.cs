namespace SharpCompress.Compressor.LZMA {
    public class LzmaEncoderProperties {
        internal object[] properties;
        internal CoderPropID[] propIDs;

        public LzmaEncoderProperties()
            : this(false) { }

        public LzmaEncoderProperties(bool eos)
            : this(eos, 1048576) { }

        public LzmaEncoderProperties(bool eos, int dictionary)
            : this(eos, dictionary, 32) { }

        public LzmaEncoderProperties(bool eos, int dictionary, int numFastBytes) {
            int num = 2;
            int num2 = 3;
            int num3 = 0;
            int num4 = 2;
            string text = "bt4";

            propIDs = new CoderPropID[8] {
                CoderPropID.DictionarySize,
                CoderPropID.PosStateBits,
                CoderPropID.LitContextBits,
                CoderPropID.LitPosBits,
                CoderPropID.Algorithm,
                CoderPropID.NumFastBytes,
                CoderPropID.MatchFinder,
                CoderPropID.EndMarker
            };

            properties = new object[8] { dictionary, num, num2, num3, num4, numFastBytes, text, eos };
        }
    }
}