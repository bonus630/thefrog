using UnityEngine;

namespace br.com.bonus630.thefrog.Utils
{
    public class SpawnerDetector : MonoBehaviour
    {
      
            void Awake()
            {
                Debug.Log($"Spawnado: {name}", this);
            }
       
    }
}
