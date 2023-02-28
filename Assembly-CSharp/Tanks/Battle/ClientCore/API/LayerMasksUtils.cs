namespace Tanks.Battle.ClientCore.API {
    public static class LayerMasksUtils {
        static void ValidateLayer(int layer) {
            if (layer < 0 || layer > 31) {
                throw new LayerMasksValidationException(string.Format("Invalid layer: {0}; Argument must be [0; 31]", layer));
            }
        }

        public static int CreateLayerMask(params int[] layers) => AddLayersToMask(0, layers);

        public static int AddLayerToMask(int layerMask, int layer) {
            ValidateLayer(layer);
            return layerMask | 1 << layer;
        }

        public static int AddLayersToMask(int layerMask, params int[] layers) {
            int num = layerMask;

            for (int i = 0; i < layers.Length; i++) {
                num = AddLayerToMask(num, layers[i]);
            }

            return num;
        }

        public static int RemoveLayerFromMask(int layerMask, int layer) {
            ValidateLayer(layer);
            return layerMask & ~(1 << layer);
        }

        public static int RemoveLayersFromMask(int layerMask, params int[] layers) {
            int num = layerMask;

            for (int i = 0; i < layers.Length; i++) {
                num = RemoveLayerFromMask(num, layers[i]);
            }

            return num;
        }
    }
}