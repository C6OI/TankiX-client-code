namespace Tanks.Battle.ClientGraphics.Impl {
    public class TrackRenderData {
        public float baseAlpha;

        public int currentPart;

        public int firstSectorToHide;

        public int lastSectorIndex;

        public int maxSectorCountPerTrack;

        public int parts;

        public int sectorCount;
        public TrackSector[] sectors;

        public float texturePart;

        public TrackRenderData(int maxSectorCountPerTrack, int firstSectorToHide, float baseAlpha, int parts) {
            this.maxSectorCountPerTrack = maxSectorCountPerTrack;
            this.firstSectorToHide = firstSectorToHide;
            this.baseAlpha = baseAlpha;
            this.parts = parts;
            texturePart = 1f / parts;
            sectors = new TrackSector[maxSectorCountPerTrack];
            Reset();
        }

        public void Reset() {
            lastSectorIndex = -1;
            sectorCount = 0;
            currentPart = 0;
        }
    }
}