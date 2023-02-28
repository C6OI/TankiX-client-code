namespace Platform.Library.ClientProtocol.API {
    public static class BufferUtils {
        public static byte[] GetBufferWithValidSize(byte[] input, int size) {
            int num = input.Length;

            if (size > num) {
                return new byte[size];
            }

            return input;
        }
    }
}