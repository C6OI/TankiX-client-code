using System;
using System.Text;
using Steamworks;
using UnityEngine;

[DisallowMultipleComponent]
public class SteamManager : MonoBehaviour {
    static SteamManager s_instance;

    static bool s_EverInialized;

    bool m_bInitialized;

    SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;

    static SteamManager Instance {
        get {
            if (s_instance == null) {
                return new GameObject("SteamManager").AddComponent<SteamManager>();
            }

            return s_instance;
        }
    }

    public static bool Initialized => Instance.m_bInitialized;

    void Awake() {
        if (s_instance != null) {
            Destroy(gameObject);
            return;
        }

        s_instance = this;

        if (s_EverInialized) {
            throw new Exception("Tried to Initialize the SteamAPI twice in one session!");
        }

        DontDestroyOnLoad(gameObject);

        if (!Packsize.Test()) {
            Debug.LogError("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", this);
        }

        if (!DllCheck.Test()) {
            Debug.LogError("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", this);
        }

        try {
            if (SteamAPI.RestartAppIfNecessary(AppId_t.Invalid)) {
                Application.Quit();
                return;
            }
        } catch (DllNotFoundException ex) {
            Debug.LogError("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + ex, this);
            Application.Quit();
            return;
        }

        m_bInitialized = SteamAPI.Init();

        if (!m_bInitialized) {
            Debug.Log("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.", this);
        } else {
            s_EverInialized = true;
        }
    }

    void Update() {
        if (m_bInitialized) {
            SteamAPI.RunCallbacks();
        }
    }

    void OnEnable() {
        if (s_instance == null) {
            s_instance = this;
        }

        if (m_bInitialized && m_SteamAPIWarningMessageHook == null) {
            m_SteamAPIWarningMessageHook = SteamAPIDebugTextHook;
            SteamClient.SetWarningMessageHook(m_SteamAPIWarningMessageHook);
        }
    }

    void OnDestroy() {
        if (!(s_instance != this)) {
            s_instance = null;

            if (m_bInitialized) {
                SteamAPI.Shutdown();
            }
        }
    }

    static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText) {
        Debug.LogWarning(pchDebugText);
    }
}