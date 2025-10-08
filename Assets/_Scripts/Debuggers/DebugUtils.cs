using UnityEngine;
using System.Runtime.CompilerServices;


namespace br.com.bonus630.thefrog.Debuggers
{
    public static class DebugUtils
    {
        public static void Log(string message,
            [CallerMemberName] string methodName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            string className = System.IO.Path.GetFileNameWithoutExtension(filePath);
            Debug.Log($"[{className}.{methodName} (linha {lineNumber})] {message}");
        }

        public static void LogWarning(string message,
            [CallerMemberName] string methodName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            string className = System.IO.Path.GetFileNameWithoutExtension(filePath);
            Debug.LogWarning($"[{className}.{methodName} (linha {lineNumber})] {message}");
        }

        public static void LogError(string message,
            [CallerMemberName] string methodName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            string className = System.IO.Path.GetFileNameWithoutExtension(filePath);
            Debug.LogError($"[{className}.{methodName} (linha {lineNumber})] {message}");
        }
    }
}
