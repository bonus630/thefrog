using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
namespace br.com.bonus630.thefrog.Manager
{
    public class MusicSource : MonoBehaviour
    {
        [SerializeField] AudioSource audioLeft;
        [SerializeField] AudioSource audioRight;
        [SerializeField] AudioMixer mixer;

        [SerializeField] private AudioClip[] BackgroundMusics;
        [SerializeField] private AudioClip[] BackgroundMusicsRandom;

        private float fadDuration = 10f;
        private float targetVolume = 0.36f;
        private float silentTime;
        private bool leftTurn = true;

        private float savedTime = 0f; // Guarda o tempo da música antes de pausar

    
        /// <summary>
        /// Global volume
        /// </summary>
        /// <param name="vol">-80 a 80</param>
        public void SetMasterVolume(float vol)
        {
            mixer.SetFloat("MasterVolume", vol);
        }
        /// <summary>
        /// Music volume
        /// </summary>
        /// <param name="vol">-80 a 80</param>
        public void SetMusicVolume(float vol)
        {
            mixer.SetFloat("MusicVolume", vol);
        }
        /// <summary>
        /// Music volume
        /// </summary>
        /// <param name="vol">-80 a 80</param>
        public void SetSFXVolume(float vol)
        {
            mixer.SetFloat("SFXVolume", vol);
        }

        private void Update()
        {
            if (sleep)
                return;
            if (!audioLeft.isPlaying && !audioRight.isPlaying)
                silentTime += Time.deltaTime;
            else
                silentTime = 0;

            if (silentTime > 2f)
            {
                audioLeft.loop = false;
                audioRight.loop = false;
                //PlayFadIn(BackgroundMusicsRandom[Random.Range(0, BackgroundMusicsRandom.Length)]);
                CrossFade(BackgroundMusicsRandom[Random.Range(0, BackgroundMusicsRandom.Length)]);
            }
            if (leftTurn)
            {
                StartNewMusic(audioRight, audioLeft);
            }
            else
            {
                StartNewMusic(audioLeft, audioRight);
            }
            //   Debug.Log("Left Time:" + (audioLeft.clip.length - audioLeft.time));
        }
        private void StartNewMusic(AudioSource current, AudioSource next)
        {
            if (current.clip == null)
                return;
            if (current.clip.length - fadDuration - current.time <= 0 && !next.isPlaying)
            {
                CrossFade(BackgroundMusicsRandom[Random.Range(0, BackgroundMusicsRandom.Length)]);
            }
        }
        //private void PlayFadIn()
        //{
        //    StopAllCoroutines();
        //    audioLeft.volume = 0f;
        //    audioRight.volume = 0f;
        //    audioLeft.Play();
        //    audioRight.Play();
        //    //StartCoroutine(FadIn());
        //}
        private void CrossFade(AudioClip clip, bool disableLoop = true)
        {
            if (disableLoop)
            {
                audioLeft.loop = false;
                audioRight.loop = false;
            }
            if (leftTurn)
            {
                PlayFadIn(new AudioSource[] { audioLeft }, clip);
                //StopFadOut(new AudioSource[] { audioRight });
                StartCoroutine(WaitToPlay(audioLeft, audioRight));
                //audioRight.clip = audioLeft.clip;
                //audioRight.volume = targetVolume;
                //audioRight.time = audioLeft.time;
                //audioRight.Play();
            }
            else
            {
                PlayFadIn(new AudioSource[] { audioRight }, clip);
                StartCoroutine(WaitToPlay(audioRight, audioLeft));

                //StopFadOut(new AudioSource[] { audioLeft });
                //audioLeft.clip = audioRight.clip;
                //audioLeft.volume = targetVolume;
                //audioLeft.time = audioRight.time;
                //audioLeft.Play();
            }
            leftTurn = !leftTurn;
        }
        public void CrossFade(BackgroundMusic music)
        {
            if (leftTurn)
                audioLeft.loop = true;
            else
                audioRight.loop = true;
            CrossFade(BackgroundMusics[(int)music], false);
        }

        IEnumerator WaitToNext(float delay)
        {
            yield return new WaitForSeconds(delay - fadDuration);
            PlayFadIn(BackgroundMusicsRandom[Random.Range(0, BackgroundMusicsRandom.Length)]);
        }

        private IEnumerator WaitToPlay(AudioSource toPlay, AudioSource nowPlaying)
        {
            while (toPlay.volume < targetVolume / 2)
            {
                yield return null;
            }
            StopFadOut(new AudioSource[] { nowPlaying });
            //Debug.Log("Estamos no audio:" + toPlay.time);
        }
        public void PlayFadIn(AudioClip clip)
        {
            // Debug.Log("Audio");
            PlayFadIn(new AudioSource[] { audioLeft, audioRight }, clip);
        }

        public void PlayFadIn(BackgroundMusic music)
        {
            AudioClip clip = BackgroundMusics[(int)music];
            PlayFadIn(new AudioSource[] { audioLeft, audioRight }, clip);
        }

        private void PlayFadIn(AudioSource[] channels, AudioClip clip)
        {
            // StopAllCoroutines();
            StartCoroutine(FadIn(channels, clip));
        }


        private IEnumerator FadIn(AudioSource[] channels, AudioClip clip)
        {
            sleep = false;
            if (!clip.loadState.Equals(AudioDataLoadState.Loaded))
            {
                clip.LoadAudioData();
                while (clip.loadState != AudioDataLoadState.Loaded)
                    yield return null;
            }
            foreach (var a in channels)
            {
                a.clip = clip;
                a.volume = 0f;
                a.Play();
            }
            float currentTime = 0f;
            while (currentTime < fadDuration)
            {
                currentTime += Time.deltaTime; // Corrigido: remoção da divisão por 100
                float progress = currentTime / fadDuration; // Corrigido: cálculo de progresso com base em fadDuration

                for (int i = 0; i < channels.Length; i++)
                    channels[i].volume = Mathf.Lerp(0, targetVolume, progress);

                yield return null;
            }

            for (int i = 0; i < channels.Length; i++)
                channels[i].volume = targetVolume; // Corrigido: channels[1] → channels[i]
        }
        public void Play(BackgroundMusic music)
        {
            AudioClip clip = BackgroundMusics[(int)music];
            if (leftTurn)
            {
                audioRight.Stop();
                Play(audioLeft, clip);
            }
            else
            {
                audioLeft.Stop();
                Play(audioRight, clip);
            }
            leftTurn = !leftTurn;
        }
        private void Play(AudioSource audio, AudioClip clip)
        {
            audio.volume = targetVolume;
            audio.clip = clip;
            audio.time = 0;
            audio.Play();
        }
        public void StopAll()
        {
            StopAllCoroutines();
            audioLeft.Stop();
            audioRight.Stop();
        }
        private bool sleep = false;
        public void Sleep()
        {
            sleep = true;
            StopAll();
        }
        public void WakeUp()
        {
            sleep = false;
        }
        public void StopFadOut(AudioSource[] channels)
        {
            // StopAllCoroutines();
            StartCoroutine(FadOut(channels));
        }

        private IEnumerator FadOut(AudioSource[] channels)
        {
            float currentTime = 0f;
            while (currentTime < fadDuration)
            {
                currentTime += Time.deltaTime; // Corrigido: mesma lógica do FadIn
                float progress = currentTime / fadDuration;

                for (int i = 0; i < channels.Length; i++)
                    channels[i].volume = Mathf.Lerp(targetVolume, 0, progress);

                yield return null;
            }

            for (int i = 0; i < channels.Length; i++)
                channels[i].Stop(); // Corrigido: channels[1] → channels[i]
        }

        public void PauseMainMusic()
        {
            if (audioLeft.isPlaying)
            {
                savedTime = audioLeft.time; // Salva o ponto atual da música
                audioLeft.Pause();
                audioRight.Pause();
            }
        }

        public void ResumeMainMusic()
        {
            audioLeft.time = savedTime;
            audioRight.time = savedTime;
            audioLeft.Play();
            audioRight.Play();
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
        AppleTree = 6,
        Gravity = 7,
        Ship = 8,
        MiniTour
    }

}