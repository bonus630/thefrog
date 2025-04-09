using System.Collections;
using UnityEngine;
namespace br.com.bonus630.thefrog.Manager
{
    public class MusicSource : MonoBehaviour
    {
        [SerializeField] AudioSource audioLeft;
        [SerializeField] AudioSource audioRight;

        [SerializeField] private AudioClip[] BackgroundMusics;
        [SerializeField] private AudioClip[] BackgroundMusicsRandom;

        private float fadDuration = 2f;
        private float targetVolume = 0.3f;
        private float silentTime;


        private void Awake()
        {
            //audioSourceRight = GetComponent<AudioSource>();

        }
        private void Update()
        {
            if (!audioLeft.isPlaying && !audioRight.isPlaying)
                silentTime += Time.deltaTime;
            else
            {
                silentTime = 0;
            }
            if (silentTime > 2f)
            {
                audioLeft.loop = false;
                audioRight.loop = false;
                PlayFadIn(BackgroundMusicsRandom[Random.Range(0, BackgroundMusicsRandom.Length)]);
            }
        }
        private void PlayFadIn()
        {
            StopAllCoroutines();
            audioLeft.volume = 0f;
            audioRight.volume = 0f;
            audioLeft.Play();
            audioRight.Play();
            //StartCoroutine(FadIn());
        }
        public void PlayFadIn(AudioClip clip)
        {

            audioLeft.resource = clip;
            audioRight.resource = clip;
            PlayFadIn(new AudioSource[] { audioLeft, audioRight });
        }
        public void PlayFadIn(BackgroundMusic music)
        {
            audioLeft.loop = true;
            audioLeft.resource = BackgroundMusics[(int)music];
            audioRight.loop = true;
            audioRight.resource = BackgroundMusics[(int)music];
            PlayFadIn(new AudioSource[] { audioLeft, audioRight });
        }
        private void PlayFadIn(AudioSource[] channels)
        {
            StopAllCoroutines();
            audioLeft.volume = 0f;
            audioRight.volume = 0f;
            audioLeft.Play();
            audioRight.Play();
            StartCoroutine(FadIn(channels));

        }
        public void Stop()
        {
            audioLeft.Stop();
            audioRight.Stop();

        }
        public void StopFadOut(AudioSource[] channels)
        {
            StopAllCoroutines();
            StartCoroutine(FadOut(channels));
            
        }
        private IEnumerator FadIn(AudioSource[] channels)
        {
            float currentTime = 0f;
            while (currentTime < fadDuration)
            {
                currentTime += Time.deltaTime / 100;
                for (int i = 0; i < channels.Length; i++)
                    channels[i].volume = Mathf.Lerp(0, targetVolume, currentTime / targetVolume);

                yield return null;
            }
            for (int i = 0; i < channels.Length; i++)
                channels[1].volume = targetVolume;

        }
        private IEnumerator FadOut(AudioSource[] channels)
        {
            float currentTime = 0f;
            while (currentTime < fadDuration)
            {
                currentTime += Time.deltaTime / 100;
                for (int i = 0; i < channels.Length; i++)
                    channels[i].volume = Mathf.Lerp(targetVolume, 0, currentTime / targetVolume);
                // Debug.Log("Lerp:" + Mathf.Lerp(targetVolume, 0, currentTime / targetVolume));
                yield return null;
            }
            for (int i = 0; i < channels.Length; i++)
                channels[1].Stop();
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