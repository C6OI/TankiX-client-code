using System;
using System.Runtime.InteropServices;

namespace Steamworks {
    public sealed class CallResult<T> : IDisposable {
        public delegate void APIDispatchDelegate(T param, bool bIOFailure);

        readonly int m_size = Marshal.SizeOf(typeof(T));

        bool m_bDisposed;

        CCallbackBase m_CCallbackBase;

        GCHandle m_pCCallbackBase;

        IntPtr m_pVTable = IntPtr.Zero;

        CCallbackBaseVTable VTable;

        public CallResult(APIDispatchDelegate func = null) {
            m_Func = func;
            BuildCCallbackBase();
        }

        public SteamAPICall_t Handle { get; private set; } = SteamAPICall_t.Invalid;

        public void Dispose() {
            if (!m_bDisposed) {
                GC.SuppressFinalize(this);
                Cancel();

                if (m_pVTable != IntPtr.Zero) {
                    Marshal.FreeHGlobal(m_pVTable);
                }

                if (m_pCCallbackBase.IsAllocated) {
                    m_pCCallbackBase.Free();
                }

                m_bDisposed = true;
            }
        }

        event APIDispatchDelegate m_Func;

        public static CallResult<T> Create(APIDispatchDelegate func = null) => new(func);

        ~CallResult() {
            Dispose();
        }

        public void Set(SteamAPICall_t hAPICall, APIDispatchDelegate func = null) {
            if (func != null) {
                m_Func = func;
            }

            if (m_Func == null) {
                throw new Exception("CallResult function was null, you must either set it in the CallResult Constructor or in Set()");
            }

            if (Handle != SteamAPICall_t.Invalid) {
                NativeMethods.SteamAPI_UnregisterCallResult(m_pCCallbackBase.AddrOfPinnedObject(), (ulong)Handle);
            }

            Handle = hAPICall;

            if (hAPICall != SteamAPICall_t.Invalid) {
                NativeMethods.SteamAPI_RegisterCallResult(m_pCCallbackBase.AddrOfPinnedObject(), (ulong)hAPICall);
            }
        }

        public bool IsActive() => Handle != SteamAPICall_t.Invalid;

        public void Cancel() {
            if (Handle != SteamAPICall_t.Invalid) {
                NativeMethods.SteamAPI_UnregisterCallResult(m_pCCallbackBase.AddrOfPinnedObject(), (ulong)Handle);
                Handle = SteamAPICall_t.Invalid;
            }
        }

        public void SetGameserverFlag() {
            m_CCallbackBase.m_nCallbackFlags |= 2;
        }

        void OnRunCallback(IntPtr pvParam) {
            Handle = SteamAPICall_t.Invalid;

            try {
                m_Func((T)Marshal.PtrToStructure(pvParam, typeof(T)), false);
            } catch (Exception e) {
                CallbackDispatcher.ExceptionHandler(e);
            }
        }

        void OnRunCallResult(IntPtr pvParam, bool bFailed, ulong hSteamAPICall) {
            SteamAPICall_t steamAPICall_t = (SteamAPICall_t)hSteamAPICall;

            if (steamAPICall_t == Handle) {
                try {
                    m_Func((T)Marshal.PtrToStructure(pvParam, typeof(T)), bFailed);
                } catch (Exception e) {
                    CallbackDispatcher.ExceptionHandler(e);
                }

                if (steamAPICall_t == Handle) {
                    Handle = SteamAPICall_t.Invalid;
                }
            }
        }

        int OnGetCallbackSizeBytes() => m_size;

        void BuildCCallbackBase() {
            VTable = new CCallbackBaseVTable {
                m_RunCallback = OnRunCallback,
                m_RunCallResult = OnRunCallResult,
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