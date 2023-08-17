using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Platform.Library.ClientDataStructures.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class InputManagerImpl : InputManager {
        readonly MultiMap<InputAction, string> actionToAxes = new();

        readonly MultiMap<InputAction, KeyCode> actionToKeys = new();

        readonly HashSet<string> activeContexts = new();

        readonly Dictionary<InputAction, float> axisVals = new();
        readonly MultiMap<string, InputAction> contextToActions = new();

        readonly HashSet<int> keysPressed = new();

        readonly MultiMap<string, InputAction> nameToAction = new();

        readonly HashSet<InputAction> pendingActions = new();

        bool suspended;

        public void ClearActions() {
            keysPressed.Clear();
            pendingActions.Clear();
        }

        public void RegisterInputAction(InputAction action) {
            string actionName = action.actionId.actionName;
            nameToAction.Add(actionName, action);
            string contextName = action.contextId.contextName;
            contextToActions.Add(contextName, action);
            KeyCode[] keys = action.keys;
            int num = keys.Length;

            for (int i = 0; i < num; i++) {
                actionToKeys.Add(action, keys[i]);
            }

            MultiKeys[] multiKeys = action.multiKeys;
            int num2 = multiKeys.Length;

            for (int j = 0; j < num2; j++) {
                KeyCode[] keys2 = multiKeys[j].keys;
                num = keys2.Length;

                for (int k = 0; k < num; k++) {
                    actionToKeys.Add(action, keys2[k]);
                }
            }

            UnityInputAxes[] axes = action.axes;
            int num3 = axes.Length;

            for (int l = 0; l < num3; l++) {
                string enumDescription = GetEnumDescription(action.axes[l]);
                actionToAxes.Add(action, enumDescription);
            }
        }

        public bool CheckAction(string actionName) {
            if (suspended) {
                return false;
            }

            HashSet<string>.Enumerator enumerator = activeContexts.GetEnumerator();

            while (enumerator.MoveNext()) {
                string current = enumerator.Current;
                HashSet<InputAction> hashSet = contextToActions[current];
                HashSet<InputAction>.Enumerator enumerator2 = hashSet.GetEnumerator();

                while (enumerator2.MoveNext()) {
                    InputAction current2 = enumerator2.Current;

                    if (!(current2.actionId.actionName == actionName)) {
                        continue;
                    }

                    KeyCode[] keys = current2.keys;
                    int num = keys.Length;

                    for (int i = 0; i < num; i++) {
                        if (keysPressed.Contains((int)keys[i])) {
                            return true;
                        }
                    }

                    MultiKeys[] multiKeys = current2.multiKeys;
                    int num2 = multiKeys.Length;

                    for (int j = 0; j < num2; j++) {
                        MultiKeys multiKeys2 = multiKeys[j];
                        keys = multiKeys2.keys;
                        num = keys.Length;

                        if (num > 0) {
                            bool flag = true;

                            for (int k = 0; k < num; k++) {
                                flag &= keysPressed.Contains((int)keys[k]);
                            }

                            if (flag) {
                                return true;
                            }
                        }
                    }
                }
            }

            HashSet<InputAction>.Enumerator enumerator3 = pendingActions.GetEnumerator();

            while (enumerator3.MoveNext()) {
                InputAction current3 = enumerator3.Current;

                if (current3.actionId.actionName == actionName) {
                    return true;
                }
            }

            return false;
        }

        public bool GetActionKeyDown(string actionName) {
            if (suspended) {
                return false;
            }

            HashSet<string>.Enumerator enumerator = activeContexts.GetEnumerator();

            while (enumerator.MoveNext()) {
                string current = enumerator.Current;
                HashSet<InputAction> hashSet = contextToActions[current];
                HashSet<InputAction>.Enumerator enumerator2 = hashSet.GetEnumerator();

                while (enumerator2.MoveNext()) {
                    InputAction current2 = enumerator2.Current;

                    if (!(current2.actionId.actionName == actionName)) {
                        continue;
                    }

                    KeyCode[] keys = current2.keys;
                    int num = keys.Length;

                    for (int i = 0; i < num; i++) {
                        if (Input.GetKeyDown(keys[i])) {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public bool GetActionKeyUp(string actionName) {
            if (suspended) {
                return false;
            }

            HashSet<string>.Enumerator enumerator = activeContexts.GetEnumerator();

            while (enumerator.MoveNext()) {
                string current = enumerator.Current;
                HashSet<InputAction> hashSet = contextToActions[current];
                HashSet<InputAction>.Enumerator enumerator2 = hashSet.GetEnumerator();

                while (enumerator2.MoveNext()) {
                    InputAction current2 = enumerator2.Current;

                    if (!(current2.actionId.actionName == actionName)) {
                        continue;
                    }

                    KeyCode[] keys = current2.keys;
                    int num = keys.Length;

                    for (int i = 0; i < num; i++) {
                        if (Input.GetKeyUp(keys[i])) {
                            return true;
                        }
                    }
                }
            }

            HashSet<InputAction>.Enumerator enumerator3 = pendingActions.GetEnumerator();

            while (enumerator3.MoveNext()) {
                InputAction current3 = enumerator3.Current;

                if (!(current3.actionId.actionName == actionName)) {
                    continue;
                }

                KeyCode[] keys2 = current3.keys;
                int num2 = keys2.Length;

                for (int j = 0; j < num2; j++) {
                    if (Input.GetKeyUp(keys2[j])) {
                        return true;
                    }
                }
            }

            return false;
        }

        public void SetActionStartHandler(string name, Action action) => SetStartHandlerToInputActions(name, action);

        public void UnsetActionStartHandler(string name) => SetStartHandlerToInputActions(name, null);

        public void SetActionStopHandler(string name, Action action) => SetStopHandlerToInputActions(name, action);

        public void UnsetActionStopHandler(string name) => SetStopHandlerToInputActions(name, null);

        public float GetUnityAxis(string axisName) => !suspended ? Input.GetAxis(axisName) : 0f;

        public float GetAxis(string name, bool mustExistForAllContext) {
            if (suspended) {
                return 0f;
            }

            HashSet<string>.Enumerator enumerator = activeContexts.GetEnumerator();
            float num = 0f;

            while (enumerator.MoveNext()) {
                string current = enumerator.Current;
                HashSet<InputAction> hashSet = contextToActions[current];
                HashSet<InputAction>.Enumerator enumerator2 = hashSet.GetEnumerator();
                bool flag = false;

                while (enumerator2.MoveNext()) {
                    InputAction current2 = enumerator2.Current;

                    if (current2.actionId.actionName == name) {
                        flag = true;
                        float value;

                        if (axisVals.TryGetValue(current2, out value) && value != 0f) {
                            num = value;
                        }
                    }
                }

                if (mustExistForAllContext && !flag) {
                    return 0f;
                }
            }

            if (num == 0f && CheckAction(name)) {
                num = 1f;
            }

            return num;
        }

        public bool GetMouseButton(int mouseButton) => !suspended && Input.GetMouseButton(mouseButton);

        public bool GetMouseButtonDown(int mouseButton) => !suspended && Input.GetMouseButtonDown(mouseButton);

        public bool GetMouseButtonUp(int mouseButton) => !suspended && Input.GetMouseButtonUp(mouseButton);

        public bool CheckMouseButtonInAllActiveContexts(string actionName, int mouseButton) {
            if (suspended) {
                return false;
            }

            if (!Input.GetMouseButton(mouseButton)) {
                return false;
            }

            return AllActiveContextsContainAction(actionName);
        }

        public bool GetKey(KeyCode keyCode) {
            if (suspended) {
                return false;
            }

            return keysPressed.Contains((int)keyCode);
        }

        public bool GetKeyDown(KeyCode keyCode) {
            if (suspended) {
                return false;
            }

            return !suspended && Input.GetKeyDown(keyCode);
        }

        public bool GetKeyUp(KeyCode keyCode) {
            if (suspended) {
                return false;
            }

            return !suspended && Input.GetKeyUp(keyCode);
        }

        public bool IsAnyKey() {
            if (suspended) {
                return false;
            }

            return Input.anyKeyDown;
        }

        public void ActivateContext(string contextName) {
            activeContexts.Add(contextName);
            Update();
        }

        public void DeactivateContext(string contextName) {
            if (!activeContexts.Contains(contextName)) {
                return;
            }

            HashSet<InputAction> hashSet = contextToActions[contextName];
            activeContexts.Remove(contextName);
            HashSet<InputAction>.Enumerator enumerator = hashSet.GetEnumerator();

            while (enumerator.MoveNext()) {
                InputAction current = enumerator.Current;
                KeyCode[] keys = current.keys;
                int num = keys.Length;

                for (int i = 0; i < num; i++) {
                    if (keysPressed.Contains((int)keys[i])) {
                        pendingActions.Add(current);
                    }
                }
            }

            ResetAxes(contextName);
        }

        public void Suspend() {
            ClearActions();
            suspended = true;
        }

        public void Resume() => suspended = false;

        public void Update() {
            if (suspended) {
                return;
            }

            HashSet<string>.Enumerator enumerator = activeContexts.GetEnumerator();

            while (enumerator.MoveNext()) {
                string current = enumerator.Current;
                HashSet<InputAction> hashSet = contextToActions[current];
                HashSet<InputAction>.Enumerator enumerator2 = hashSet.GetEnumerator();

                while (enumerator2.MoveNext()) {
                    InputAction current2 = enumerator2.Current;
                    UpdateKeysInActiveContext(current2);
                    UpdateAxisInActiveContext(current2);
                }
            }

            if (pendingActions.Count <= 0) {
                return;
            }

            pendingActions.RemoveWhere(action => !action.keys.Any(key => Input.GetKey(key) || Input.GetKeyUp(key)));
        }

        static string GetEnumDescription(Enum value) {
            FieldInfo field = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] array =
                (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (array != null && array.Length > 0) {
                return array[0].Description;
            }

            return value.ToString();
        }

        void SetStartHandlerToInputActions(string name, Action action) {
            if (nameToAction.ContainsKey(name)) {
                HashSet<InputAction> hashSet = nameToAction[name];
                HashSet<InputAction>.Enumerator enumerator = hashSet.GetEnumerator();

                while (enumerator.MoveNext()) {
                    InputAction current = enumerator.Current;
                    current.StartHandler = action;
                }
            }
        }

        void SetStopHandlerToInputActions(string name, Action action) {
            if (nameToAction.ContainsKey(name)) {
                HashSet<InputAction> hashSet = nameToAction[name];
                HashSet<InputAction>.Enumerator enumerator = hashSet.GetEnumerator();

                while (enumerator.MoveNext()) {
                    InputAction current = enumerator.Current;
                    current.StopHandler = action;
                }
            }
        }

        bool AllActiveContextsContainAction(string name) {
            bool result = false;
            HashSet<string>.Enumerator enumerator = activeContexts.GetEnumerator();

            while (enumerator.MoveNext()) {
                string current = enumerator.Current;
                HashSet<InputAction> hashSet = contextToActions[current];
                HashSet<InputAction>.Enumerator enumerator2 = hashSet.GetEnumerator();
                bool flag = false;

                while (enumerator2.MoveNext()) {
                    if (enumerator2.Current.actionId.actionName == name) {
                        flag = true;
                        result = true;
                        break;
                    }
                }

                if (!flag) {
                    return false;
                }
            }

            return result;
        }

        void ResetAxes(string contextName) {
            HashSet<InputAction> hashSet = contextToActions[contextName];
            HashSet<InputAction>.Enumerator enumerator = hashSet.GetEnumerator();

            while (enumerator.MoveNext()) {
                InputAction current = enumerator.Current;

                if (axisVals.ContainsKey(current)) {
                    axisVals[current] = 0f;
                }
            }
        }

        void UpdateKeysInActiveContext(InputAction action) {
            if (!actionToKeys.ContainsKey(action)) {
                return;
            }

            HashSet<KeyCode> hashSet = actionToKeys[action];
            HashSet<KeyCode>.Enumerator enumerator = hashSet.GetEnumerator();

            while (enumerator.MoveNext()) {
                KeyCode current = enumerator.Current;
                int item = (int)current;
                bool flag = keysPressed.Contains(item);

                if (Input.GetKey(current)) {
                    if (!flag) {
                        keysPressed.Add(item);
                        action.StartInputAction();
                    }
                } else if (flag) {
                    keysPressed.Remove(item);
                    action.StopInputAction();
                }
            }
        }

        void UpdateAxisInActiveContext(InputAction action) {
            if (!actionToAxes.ContainsKey(action)) {
                return;
            }

            HashSet<string> hashSet = actionToAxes[action];
            HashSet<string>.Enumerator enumerator = hashSet.GetEnumerator();

            while (enumerator.MoveNext()) {
                string current = enumerator.Current;
                float axis = Input.GetAxis(current);
                bool flag = action.onlyNegativeAxes && !action.onlyPositiveAxes;
                bool flag2 = !action.onlyNegativeAxes && action.onlyPositiveAxes;
                float num = !action.invertAxes ? 1 : -1;

                if (flag) {
                    if (axis * num < 0f) {
                        axisVals[action] = Mathf.Abs(axis);
                    } else {
                        axisVals[action] = 0f;
                    }
                } else if (flag2) {
                    if (axis * num > 0f) {
                        axisVals[action] = Mathf.Abs(axis);
                    } else {
                        axisVals[action] = 0f;
                    }
                } else {
                    axisVals[action] = axis * num;
                }
            }
        }
    }
}