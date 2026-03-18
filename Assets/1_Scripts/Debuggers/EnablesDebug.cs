using UnityEngine;
using System.Diagnostics;

namespace br.com.bonus630.thefrog.Debuggers
{

    public class EnablesDebug : MonoBehaviour
    {
        private void OnEnable()
        {
            LogCaller("ativado");
        }

        private void OnDisable()
        {
            LogCaller("desativado");
        }

        private void LogCaller(string action)
        {
            // Captura a stack trace
            StackTrace trace = new StackTrace(true);
            // Pega os primeiros 5 frames para não poluir muito
            string stackInfo = "";
            for (int i = 1; i < Mathf.Min(trace.FrameCount, 6); i++)
            {
                var frame = trace.GetFrame(i);
                stackInfo += frame.GetMethod().DeclaringType + "." + frame.GetMethod().Name + " -> ";
            }

            UnityEngine.Debug.Log($"[{gameObject.name}] {action} pelo StackTrace: {stackInfo}");
        }
    }

}
