using System;
using System.Runtime.InteropServices;

namespace Steamworks {
    public sealed class Callback<T> : IDisposable {
        public delegate void DispatchDelegate(T param);

        readonly int m_size = Marshal.SizeOf(typeof(T));

        bool m_bDisposed;

        readonly bool m_bGameServer;

        CCallbackBase m_CCallbackBase;

        GCHandle m_pCCallbackBase;

        IntPtr m_pVTable = IntPtr.Zero;

        CCallbackBaseVTable VTable;

        public Callback(DispatchDelegate func, bool bGameServer = false) {
            m_bGameServer = bGameServer;
            BuildCCallbackBase();
            Register(func);
        }

        public void Dispose() {
            if (!m_bDisposed) {
                GC.SuppressFinalize(this);
                Unregister();

                if (m_pVTable != IntPtr.Zero) {
                    Marshal.FreeHGlobal(m_pVTable);
                }

                if (m_pCCallbackBase.IsAllocated) {
                    m_pCCallbackBase.Free();
                }

                m_bDisposed = true;
            }
        }

        event DispatchDelegate m_Func;

        public static Callback<T> Create(DispatchDelegate func) => new(func);

        public static Callback<T> CreateGameServer(DispatchDelegate func) => new(func, true);

        ~Callback() {
            Dispose();
        }

        public void Register(DispatchDelegate func) {
            if (func == null) {
                throw new Exception("Callback function must not be null.");
            }

            if ((m_CCallbackBase.m_nCallbackFlags & 1) == 1) {
                Unregister();
            }

            if (m_bGameServer) {
                SetGameserverFlag();
            }

            m_Func = func;
            NativeMethods.SteamAPI_RegisterCallback(m_pCCallbackBase.AddrOfPinnedObject(), CallbackIdentities.GetCallbackIdentity(typeof(T)));
        }

        public void Unregister() {
            NativeMethods.SteamAPI_UnregisterCallback(m_pCCallbackBase.AddrOfPinnedObject());
        }

        public void SetGameserverFlag() {
            m_CCallbackBase.m_nCallbackFlags |= 2;
        }

        void OnRunCallback(IntPtr pvParam) {
            try {
                m_Func((T)Marshal.PtrToStructure(pvParam, typeof(T)));
            } catch (Exception e) {
                CallbackDispatcher.ExceptionHandler(e);
            }
        }

        void OnRunCallResult(IntPtr pvParam, bool bFailed, ulong hSteamAPICall) {
            try {
                m_Func((T)Marshal.PtrToStructure(pvParam, typeof(T)));
            } catch (Exception e) {
                CallbackDispatcher.ExceptionHandler(e);
            }
        }

        int OnGetCallbackSizeBytes() => m_size;

        void BuildCCallbackBase() {
            VTable = new CCallbackBaseVTable {
                m_RunCallResult = OnRunCallResult,
                m_RunCallback = OnRunCallback,
                m_GetCallbackSizeBytes = OnGetCallbackSizeBytes
            };

            m_pVTable = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(CCallbackBaseVTable)));
            Marshal.StructureToPtr(VTable, m_pVTable, false);

            m_CCallbackBase = new CCallbackBase {
                m_vfptr = m_pVTable,
                m_nCallbackFlags = 0,
                m_iCallback = CallbackIdentities.GetCallbackIdentity(typeof(T))
            };

            m_pCCallbackBase = GCHandle.Alloc(m_CCallbackBase, GCHandleType.Pinned);
        }
    }
}