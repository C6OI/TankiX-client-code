using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientEntrance.API {
    public class UserMoneyBufferComponent : Component {
        public int CrystalBuffer { get; set; }

        public int XCrystalBuffer { get; set; }

        public void ChangeCrystalBufferBy(int delta) {
            CrystalBuffer += delta;
        }

        public void ChangeXCrystalBufferBy(int delta) {
            XCrystalBuffer += delta;
        }
    }
}