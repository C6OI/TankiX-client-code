using UnityEngine;

namespace Tanks.Lobby.ClientControls.API {
    public class CursorsConfigurator : MonoBehaviour {
        public Texture2D regularCursor;

        public Vector2 regularCursorHotspot;

        public Texture2D handCursor;

        public Vector2 handCursorHotspot;

        public Texture2D inputCursor;

        public Vector2 inputCursorHotspot;

        void Awake() {
            Cursors.InitDefaultCursor(regularCursor, regularCursorHotspot);
            Cursors.InitCursor(CursorType.HAND, handCursor, handCursorHotspot);
            Cursors.InitCursor(CursorType.INPUT, inputCursor, inputCursorHotspot);
            Cursors.SwitchToDefaultCursor();
        }
    }
}