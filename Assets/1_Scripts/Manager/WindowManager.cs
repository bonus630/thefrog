#if UNITY_STANDALONE_WIN
using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace br.com.bonus630.thefrog.Assets.Scripts.Manager
{


    public class WindowCloseInterceptor : MonoBehaviour
    {
        const int WM_CLOSE = 0x0010;
        const int GWL_WNDPROC = -4;

        private delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        private static WndProcDelegate newWndProc = WndProc;
        private static IntPtr oldWndProc = IntPtr.Zero;

        private static bool initialized = false;
        private static WindowCloseInterceptor instance;

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, WndProcDelegate newProc);

        [DllImport("user32.dll")]
        private static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr GetActiveWindow();

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);

            if (!initialized)
            {
                initialized = true;
                var hwnd = GetActiveWindow();
                oldWndProc = SetWindowLongPtr(hwnd, GWL_WNDPROC, newWndProc);
            }
        }

        private static IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            if (msg == WM_CLOSE)
            {
                Debug.Log("Interceptado clique no botão X da janela!");
                Application.Quit(); // fallback se não encontrar

                return IntPtr.Zero;
            }

            return CallWindowProc(oldWndProc, hWnd, msg, wParam, lParam);
        }
    }
}
#endif


