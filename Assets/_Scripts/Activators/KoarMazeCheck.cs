using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace br.com.bonus630.thefrog.Activators
{
    public class KoarMazeCheck : MonoBehaviour
    {
        [SerializeField] MusicSource musicSource;
        private void Awake()
        {
            if (DataScenePreserver.Instance.Contains("MAZE"))
                if (DataScenePreserver.Instance.Get<ListStorage<int>>("MAZE").Flag)
                {
                    FindAnyObjectByType<CameraBackground>().ChangeBackground();
                    GameObject.Find("Global Light 2D").GetComponent<Light2D>().intensity = 0.2f;
                    transform.GetChild(0).gameObject.SetActive(true);
                    musicSource.Sleep();
                    musicSource.CrossFade(BackgroundMusic.DarkWind, true);
                }
        }
    }
}
