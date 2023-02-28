using System;
using System.Collections.Generic;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;
using UnityEngine.UI;

public class SmartConsole : MonoBehaviour {
    public delegate void ConsoleCommandFunction(string parameters);

    const float k_animTime = 0.4f;

    const float k_lineSpace = 0.05f;

    const int k_historyLines = 120;

    const float k_toogleCDTime = 0.35f;

    static readonly Vector3 k_position = new(0.01f, 0.65f, 0f);

    static readonly Vector3 k_fullPosition = new(0.01f, 0.05f, 0f);

    static readonly Vector3 k_hidePosition = new(0.01f, 1.1f, 0f);

    static readonly Vector3 k_scale = new(0.5f, 0.5f, 1f);

    static int s_flippy;

    static bool s_blink;

    static bool s_first = true;

    static float s_toggleCooldown;

    static int s_currentCommandHistoryIndex;

    static Font s_font;

    static Variable<bool> s_drawFPS;

    static Variable<bool> s_drawFullConsole;

    static Variable<bool> s_consoleLock;

    static Variable<bool> s_logging;

    static GameObject s_fps;

    static GameObject s_textInput;

    static GameObject[] s_historyDisplay;

    static readonly AutoCompleteDictionary<Command> s_commandDictionary = new();

    static readonly AutoCompleteDictionary<Command> s_variableDictionary = new();

    static readonly AutoCompleteDictionary<Command> s_masterDictionary = new();

    static readonly List<string> s_commandHistory = new();

    static readonly List<string> s_outputHistory = new();

    static string s_lastExceptionCallStack = "(none yet)";

    static string s_lastErrorCallStack = "(none yet)";

    static string s_lastWarningCallStack = "(none yet)";

    static string s_currentInputLine = string.Empty;

    static float s_visiblityLerp;

    static bool s_showConsole;

    public Font m_font;

    void Awake() {
        DontDestroyOnLoad(gameObject);

        if (m_font == null) {
            Debug.LogError("SmartConsole requires a font to be set in the inspector");
        }

        Initialise(this);
    }

    void Update() {
        if (!this.gameObject.activeSelf) {
            return;
        }

        if (s_first) {
            if (s_fps == null || s_textInput == null) {
                Debug.LogWarning("Some variables are null that really shouldn't be! Did you make code changes whilst paused? Be aware that such changes are not safe in general!");
                return;
            }

            SetTopDrawOrderOnGUIText(s_fps.GetComponent<Text>());
            SetTopDrawOrderOnGUIText(s_textInput.GetComponent<Text>());
            GameObject[] array = s_historyDisplay;

            foreach (GameObject gameObject in array) {
                SetTopDrawOrderOnGUIText(gameObject.GetComponent<Text>());
            }

            s_first = false;
        }

        HandleInput();

        if (s_showConsole) {
            s_visiblityLerp += Time.deltaTime / 0.4f;
        } else {
            s_visiblityLerp -= Time.deltaTime / 0.4f;
        }

        s_visiblityLerp = Mathf.Clamp01(s_visiblityLerp);
        transform.position = Vector3.Lerp(k_hidePosition, !s_drawFullConsole ? k_position : k_fullPosition, SmootherStep(s_visiblityLerp));
        transform.localScale = k_scale;

        if (s_textInput != null && s_textInput.GetComponent<Text>() != null) {
            s_textInput.GetComponent<Text>().text = ">" + s_currentInputLine + (!s_blink ? string.Empty : "_");
        }

        s_flippy++;
        s_flippy &= 7;

        if (s_flippy == 0) {
            s_blink = !s_blink;
        }

        if ((bool)s_drawFPS) {
            s_fps.GetComponent<Text>().text = string.Empty + 1f / Time.deltaTime + " fps ";
            s_fps.transform.position = new Vector3(0.8f, 1f, 0f);
        } else {
            s_fps.transform.position = new Vector3(1f, 10f, 0f);
        }
    }

    public static void Clear() {
        s_outputHistory.Clear();
        SetStringsOnHistoryElements();
    }

    public static void Print(string message) {
        WriteLine(message);
    }

    public static void WriteLine(string message) {
        s_outputHistory.Add(DeNewLine(message));
        s_currentCommandHistoryIndex = s_outputHistory.Count - 1;
        SetStringsOnHistoryElements();
    }

    public static void ExecuteLine(string inputLine) {
        WriteLine(">" + inputLine);
        string[] array = CComParameterSplit(inputLine);

        if (array.Length > 0) {
            if (s_masterDictionary.ContainsKey(array[0])) {
                s_commandHistory.Add(inputLine);
                s_masterDictionary[array[0]].m_callback(inputLine);
            } else {
                WriteLine("Unrecognised command or variable name: " + array[0]);
            }
        }
    }

    public static void RemoveCommandIfExists(string name) {
        s_commandDictionary.Remove(name);
        s_masterDictionary.Remove(name);
    }

    public static void RegisterCommand(string name, string exampleUsage, string helpDescription, ConsoleCommandFunction callback) {
        if (!s_commandDictionary.ContainsKey(name)) {
            Command command = new();
            command.m_name = name;
            command.m_paramsExample = exampleUsage;
            command.m_help = helpDescription;
            command.m_callback = callback;
            s_commandDictionary.Add(name, command);
            s_masterDictionary.Add(name, command);
        }
    }

    public static void RegisterCommand(string name, string helpDescription, ConsoleCommandFunction callback) {
        RegisterCommand(name, string.Empty, helpDescription, callback);
    }

    public static void RegisterCommand(string name, ConsoleCommandFunction callback) {
        RegisterCommand(name, string.Empty, "(no description)", callback);
    }

    public static Variable<T> CreateVariable<T>(string name, string description, T initialValue) where T : new() {
        if (s_variableDictionary.ContainsKey(name)) {
            Debug.LogError("Tried to add already existing console variable!");
            return null;
        }

        Variable<T> variable = new(name, description, initialValue);
        s_variableDictionary.Add(name, variable);
        s_masterDictionary.Add(name, variable);
        return variable;
    }

    public static Variable<T> CreateVariable<T>(string name, string description) where T : new() => CreateVariable(name, description, new T());

    public static Variable<T> CreateVariable<T>(string name) where T : new() => CreateVariable<T>(name, string.Empty);

    public static void DestroyVariable<T>(Variable<T> variable) where T : new() {
        s_variableDictionary.Remove(variable.m_name);
        s_masterDictionary.Remove(variable.m_name);
    }

    static void Help(string parameters) {
        string value = string.Empty;

        try {
            value = parameters.Split(' ')[1];
        } catch (Exception) { }

        foreach (Command value2 in s_commandDictionary.Values) {
            if (string.IsNullOrEmpty(value) || value2.m_name.Contains(value)) {
                string text = value2.m_name;

                for (int i = value2.m_name.Length; i < 25; i++) {
                    text += " ";
                }

                text = value2.m_paramsExample.Length <= 0 ? text + "          " : text + " example: " + value2.m_paramsExample;

                for (int j = value2.m_paramsExample.Length; j < 35; j++) {
                    text += " ";
                }

                WriteLine(text + value2.m_help);
            }
        }
    }

    static void Echo(string parameters) {
        string text = string.Empty;
        string[] array = CComParameterSplit(parameters);

        for (int i = 1; i < array.Length; i++) {
            text = text + array[i] + " ";
        }

        if (text.EndsWith(" ")) {
            text.Substring(0, text.Length - 1);
        }

        WriteLine(text);
    }

    static void Clear(string parameters) {
        Clear();
    }

    static void LastExceptionCallStack(string parameters) {
        DumpCallStack(s_lastExceptionCallStack);
    }

    static void LastErrorCallStack(string parameters) {
        DumpCallStack(s_lastErrorCallStack);
    }

    static void LastWarningCallStack(string parameters) {
        DumpCallStack(s_lastWarningCallStack);
    }

    static void Quit(string parameters) {
        Application.Quit();
    }

    static void ListCvars(string parameters) {
        foreach (Command value in s_variableDictionary.Values) {
            string text = value.m_name;

            for (int i = value.m_name.Length; i < 50; i++) {
                text += " ";
            }

            WriteLine(text + value.m_help);
        }
    }

    static void Initialise(SmartConsole instance) {
        if (!(s_textInput != null)) {
            Application.RegisterLogCallback(LogHandler);
            InitialiseCommands();
            InitialiseVariables();
            InitialiseUI(instance);
        }
    }

    static void HandleInput() {
        s_toggleCooldown += !(Time.deltaTime < 0.0166f) ? Time.deltaTime : 0.0166f;

        if (s_toggleCooldown < 0.35f) {
            return;
        }

        bool flag = false;

        if (Input.touchCount > 0) {
            flag = IsInputCoordInBounds(Input.touches[0].position);
        } else if (Input.GetMouseButton(0)) {
            flag = IsInputCoordInBounds(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        }

        if (flag || Input.GetKeyUp(KeyCode.BackQuote)) {
            if (!s_consoleLock) {
                InputManagerImpl.SUSPENDED = false;
                s_showConsole = !s_showConsole;

                if (s_showConsole) {
                    InputManagerImpl.SUSPENDED = true;
                    s_currentInputLine = string.Empty;
                }
            }

            s_toggleCooldown = 0f;
        }

        if (s_commandHistory.Count > 0) {
            bool flag2 = false;

            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                flag2 = true;
                s_currentCommandHistoryIndex--;
            } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
                flag2 = true;
                s_currentCommandHistoryIndex++;
            }

            if (flag2) {
                s_currentCommandHistoryIndex = Mathf.Clamp(s_currentCommandHistoryIndex, 0, s_commandHistory.Count - 1);
                s_currentInputLine = s_commandHistory[s_currentCommandHistoryIndex];
            }
        }

        HandleTextInput();
    }

    static void InitialiseCommands() {
        RegisterCommand("clear", "clear the console log", Clear);
        RegisterCommand("cls", "clear the console log (alias for Clear)", Clear);
        RegisterCommand("echo", "echo <string>", "writes <string> to the console log (alias for echo)", Echo);
        RegisterCommand("help", "displays help information for console command where available. Add parameter to search by filter", Help);
        RegisterCommand("list", "lists all currently registered console variables", ListCvars);
        RegisterCommand("print", "print <string>", "writes <string> to the console log", Echo);
        RegisterCommand("quit", "quit the game (not sure this works with iOS/Android)", Quit);
        RegisterCommand("callstack.warning", "display the call stack for the last warning message", LastWarningCallStack);
        RegisterCommand("callstack.error", "display the call stack for the last error message", LastErrorCallStack);
        RegisterCommand("callstack.exception", "display the call stack for the last exception message", LastExceptionCallStack);
    }

    static void InitialiseVariables() {
        s_drawFPS = CreateVariable("show.fps", "whether to draw framerate counter or not", false);
        s_drawFullConsole = CreateVariable("console.fullscreen", "whether to draw the console over the whole screen or not", false);
        s_consoleLock = CreateVariable("console.lock", "whether to allow showing/hiding the console", false);
        s_logging = CreateVariable("console.log", "whether to redirect log to the console", true);
    }

    static void InitialiseUI(SmartConsole instance) {
        s_font = instance.m_font;

        if (s_font == null) {
            Debug.LogError("SmartConsole needs to have a font set on an instance in the editor!");
            s_font = new Font("Arial");
        }

        s_fps = instance.AddChildWithGUIText("FPSCounter");
        s_textInput = instance.AddChildWithGUIText("SmartConsoleInputField");
        s_historyDisplay = new GameObject[120];

        for (int i = 0; i < 120; i++) {
            s_historyDisplay[i] = instance.AddChildWithGUIText("SmartConsoleHistoryDisplay" + i);
        }

        instance.Layout();
    }

    GameObject AddChildWithGUIText(string name) => AddChildWithComponent<Text>(name);

    GameObject AddChildWithComponent<T>(string name) where T : Component {
        GameObject gameObject = new();
        gameObject.AddComponent<T>();
        gameObject.transform.parent = transform;
        gameObject.name = name;
        return gameObject;
    }

    static void SetTopDrawOrderOnGUIText(Text text) { }

    static void HandleTextInput() {
        bool flag = false;
        string inputString = Input.inputString;

        foreach (char c in inputString) {
            switch (c) {
                case '\b':
                    s_currentInputLine = s_currentInputLine.Length <= 0 ? string.Empty : s_currentInputLine.Substring(0, s_currentInputLine.Length - 1);
                    break;

                case '\n':
                case '\r':
                    ExecuteCurrentLine();
                    s_currentInputLine = string.Empty;
                    break;

                case '\t':
                    AutoComplete();
                    flag = true;
                    break;

                default:
                    s_currentInputLine += c;
                    break;
            }
        }

        if (!flag && Input.GetKeyDown(KeyCode.Tab)) {
            AutoComplete();
        }
    }

    static void ExecuteCurrentLine() {
        ExecuteLine(s_currentInputLine);
    }

    static void AutoComplete() {
        string[] array = CComParameterSplit(s_currentInputLine);

        if (array.Length == 0) {
            return;
        }

        Command command = s_masterDictionary.AutoCompleteLookup(array[0]);
        int num = 0;

        do {
            num = command.m_name.IndexOf(".", num + 1);
        } while (num > 0 && num < array[0].Length);

        string text = command.m_name;

        if (num >= 0) {
            text = command.m_name.Substring(0, num + 1);
        }

        if (text.Length < s_currentInputLine.Length) {
            if (!AutoCompleteTailString("true") && !AutoCompleteTailString("false") && !AutoCompleteTailString("True") && !AutoCompleteTailString("False") &&
                !AutoCompleteTailString("TRUE") && !AutoCompleteTailString("FALSE")) { }
        } else if (text.Length >= s_currentInputLine.Length) {
            s_currentInputLine = text;
        }
    }

    static bool AutoCompleteTailString(string tailString) {
        for (int i = 1; i < tailString.Length; i++) {
            if (s_currentInputLine.EndsWith(" " + tailString.Substring(0, i))) {
                s_currentInputLine = s_currentInputLine.Substring(0, s_currentInputLine.Length - 1) + tailString.Substring(i - 1);
                return true;
            }
        }

        return false;
    }

    void Layout() {
        float num = 0f;
        LayoutTextAtY(s_textInput, num);
        LayoutTextAtY(s_fps, num);
        num += 0.05f;

        for (int i = 0; i < 120; i++) {
            LayoutTextAtY(s_historyDisplay[i], num);
            num += 0.05f;
        }
    }

    static void LayoutTextAtY(GameObject o, float y) {
        o.transform.localPosition = new Vector3(0f, y, 0f);
        o.GetComponent<Text>().fontStyle = FontStyle.Normal;
        o.GetComponent<Text>().font = s_font;
    }

    static void SetStringsOnHistoryElements() {
        for (int i = 0; i < 120; i++) {
            int num = s_outputHistory.Count - 1 - i;

            if (num >= 0) {
                s_historyDisplay[i].GetComponent<Text>().text = s_outputHistory[s_outputHistory.Count - 1 - i];
            } else {
                s_historyDisplay[i].GetComponent<Text>().text = string.Empty;
            }
        }
    }

    static bool IsInputCoordInBounds(Vector2 inputCoordinate) => inputCoordinate.x < 0.05f * Screen.width && inputCoordinate.y > 0.95f * Screen.height;

    static void LogHandler(string message, string stack, LogType type) {
        if ((bool)s_logging) {
            string text = "[Assert]:             ";
            string text2 = "[Debug.LogError]:     ";
            string text3 = "[Debug.LogException]: ";
            string text4 = "[Debug.LogWarning]:   ";
            string text5 = "[Debug.Log]:          ";
            string text6 = text5;

            switch (type) {
                case LogType.Assert:
                    text6 = text;
                    break;

                case LogType.Warning:
                    text6 = text4;
                    s_lastWarningCallStack = stack;
                    break;

                case LogType.Error:
                    text6 = text2;
                    s_lastErrorCallStack = stack;
                    break;

                case LogType.Exception:
                    text6 = text3;
                    s_lastExceptionCallStack = stack;
                    break;
            }

            WriteLine(text6 + message);

            switch (type) {
                case LogType.Error:
                case LogType.Assert:
                case LogType.Exception:
                    break;

                case LogType.Warning:
                case LogType.Log:
                    break;
            }
        }
    }

    public static string[] CComParameterSplit(string parameters) {
        return parameters.Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
    }

    public static string[] CComParameterSplit(string parameters, int requiredParameters) {
        string[] array = CComParameterSplit(parameters);

        if (array.Length < requiredParameters + 1) {
            WriteLine("Error: not enough parameters for command. Expected " + requiredParameters + " found " + (array.Length - 1));
        }

        if (array.Length > requiredParameters + 1) {
            int num = array.Length - 1 - requiredParameters;
            WriteLine("Warning: " + num + "additional parameters will be dropped:");

            for (int i = array.Length - num; i < array.Length; i++) {
                WriteLine("\"" + array[i] + "\"");
            }
        }

        return array;
    }

    static string[] CVarParameterSplit(string parameters) {
        string[] array = CComParameterSplit(parameters);

        if (array.Length == 0) {
            WriteLine("Error: not enough parameters to set or display the value of a console variable.");
        }

        if (array.Length > 2) {
            int num = array.Length - 3;
            WriteLine("Warning: " + num + "additional parameters will be dropped:");

            for (int i = array.Length - num; i < array.Length; i++) {
                WriteLine("\"" + array[i] + "\"");
            }
        }

        return array;
    }

    static string DeNewLine(string message) => message.Replace("\n", " | ");

    static void DumpCallStack(string stackString) {
        string[] array = stackString.Split('\r', '\n');

        if (array.Length != 0) {
            int i;

            for (i = 0; array[array.Length - 1 - i].Length == 0 && i < array.Length; i++) { }

            int num = array.Length - i;

            for (int j = 0; j < num; j++) {
                WriteLine(j + 1 + (j >= 9 ? " " : "  ") + array[j]);
            }
        }
    }

    float SmootherStep(float t) => ((6f * t - 15f) * t + 10f) * t * t * t;

    public class Command {
        public ConsoleCommandFunction m_callback;

        public string m_help = "(no description)";

        public string m_name;

        public string m_paramsExample = string.Empty;
    }

    public class Variable<T> : Command where T : new() {
        T m_value;

        public Variable(string name) {
            Initialise(name, string.Empty, new T());
        }

        public Variable(string name, string description) {
            Initialise(name, description, new T());
        }

        public Variable(string name, T initialValue) {
            Initialise(name, string.Empty, initialValue);
        }

        public Variable(string name, string description, T initalValue) {
            Initialise(name, description, initalValue);
        }

        public void Set(T val) {
            m_value = val;
        }

        public static implicit operator T(Variable<T> var) => var.m_value;

        void Initialise(string name, string description, T initalValue) {
            m_name = name;
            m_help = description;
            m_paramsExample = string.Empty;
            m_value = initalValue;
            m_callback = CommandFunction;
        }

        static void CommandFunction(string parameters) {
            string[] array = CVarParameterSplit(parameters);

            if (array.Length != 0 && s_variableDictionary.ContainsKey(array[0])) {
                Variable<T> variable = s_variableDictionary[array[0]] as Variable<T>;
                string text = " is set to ";

                if (array.Length == 2) {
                    variable.SetFromString(array[1]);
                    text = " has been set to ";
                }

                WriteLine(variable.m_name + text + variable.m_value);
            }
        }

        void SetFromString(string value) {
            m_value = (T)Convert.ChangeType(value, typeof(T));
        }
    }

    class AutoCompleteDictionary<T> : SortedDictionary<string, T> {
        readonly AutoCompleteComparer m_comparer;

        public AutoCompleteDictionary()
            : base(new AutoCompleteComparer()) => m_comparer = Comparer as AutoCompleteComparer;

        public T LowerBound(string lookupString) {
            m_comparer.Reset();
            ContainsKey(lookupString);
            return base[m_comparer.LowerBound];
        }

        public T UpperBound(string lookupString) {
            m_comparer.Reset();
            ContainsKey(lookupString);
            return base[m_comparer.UpperBound];
        }

        public T AutoCompleteLookup(string lookupString) {
            m_comparer.Reset();
            ContainsKey(lookupString);
            string key = m_comparer.UpperBound != null ? m_comparer.UpperBound : m_comparer.LowerBound;
            return base[key];
        }

        class AutoCompleteComparer : IComparer<string> {
            public string LowerBound { get; private set; }

            public string UpperBound { get; private set; }

            public int Compare(string x, string y) {
                int num = Comparer<string>.Default.Compare(x, y);

                if (num >= 0) {
                    LowerBound = y;
                }

                if (num <= 0) {
                    UpperBound = y;
                }

                return num;
            }

            public void Reset() {
                LowerBound = null;
                UpperBound = null;
            }
        }
    }
}