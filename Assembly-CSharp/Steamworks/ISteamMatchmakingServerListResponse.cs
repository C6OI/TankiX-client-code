using System;
using System.Runtime.InteropServices;

namespace Steamworks {
    public class ISteamMatchmakingServerListResponse {
        public delegate void RefreshComplete(HServerListRequest hRequest, EMatchMakingServerResponse response);

        public delegate void ServerFailedToRespond(HServerListRequest hRequest, int iServer);

        public delegate void ServerResponded(HServerListRequest hRequest, int iServer);

        GCHandle m_pGCHandle;

        readonly IntPtr m_pVTable;

        readonly RefreshComplete m_RefreshComplete;

        readonly ServerFailedToRespond m_ServerFailedToRespond;

        readonly ServerResponded m_ServerResponded;

        readonly VTable m_VTable;

        public ISteamMatchmakingServerListResponse(ServerResponded onServerResponded, ServerFailedToRespond onServerFailedToRespond, RefreshComplete onRefreshComplete) {
            if (onServerResponded == null || onServerFailedToRespond == null || onRefreshComplete == null) {
                throw new ArgumentNullException();
            }

            m_ServerResponded = onServerResponded;
            m_ServerFailedToRespond = onServerFailedToRespond;
            m_RefreshComplete = onRefreshComplete;

            m_VTable = new VTable {
                m_VTServerResponded = InternalOnServerResponded,
                m_VTServerFailedToRespond = InternalOnServerFailedToRespond,
                m_VTRefreshComplete = InternalOnRefreshComplete
            };

            m_pVTable = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VTable)));
            Marshal.StructureToPtr(m_VTable, m_pVTable, false);
            m_pGCHandle = GCHandle.Alloc(m_pVTable, GCHandleType.Pinned);
        }

        ~ISteamMatchmakingServerListResponse() {
            if (m_pVTable != IntPtr.Zero) {
                Marshal.FreeHGlobal(m_pVTable);
            }

            if (m_pGCHandle.IsAllocated) {
                m_pGCHandle.Free();
            }
        }

        void InternalOnServerResponded(HServerListRequest hRequest, int iServer) {
            m_ServerResponded(hRequest, iServer);
        }

        void InternalOnServerFailedToRespond(HServerListRequest hRequest, int iServer) {
            m_ServerFailedToRespond(hRequest, iServer);
        }

        void InternalOnRefreshComplete(HServerListRequest hRequest, EMatchMakingServerResponse response) {
            m_RefreshComplete(hRequest, response);
        }

        public static explicit operator IntPtr(ISteamMatchmakingServerListResponse that) => that.m_pGCHandle.AddrOfPinnedObject();

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        delegate void InternalServerResponded(HServerListRequest hRequest, int iServer);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        delegate void InternalServerFailedToRespond(HServerListRequest hRequest, int iServer);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        delegate void InternalRefreshComplete(HServerListRequest hRequest, EMatchMakingServerResponse response);

        [StructLayout(LayoutKind.Sequential)]
        class VTable {
            [NonSerialized] [MarshalAs(UnmanagedType.FunctionPtr)]
            public InternalRefreshComplete m_VTRefreshComplete;

            [NonSerialized] [MarshalAs(UnmanagedType.FunctionPtr)]
            public InternalServerFailedToRespond m_VTServerFailedToRespond;

            [NonSerialized] [MarshalAs(UnmanagedType.FunctionPtr)]
            public InternalServerResponded m_VTServerResponded;
        }
    }
}