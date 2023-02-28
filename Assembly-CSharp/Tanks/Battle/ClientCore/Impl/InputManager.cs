using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public interface InputManager {
        void ResetToDefaultActions();

        void ClearActions();

        void RegisterInputAction(InputAction action);

        void RegisterDefaultInputAction(InputAction inputAction);

        bool CheckAction(string actionName);

        bool GetActionKeyDown(string actionName);

        bool GetActionKeyUp(string actionName);

        float GetUnityAxis(string axisName);

        float GetAxis(string name, bool mustExistForAllContext = false);

        float GetAxisOrKey(string actionName);

        void ChangeInputActionKey(InputActionId actionId, InputActionContextId contextId, int keyId, KeyCode newKeyCode);

        InputKeyCode GetCurrentKeyPressed();

        InputAction GetAction(InputActionId actionId, InputActionContextId contextId);

        bool GetMouseButton(int mouseButton);

        bool GetMouseButtonDown(int mouseButton);

        bool GetMouseButtonUp(int mouseButton);

        bool CheckMouseButtonInAllActiveContexts(string actionName, int mouseButton);

        bool GetKey(KeyCode keyCode);

        bool GetKeyDown(KeyCode keyCode);

        bool GetKeyUp(KeyCode keyCode);

        void ActivateContext(string contextName);

        void DeactivateContext(string contextName);

        void Suspend();

        void Resume();

        void ResumeAtNextFrame();

        void Update();

        bool IsAnyKey();

        void DeleteKeyBinding(InputActionId actionId, InputActionContextId contextId, int id);
    }
}