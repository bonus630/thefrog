using UnityEngine;

namespace br.com.bonus630.thefrog.Debuggers
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class SceneWatchdog : MonoBehaviour
    {
        void Awake()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.LogError($"[SceneWatchdog] Cena recarregada: {scene.name} — modo: {mode}");
        }

        void OnDestroy()
        {
            Debug.LogError($"[SceneWatchdog] O próprio Watchdog foi destruído! Cena atual: {gameObject.scene.name}");
        }
    }

}
