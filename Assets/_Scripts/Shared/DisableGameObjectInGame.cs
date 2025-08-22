using UnityEngine;

namespace br.com.bonus630.thefrog.Shared
{
    public class DisableGameObjectInGame : MonoBehaviour
    {
        private void Awake()
        {
#if !UNITY_EDITOR
            gameObject.SetActive(false);
#endif
        }

    }
}
