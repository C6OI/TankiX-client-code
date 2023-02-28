using System;
using System.Runtime.InteropServices;

namespace Steamworks {
    public class ISteamMatchmakingPingResponse {
        public delegate void ServerFailedToRespond();

        public delegate void ServerResponded(gameserveritem_t server);

        GCHandle m_pGCHandle;

        readonly IntPtr m_pVTable;

        readonly ServerFailedToRespond m_ServerFailedToRespond;

        readonly ServerResponded m_ServerResponded;

        readonly VTable m_VTable;

        public ISteamMatchmakingPingResponse(ServerResponded onServerResponded, ServerFailedToRespond onServerFailedToRespond) {
            if (onServerResponded == null || onServerFailedToRespond == null) {
                throw new ArgumentNullException();
            }

            m_ServerResponded = onServerResponded;
            m_ServerFailedToRespond = onServerFailedToRespond;

            m_VTable = new VTable {
                m_VTServerResponded = InternalOnServerResponded,
                m_VTServerFailedToRespond = InternalOnServerFailedToRespond
            };

            m_pVTable = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VTable)));
            Marshal.StructureToPtr(m_VTable, m_pVTable, false);
            m_pGCHandle = GCHandle.Alloc(m_pVTable, GCHandleType.Pinned);
        }

        ~ISteamMatchmakingPingResponse() {
            if (m_pVTable != IntPtr.Zero) {
                Marshal.FreeHGlobal(m_pVTable);
            }

            if (m_pGCHandle.IsAllocated) {
                m_pGCHandle.Free();
            }
        }

        void InternalOnServerResponded(gameserveritem_t server) {
            m_ServerResponded(server);
        }

        void InternalOnServerFailedToRespond() {
            m_ServerFailedToRespond();
        }

        public static explicit operator IntPtr(ISteamMatchmakingPingResponse that) => that.m_pGCHandle.AddrOfPinnedObject();

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        delegate void InternalServerResponded(gameserveritem_t server);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        delegate void InternalServerFailedToRespond();

        [StructLayout(LayoutKind.Sequential)]
        class VTable {
            [NonSerialized] [MarshalAs(UnmanagedType.FunctionPtr)]
            public InternalServerFailedToRespond m_VTServerFailedToRespond;

            [NonSerialized] [MarshalAs(UnmanagedType.FunctionPtr)]
            public InternalServerResponded m_VTServerResponded;
        }
    }
}