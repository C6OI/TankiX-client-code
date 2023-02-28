namespace Tanks.Battle.ClientGraphics.Impl {
    public class TrackSegment {
        public TrackPoint a;

        public TrackPoint b;

        public float length;

        public TrackSegment(TrackPoint a, TrackPoint b, float length) {
            this.a = a;
            this.b = b;
            this.length = length;
        }
    }
}