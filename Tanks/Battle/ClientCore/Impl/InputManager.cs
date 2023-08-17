using System;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public interface InputManager {
        void ClearActions();

        void RegisterInputAction(InputAction action);

        bool CheckAction(string actionName);

        bool GetActionKeyDown(string actionName);

        bool GetActionKeyUp(string actionName);

        void SetActionStartHandler(string name, Action action);

        void UnsetActionStartHandler(string name);

        void SetActionStopHandler(string name, Action action);

        void UnsetActionStopHandler(string name);

        float GetUnityAxis(string axisName);

        float GetAxis(string name, bool mustExistForAllContext = false);

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

        void Update();

        bool IsAnyKey();
    }
}