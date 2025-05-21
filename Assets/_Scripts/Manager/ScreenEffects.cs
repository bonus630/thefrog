using UnityEngine;

namespace br.com.bonus630.thefrog.Manager
{
    public class ScreenEffects : MonoBehaviour
    {
        [field:SerializeField]public ScreenFader screenFader { get; private set; }
        [field:SerializeField]public CamerasController camerasController { get; private set; }
        public void ScreenShake()
        {
            camerasController.ShakeCameraEffect();
        }
        public void FadeOut(float duration = 1f)
        {
            screenFader.fadeDuration = duration;
            screenFader.FadeOut();
        }
        public void FadeIn(float duration = 1f)
        {
            screenFader.fadeDuration = duration;
            screenFader.FadeIn();
        }
    }
}
