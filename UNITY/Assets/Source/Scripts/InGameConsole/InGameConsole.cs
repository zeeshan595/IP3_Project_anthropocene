using UnityEngine;
using System.Collections.Generic;
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Text;

public class InGameConsole : MonoBehaviour
{
    public bool showConsole = false;
    public Color errorColor = Color.red;
    public Color warningColor = Color.magenta;
    public Color normalColor = Color.white;

    private List<LogMessage> logs = new List<LogMessage>();
    private string command = "";
    private GUIStyle error;
    private GUIStyle warning;
    private GUIStyle normal;
    private Vector2 scrollView = Vector2.zero;
    private bool autoScroll = true;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        Application.logMessageReceived += Application_logMessageReceived;
        error = new GUIStyle();
        warning = new GUIStyle();
        normal = new GUIStyle();
        error.normal.textColor = errorColor;
        warning.normal.textColor = warningColor;
        normal.normal.textColor = normalColor;
    }

    private void Run(string cmd)
    {
        var assembly = Compile(@"
        using UnityEngine;
 
        public class Test
        {
            public static void Foo()
            {
                "+ cmd + "}}");

        var method = assembly.GetType("Test").GetMethod("Foo");
        var del = (Action)Delegate.CreateDelegate(typeof(Action), method);
        del.Invoke();
    }

    private static Assembly Compile(string source)
    {
        var provider = new CSharpCodeProvider();
        var param = new CompilerParameters();

        // Add ALL of the assembly references
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            param.ReferencedAssemblies.Add(assembly.Location);
        }

        // Add specific assembly references
        //param.ReferencedAssemblies.Add("System.dll");
        //param.ReferencedAssemblies.Add("CSharp.dll");
        //param.ReferencedAssemblies.Add("UnityEngines.dll");

        // Generate a dll in memory
        param.GenerateExecutable = false;
        param.GenerateInMemory = true;

        // Compile the source
        var result = provider.CompileAssemblyFromSource(param, source);

        if (result.Errors.Count > 0)
        {
            var msg = new StringBuilder();
            foreach (CompilerError error in result.Errors)
            {
                msg.AppendFormat("Error ({0}): {1}\n",
                    error.ErrorNumber, error.ErrorText);
            }
            throw new Exception(msg.ToString());
        }

        // Return the assembly
        return result.CompiledAssembly;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Slash))
        {
            command = "";
            showConsole = !showConsole;
        }
    }

    private void OnGUI()
    {
        if (showConsole)
        {
            GUI.Window(0, new Rect(5, Screen.height - 235, Screen.width - 10, 200), DebugLog, "Console");
            GUI.SetNextControlName("MyTextField");
            command = GUI.TextField(new Rect(5, Screen.height - 30, Screen.width - 10, 25), command);
            if (Event.current.keyCode == KeyCode.Return)
            {
                Run(command);
                command = "";
                GUI.FocusControl("MyTextField");
            }

            if (autoScroll)
                scrollView.y++;
        }
    }

    private void DebugLog(int id)
    {
        autoScroll = GUILayout.Toggle(autoScroll, "Auto Scroll");
        scrollView = GUILayout.BeginScrollView(scrollView);
        for (int i = 0; i < logs.Count; i++)
        {
            switch (logs[i].type)
            {
                case LogType.Error:
                    GUILayout.Label(logs[i].condition, error);
                    break;
                case LogType.Warning:
                    GUILayout.Label(logs[i].condition, warning);
                    break;
                default:
                    GUILayout.Label(logs[i].condition, normal);
                    break;
            }
        }
        GUILayout.EndScrollView();
    }

    private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
    {
        logs.Add(new LogMessage(">>" + condition, stackTrace, type));
    }
}

public struct LogMessage
{
    public string condition;
    public string trace;
    public LogType type;

    public LogMessage(string condition, string trace, LogType type)
    {
        this.condition = condition;
        this.trace = trace;
        this.type = type;
    }
}