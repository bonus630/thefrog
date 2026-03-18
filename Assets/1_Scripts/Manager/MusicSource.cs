using System.Collections;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;
using UnityEngine.Audio;
namespace br.com.bonus630.thefrog.Manager
{
    public class MusicSource : MonoBehaviour, IService
    {
        [SerializeField] AudioSource audioLeft;
        [SerializeField] AudioSource audioRight;
        [SerializeField] AudioSource audioAux;


        [SerializeField] AudioMixer mixer;

        [SerializeField] private AudioClip[] BackgroundMusics;
        [SerializeField] private AudioClip[] BackgroundMusicsRandom;


        [field: SerializeField] public float MaxSilentTime { get; set; } = 1f;

        private float fadDuration = 10f;
        private float targetVolume = 0.32f;
        private float silentTime;

        private bool leftTurn = true;

        [SerializeField] private float normalPitch = 1f;
        [SerializeField] private float dangerPitch = 1.25f;
        [SerializeField] private float pitchFadeTime = 1.5f;
        private Coroutine pitchCoroutine;


        public float SavedTime { get; set; } = 0f; // Guarda o tempo da música antes de pausar
        public bool SavedLoop { get; private set; } = false; //Guarda o loop

        private AudioClip savedClip;


        private void Start()
        {
            ServiceLocator.Instance.Register<MusicSource>(this);
        }

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

        private void LateUpdate()
        {
            if (IsSleeping)
                return;
            if (!audioLeft.isPlaying && !audioRight.isPlaying)
                silentTime += Time.deltaTime;
            else
                silentTime = 0;

            if (silentTime > MaxSilentTime)
            {
                audioLeft.loop = false;
                audioRight.loop = false;
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
            //Debug.Log("[MusicSource][LateUpdate]Left Time:" + (audioLeft.clip.length - audioLeft.time));
        }
        private void StartNewMusic(AudioSource current, AudioSource next)
        {
            if (current.clip == null )
                return;
            if (current.clip.length - fadDuration - current.time <= 0 && !next.isPlaying)
            {
                //Debug.Log("[MusicSource][StartNewMusic]Left Time:" + (current.clip.length - fadDuration - current.time));
               // Debug.Log("[MusicSource][StartNewMusic] next.isPlaying:" + next.isPlaying);
                CrossFade(BackgroundMusicsRandom[Random.Range(0, BackgroundMusicsRandom.Length)]);
            }
        }
        //private void StartNewMusic(AudioSource current, AudioSource next)
        //{
        //    if (current.clip == null ||
        //        !current.isPlaying ||
        //        current.clip.loadState != AudioDataLoadState.Loaded)
        //        return;

        //    // Proteção extra para streaming
        //    if (current.timeSamples <= 0)
        //        return;

        //    float remaining =
        //        (current.clip.samples - current.timeSamples)
        //        / (float)current.clip.frequency;

        //    if (remaining <= fadDuration && !next.isPlaying)
        //    {
        //        CrossFade(
        //            BackgroundMusicsRandom[
        //                Random.Range(0, BackgroundMusicsRandom.Length)
        //            ]);
        //    }
        //}
        private void CrossFade(AudioClip clip, bool disableLoop = true)
        {
            if (disableLoop)
            {
                WakeUp();
            }
            if (leftTurn)
            {
                PlayFadIn(new AudioSource[] { audioLeft }, clip);
                StartCoroutine(WaitToPlay(audioLeft, audioRight));
            }
            else
            {
                PlayFadIn(new AudioSource[] { audioRight }, clip);
                StartCoroutine(WaitToPlay(audioRight, audioLeft));
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
        public void CrossFade(BackgroundMusic music, bool inLoop)
        {
            if (leftTurn)
                audioLeft.loop = inLoop;
            else
                audioRight.loop = inLoop;
            CrossFade(BackgroundMusics[(int)music], !inLoop);
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
            //Debug.Log("[MusicSource][WaitToPlay]Estamos no audio:" + toPlay.time);
        }
        public void PlayFadIn(AudioClip clip)
        {
             Debug.Log("[MusicSource][PlayFadIn]Audio");
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
            IsSleeping = false;
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
        /// <summary>
        /// Utilize este método para tocar uma música das musicas pre definidas
        /// </summary>
        /// <param name="music"></param>
        /// <param name="inLoop">Se passar verdadeiro, método wakeup deve ser chamado para continuar o fluxo do musicsource</param>
        public void Play(BackgroundMusic music, bool inLoop = false)
        {
            StopAllCoroutines();
            IsSleeping = true;
            AudioClip clip = BackgroundMusics[(int)music];
            if (leftTurn)
            {
                audioRight.Stop();
                audioLeft.loop = inLoop;
                Play(audioLeft, clip);
            }
            else
            {
                audioLeft.Stop();
                audioRight.loop = inLoop;
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
        public bool IsSleeping { get; set; } = false;
        
        /// <summary>
        /// Faz o MusicSource deixar de controlar o fluxo das faixas de musica, e para todas músicas em execução
        /// </summary>
        public void Sleep()
        {
            IsSleeping = true;
            StopAll();
        }
        /// <summary>
        /// Reabilita o MusicSource para controlar o fluxo das musicas, todos loops são desativados
        /// </summary>
        public void WakeUp()
        {
            IsSleeping = false;
            audioLeft.loop = false;
            audioRight.loop = false;
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

        public void InstantPlay(BackgroundMusic music, bool loop = false)
        {
            Debug.Log("[MusicSource][InstantPlay]music:" + music);
            AudioClip clip = BackgroundMusics[(int)music];
            InstantPlay(clip, loop);
        }
        public void InstantPlay(AudioClip clip, bool loop = false)
        {
            if (loop)
                Sleep();
            StopAllCoroutines();
            AudioSource active = leftTurn ? audioLeft : audioRight;
            AudioSource inactive = leftTurn ? audioRight : audioLeft;

            inactive.Stop();

            active.clip = clip;
            active.time = SavedTime;
            active.loop = loop;
            active.volume = targetVolume; // ou targetVolume se tiver
            active.Play();

            leftTurn = !leftTurn;
        }
        public bool IsPlaying(AudioClip clip)
        {
            if (audioLeft.isPlaying && audioLeft.clip == clip)
                return true;
            if (audioRight.isPlaying && audioRight.clip == clip)
                return true;
            return false;
        }
        public bool IsPlaying(BackgroundMusic music) => IsPlaying(BackgroundMusics[(int)music]);

        public void PauseMainMusic()
        {
            SavePosition();
            audioLeft.Pause();
            audioRight.Pause();
        }
        private void SavePosition()
        {

            if (audioLeft.isPlaying)
            {
                SavedTime = audioLeft.time;
                SavedLoop = audioLeft.loop;
                savedClip = audioLeft.clip;
            }
            else if (audioRight.isPlaying)
            {
                SavedTime = audioRight.time;
                SavedLoop = audioRight.loop;
                savedClip = audioRight.clip;
            }
        }

        public void ResumeMainMusic()
        {
            if (audioLeft.clip != null)
            {
                audioLeft.clip = savedClip;
                audioLeft.time = SavedTime;
                audioLeft.loop = SavedLoop;
                audioLeft.Play();
            }

            if (audioRight.clip != null)
            {
                audioRight.clip = savedClip;
                audioRight.time = SavedTime;
                audioRight.loop = SavedLoop;
                audioRight.Play();
            }
        }
        public void PreserveMusic(string key)
        {
            SavePosition();

            var musicData = new MusicData()
            {
                Clip = savedClip,
                Time = SavedTime,
                Loop = SavedLoop
            };

            DataScenePreserver.Instance.Set(key, musicData);
        }

        public void RestoreMusic(string key)
        {
            if (!DataScenePreserver.Instance.Contains(key))
                return;

            var musicData = DataScenePreserver.Instance.Get<MusicData>(key);
            //Debug.Log($"[MusicData] clip:{musicData.Clip}, Time:{musicData.Time}, Loop:{musicData.Loop}");
            if (musicData.Clip != null)
            {
                InstantPlay(musicData.Clip, musicData.Loop);
                savedClip = musicData.Clip;
                SavedTime = musicData.Time;
                SavedLoop = musicData.Loop;
                ResumeMainMusic();
            }
        }
        public void BoostDangerMusic()
        {
            SetPitch(dangerPitch);
        }

        public void RestoreNormalMusic()
        {
            SetPitch(normalPitch);
        }
    
        public void SetPitch(float target)
        {
            if (pitchCoroutine != null)
                StopCoroutine(pitchCoroutine);

            pitchCoroutine = StartCoroutine(FadePitch(target));
        }

        private IEnumerator FadePitch(float target)
        {
            float startLeft = audioLeft.pitch;
            float startRight = audioRight.pitch;

            float elapsed = 0f;

            while (elapsed < pitchFadeTime)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / pitchFadeTime;

                audioLeft.pitch = Mathf.Lerp(startLeft, target, t);
                audioRight.pitch = Mathf.Lerp(startRight, target, t);

                yield return null;
            }

            audioLeft.pitch = target;
            audioRight.pitch = target;
        }
        public void PlayClip(AudioClip clip, int times = 1)
        {
            audioAux.PlayOneShot(clip);
        }

    }
    public class MusicData
    {
        public MusicData()
        {

        }
        public MusicData(BackgroundMusic music, float time, bool loop = false)
        {
            Music = music;
            Time = time;
            Loop = loop;
        }
        public MusicData(AudioClip clip, float time, bool loop = false)
        {
            Clip = clip;
            Time = time;
            Loop = loop;
        }
        public AudioClip Clip { get; set; }

        public BackgroundMusic Music { get; set; }
        public float Time { get; set; }
        public bool Loop { get; set; }

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
        MiniTour = 9,
        DarkWind = 10,
        GoodDayToDie = 11,
        Ignition = 12,
        lament =13,
        KoarCastle = 14,
        WizardBoss = 15
            
    }

}