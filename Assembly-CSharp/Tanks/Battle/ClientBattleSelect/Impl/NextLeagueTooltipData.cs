namespace Tanks.Battle.ClientBattleSelect.Impl {
    public class NextLeagueTooltipData {
        public int delta;

        public string icon;

        public string name;
        public double points;

        public string unfairMM;

        public NextLeagueTooltipData(double points, string icon, string name, int delta, string unfairMM) {
            this.name = name;
            this.points = points;
            this.icon = icon;
            this.delta = delta;
            this.unfairMM = unfairMM;
        }
    }
}