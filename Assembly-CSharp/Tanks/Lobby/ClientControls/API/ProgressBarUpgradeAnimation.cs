namespace Tanks.Lobby.ClientControls.API {
    public class ProgressBarUpgradeAnimation {
        public float additionalProgress;

        public float additionalProgress1;
        public float progress;

        public ProgressBarUpgradeAnimation(float progress, float additionalProgress, float additionalProgress1) {
            this.progress = progress;
            this.additionalProgress = additionalProgress;
            this.additionalProgress1 = additionalProgress1;
        }
    }
}