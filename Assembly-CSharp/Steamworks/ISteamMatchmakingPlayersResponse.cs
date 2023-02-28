using System;
using System.Runtime.InteropServices;

namespace Steamworks {
    public class ISteamMatchmakingPlayersResponse {
        public delegate void AddPlayerToList(string pchName, int nScore, float flTimePlayed);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void InternalAddPlayerToList(IntPtr pchName, int nScore, float flTimePlayed);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void InternalPlayersFailedToRespond();

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void InternalPlayersRefreshComplete();

        public delegate void PlayersFailedToRespond();

        public delegate void PlayersRefreshComplete();

        readonly AddPlayerToList m_AddPlayerToList;

        GCHandle m_pGCHandle;

        readonly PlayersFailedToRespond m_PlayersFailedToRespond;

        readonly PlayersRefreshComplete m_PlayersRefreshComplete;

        readonly IntPtr m_pVTable;

        readonly VTable m_VTable;

        public ISteamMatchmakingPlayersResponse(AddPlayerToList onAddPlayerToList, PlayersFailedToRespond onPlayersFailedToRespond, PlayersRefreshComplete onPlayersRefreshComplete) {
            if (onAddPlayerToList == null || onPlayersFailedToRespond == null || onPlayersRefreshComplete == null) {
                throw new ArgumentNullException();
            }

            m_AddPlayerToList = onAddPlayerToList;
            m_PlayersFailedToRespond = onPlayersFailedToRespond;
            m_PlayersRefreshComplete = onPlayersRefreshComplete;

            m_VTable = new VTable {
                m_VTAddPlayerToList = InternalOnAddPlayerToList,
                m_VTPlayersFailedToRespond = InternalOnPlayersFailedToRespond,
                m_VTPlayersRefreshComplete = InternalOnPlayersRefreshComplete
            };

            m_pVTable = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VTable)));
            Marshal.StructureToPtr(m_VTable, m_pVTable, false);
            m_pGCHandle = GCHandle.Alloc(m_pVTable, GCHandleType.Pinned);
        }

        ~ISteamMatchmakingPlayersResponse() {
            if (m_pVTable != IntPtr.Zero) {
                Marshal.FreeHGlobal(m_pVTable);
            }

            if (m_pGCHandle.IsAllocated) {
                m_pGCHandle.Free();
            }
        }

        void InternalOnAddPlayerToList(IntPtr pchName, int nScore, float flTimePlayed) {
            m_AddPlayerToList(InteropHelp.PtrToStringUTF8(pchName), nScore, flTimePlayed);
        }

        void InternalOnPlayersFailedToRespond() {
            m_PlayersFailedToRespond();
        }

        void InternalOnPlayersRefreshComplete() {
            m_PlayersRefreshComplete();
        }

        public static explicit operator IntPtr(ISteamMatchmakingPlayersResponse that) => that.m_pGCHandle.AddrOfPinnedObject();

        [StructLayout(LayoutKind.Sequential)]
        class VTable {
            [NonSerialized] [MarshalAs(UnmanagedType.FunctionPtr)]
            public InternalAddPlayerToList m_VTAddPlayerToList;

            [NonSerialized] [MarshalAs(UnmanagedType.FunctionPtr)]
            public InternalPlayersFailedToRespond m_VTPlayersFailedToRespond;

            [NonSerialized] [MarshalAs(UnmanagedType.FunctionPtr)]
            public InternalPlayersRefreshComplete m_VTPlayersRefreshComplete;
        }
    }
}