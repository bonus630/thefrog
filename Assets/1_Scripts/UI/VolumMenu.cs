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
        AudioSource audioSource;

        void Start()
        {
            StartCoroutine(WaitLoad());
            audioSource = GetComponent<AudioSource>();
        }
        private IEnumerator WaitLoad()
        {
            yield return new WaitUntil(() => GameManager.Instance != null); // vamos esperar a instancia estatica do gamemanager ficar pronta
            LoadVolum();
            soundSlider.onValueChanged.AddListener(soundVolum);
            musicSlider.onValueChanged.AddListener(musicVolum);
        }
        private void OnDisable()
        {
            soundSlider.onValueChanged.RemoveListener(soundVolum);
            musicSlider.onValueChanged.RemoveListener(musicVolum);
            GameManager.Instance.SaveVolum(soundVol, musicVol);

        }

        private void LoadVolum()
        {

            GameManager.Instance.LoadVolum(out soundVol, out musicVol);
            soundSlider.value = Mathf.Pow(10, soundVol / 20);
            musicSlider.value = Mathf.Pow(10, musicVol / 20);
           
            updateMixer();
        }
        private void soundVolum(float val)
        {
            soundVol = Mathf.Log10(Mathf.Clamp(soundSlider.value, 0.001f, 1f)) * 20;
            audioSource.Play();
            updateMixer();
        }
        private void musicVolum(float val)
        {
            musicVol = Mathf.Log10(Mathf.Clamp(musicSlider.value, 0.001f, 1f)) * 20;
            updateMixer();
        }
        private void updateMixer()
        {
            Debug.Log("[VolumMenu] musicVol:" + musicVol);
            mixer.SetFloat("MusicVolume", musicVol);
            mixer.SetFloat("SFXVolume", soundVol);
        }
    }
}
