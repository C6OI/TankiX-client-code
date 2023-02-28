using System.Collections.Generic;
using Platform.Library.ClientLogger.API;
using UnityEngine;

namespace Tanks.Lobby.ClientControls.API {
    public static class Cursors {
        static readonly Dictionary<CursorType, CursorData> type2Data = new();

        static CursorData defaultCursorData;

        public static void InitCursor(CursorType type, Texture2D cursorTexture, Vector2 cursorHotspot) {
            if (cursorTexture == null) {
                LoggerProvider.GetLogger(typeof(Cursors)).ErrorFormat("CursorService:InitCursor argument 'cursorTexture' is null, argument 'type' is {0}", type);
            } else {
                type2Data.Add(type, new CursorData(cursorTexture, cursorHotspot));
            }
        }

        public static void InitDefaultCursor(Texture2D cursorTexture, Vector2 cursorHotspot) {
            if (cursorTexture == null) {
                LoggerProvider.GetLogger(typeof(Cursors)).Error("CursorService:InitDefaultCursor argument 'cursorTexture' is null");
                defaultCursorData.hotspot = Vector2.zero;
                defaultCursorData.texture = null;
            } else {
                defaultCursorData.hotspot = cursorHotspot;
                defaultCursorData.texture = cursorTexture;
            }
        }

        public static void SwitchToCursor(CursorType type) {
            CursorData value;

            if (type2Data.TryGetValue(type, out value)) {
                Cursor.SetCursor(value.texture, value.hotspot, CursorMode.Auto);
            }
        }

        public static void SwitchToDefaultCursor() {
            Cursor.SetCursor(defaultCursorData.texture, defaultCursorData.hotspot, CursorMode.Auto);
        }

        public static void Clean() {
            type2Data.Clear();
            defaultCursorData.texture = null;
            defaultCursorData.hotspot = Vector2.zero;
        }

        struct CursorData {
            public Texture2D texture;

            public Vector2 hotspot;

            public CursorData(Texture2D texture, Vector2 hotspot) {
                this.texture = texture;
                this.hotspot = hotspot;
            }
        }
    }
}