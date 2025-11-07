using System.Collections;
using br.com.bonus630.thefrog.Manager;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace br.com.bonus630.thefrog.UI
{
    public class VolumMenu : MonoBehaviour
    {
        [SerializeField] Slider soundSlider;
        [SerializeField] Slider musicSlider;
        [SerializeField] AudioMixer mixer;
        float soundVol = -80f;
        float musicVol = -80f;

        void Start()
        {
            StartCoroutine(WaitLoad());
        }
        private IEnumerator WaitLoad()
        {
            yield return new WaitUntil(() => GameManager.Instance != null); // vamos esperar a instancia estatica do gamemanager ficar pronta
            LoadVolum();
            soundSlider.onValueChanged.AddListener(SaveVolum);
            musicSlider.onValueChanged.AddListener(SaveVolum);
        }
        private void OnDisable()
        {
            soundSlider.onValueChanged.RemoveListener(SaveVolum);
            musicSlider.onValueChanged.RemoveListener(SaveVolum);
            GameManager.Instance.SaveVolum(soundVol, musicVol);

        }

        private void LoadVolum()
        {

            GameManager.Instance.LoadVolum(out soundVol, out musicVol); 
            soundSlider.value = Mathf.Pow(10, soundVol / 20);
            musicSlider.value = Mathf.Pow(10, musicVol / 20);
            updateMixer();
        }
        private void SaveVolum(float val)
        {
            soundVol = Mathf.Log10(Mathf.Clamp(soundSlider.value, 0.001f, 1f)) * 20;
            musicVol = Mathf.Log10(Mathf.Clamp(musicSlider.value, 0.001f, 1f)) * 20;
            updateMixer();
        }
        private void updateMixer()
        {
            mixer.SetFloat("MusicVolume", musicVol);
            mixer.SetFloat("SFXVolume", soundVol);
        }
    }
}
