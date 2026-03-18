using UnityEngine;
using UnityEngine.SceneManagement;

namespace br.com.bonus630.thefrog.Manager
{
    public class MoveToMainScene : MonoBehaviour
    {
        void Start()
        {
            while(gameObject.transform.parent !=null)
                gameObject.transform.parent = null;
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName("Main"));
        }
    }
}
