using System.Collections;
using br.com.bonus630.thefrog.Shared;
using br.com.bonus630.thefrog.Utils;
using UnityEngine;
using UnityEngine.InputSystem;


namespace br.com.bonus630.thefrog.Manager
{
    public class ScreenEffects : MonoBehaviour
    {
        [field:SerializeField]public ScreenFader screenFader { get; private set; }
        [field:SerializeField]public CamerasController camerasController { get; private set; }
        [field:SerializeField]public VignetteManager vignetteManager { get; private set; }


        private void Awake()
        {
            if(screenFader==null)
                screenFader = FindAnyObjectByType<ScreenFader>();
        }
        private void Start()
        {
            ServiceLocator.Instance.Register<ScreenEffects>(this);
        }
        public void GameObjectFocus(GameObject gameObject, float time = 1)
        {
            camerasController.GameObjectFocus(gameObject, time);
        }
        public void GameObjectsFocus(GameObject[] gameObjects, float time = 1)
        {
            camerasController.GameObjectsFocus(gameObjects, time);
        }
        public void ScreenAndGamepadShake(int times = 1)
        {
            camerasController.ShakeCameraAndGamepadEffect(times,true);
        }
        public void ScreenShake(int times = 1)
        {
            camerasController.ShakeCameraAndGamepadEffect(times,false);
        }
        CameraShakeController shakeController;
        public void StartCameraShake(float amplitude, float frequency)
        {
            if (shakeController != null)
                shakeController.StopShake();
           // shakeController = new CameraShakeController(camerasController.GetActiveVirtualCamera());
            shakeController = new CameraShakeController(CameraUtils.GetActiveVirtualCamera2());
            shakeController.StartShake(amplitude, frequency);
            
        }
        public void StopCameraShake()
        {
            if (shakeController != null)
                shakeController.StopShake();
        }
        public void GamepadShake(float low = 0f, float hi = 0f)
        {
            if (Gamepad.current != null)
                Gamepad.current.SetMotorSpeeds(low, hi);
        }
        public void FadeOut(float duration = 1f)
        {
            screenFader.fadeDuration = duration;
            StartCoroutine(screenFader.FadeOut());
        }
        public void FadeIn(float duration = 1f)
        {
            screenFader.fadeDuration = duration;
            StartCoroutine(screenFader.FadeIn());
        }
        //public Coroutine FadeOut(float duration = 1f)
        //{
        //    screenFader.fadeDuration = duration;
        //    return StartCoroutine(screenFader.FadeOut());
        //}
        //public Coroutine FadeIn(float duration = 1f)
        //{
        //    screenFader.fadeDuration = duration;
        //    return StartCoroutine(screenFader.FadeIn());
        //}
        public void CameraOffSet(Vector2 offsetXY)
        {
            camerasController.SetOffSet(offsetXY);
        }
        public void FashVignettePlayerDamage()
        {
            vignetteManager.FashVignette(0f, 0.6f, 1f, Color.red);
        }
        void OnDisable()
        {
            if (Gamepad.current != null)
                Gamepad.current.SetMotorSpeeds(0f, 0f);
        }
    }
}
