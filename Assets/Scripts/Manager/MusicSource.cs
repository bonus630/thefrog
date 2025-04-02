using System.Collections;
using UnityEngine;
namespace br.com.bonus630.thefrog.Manager
{
    public class MusicSource : MonoBehaviour
    {
        private float fadDuration = 2f;
        private float targetVolume = 0.3f;

        [SerializeField] private AudioClip[] BackgroundMusics;
        [SerializeField] private AudioClip[] BackgroundMusicsRandom;

        private float silentTime;


        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();

        }
        private void Update()
        {
            if (!audioSource.isPlaying)
                silentTime += Time.deltaTime;
            else
            {
                silentTime = 0;
            }
            if (silentTime > 2f)
            {
                PlayFadIn(BackgroundMusicsRandom[Random.Range(0, BackgroundMusicsRandom.Length)]);
            }
        }
        public void PlayFadIn()
        {
            StopAllCoroutines();
            audioSource.volume = 0f;
            audioSource.Play();
            StartCoroutine(FadIn());
        }
        public void PlayFadIn(AudioClip clip)
        {
            audioSource.resource = clip;
            PlayFadIn();
        }
        public void PlayFadIn(BackgroundMusic music)
        {

            audioSource.resource = BackgroundMusics[(int)music];
            PlayFadIn();
        }
        public void Stop()
        {
            audioSource.Stop();

        }
        public void StopFadOut()
        {
            StopAllCoroutines();
            StartCoroutine(FadOut());
        }
        IEnumerator FadIn()
        {
            float currentTime = 0f;
            while (currentTime < fadDuration)
            {
                currentTime += Time.deltaTime / 100;
                audioSource.volume = Mathf.Lerp(0, targetVolume, currentTime / targetVolume);
                yield return null;
            }
            audioSource.volume = targetVolume;
        }
        IEnumerator FadOut()
        {
            float currentTime = 0f;
            while (currentTime < fadDuration || audioSource.volume > 0)
            {
                currentTime += Time.deltaTime / 100;
                audioSource.volume = Mathf.Lerp(targetVolume, 0, currentTime / targetVolume);
                Debug.Log("Lerp:" + Mathf.Lerp(targetVolume, 0, currentTime / targetVolume));
                yield return null;
            }
            audioSource.Stop();
        }


    }
    public enum BackgroundMusic
    {
        AdventureStarts = 0,
        CheckPoint1 = 1,
        CheckPoint2 = 2,
        PigIsDefead = 3,
        DuckPath = 4,
        AlyskyIntro = 5,
        AppleTree = 6
    }
}