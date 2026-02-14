using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
//using br.com.bonus630.thefrog.Activators;
//using br.com.bonus630.thefrog.Caracters;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using br.com.bonus630.thefrog.Shared;
using br.com.bonus630.thefrog.Utils;
using UnityEngine.UI;
using br.com.bonus630.thefrog.Debuggers;
using System.Runtime.CompilerServices;

namespace br.com.bonus630.thefrog.Manager
{
    [DefaultExecutionOrder(-100), RequireComponent(typeof(EventsManager))]
    public class GameManager : MonoBehaviour
    {
        [SerializeField] InputAction PauseAction;

        private TextMeshProUGUI scoreText;
        private TextMeshProUGUI TimerText = null;
        public event Action TimeOverEvent;
        public event Action GameStatesRestaured;
        public event Action<string, bool> ActiveItemChanged;
        private bool continueGame = false;
        private bool _isCountingTime = false;
        private bool gamePaused = false;
        float startTimer = 0;
        public float PlayTimeInSeconds { get; private set; }
        public int ToPoint { get; set; }
        private PlayerStates playerStates;
        public PlayerStates PlayerStates { get { return playerStates; } set { playerStates = value; } }

        private EnvironmentStates environmentStates;
        public EnvironmentStates EnvironmentStates { get { return environmentStates; } private set { environmentStates = value; } }
        [SerializeField] private GameObject player;
        public GameObject GetPlayer { get { if (player == null) player = GameObject.Find("Player"); return player; } }
        [SerializeField] private ScreenEffects screenEffects;
        public ScreenEffects ScreenEffects { get { if (screenEffects == null) screenEffects = FindAnyObjectByType<ScreenEffects>(); return screenEffects; } }
        public static GameManager Instance;
        public EventsManager eventManager;

        public Vector3 StartGamePosition { get; private set; }
        public Vector3 PlayerStartPosition { get; set; }
        public IPlayer GetPlayerScript { get { return GetPlayer.GetComponent<IPlayer>(); } }

        public bool GamePaused { get => gamePaused; private set => gamePaused = value; }

        private float musicVolum = 0;
        private float soundVolum = 0;


        //Scenes Names
        //public readonly string MainScene = "  ";
        public readonly string MainScene = "Main";
        public readonly string InternAreas = "InternAreas";
        public readonly string FroggerScene = "Frogger";
        public readonly string GameOverScene = "GameOver";
        public readonly string MainMenu = "MainMenu";

        //GameObjects Names
        public readonly string StartPointBuilder = "StartPointBuilder";
        public readonly string ToSkyPoint = "ToSkyPoint";
        public readonly string CameraContainer = "CameraContainer";

        //HUD Names
        public readonly string CollecteblesHUD = "CollecteblesHUD";
        public readonly string ShurykenHUD = "ShurykenHUD";
        public readonly string HeartContainerHUD = "HeartContainerHUD";
        public readonly string HeartHUD = "HeartHUD";
        public readonly string SkillsHUD = "SkillsHUD";
        public readonly string PauseHUD = "PauseHUD";
        public readonly string SaveHUD = "SaveHUD";
        public readonly string TimerHUD = "TimerHUD";
        public readonly string SpiritHUD = "SpiritHUD";

        //Pref Keys
        private readonly string MusicVolum = "MusicVolum";
        private readonly string SoundVolum = "SoundVolum";

        //Env Names


        //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        //static void InitOnLoad()
        //{
        //    if (Instance == null)
        //    {
        //        GameObject prefab = Resources.Load<GameObject>("GameManager");
        //        if (prefab != null)
        //        {
        //            GameObject obj = Instantiate(prefab);
        //            DontDestroyOnLoad(obj);
        //        }
        //    }
        //}


        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            playerStates = new PlayerStates(new PlayerPosition(gameObject.transform.position), new Datas(), new Datas(), new Datas());
            environmentStates = new EnvironmentStates(playerStates);
#if UNITY_EDITOR
            LoadEventsAndStates();
#endif
            DontDestroyOnLoad(gameObject);
            //Debug
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
        }

       

        private void Start()
        {
            PauseAction.Enable();
            //Time.timeScale = 0.5f;
        }
        public bool LoadVolum(out float soundVolum, out float musicVolum)
        {
            soundVolum = this.soundVolum;
            musicVolum = this.musicVolum;
            if (PlayerPrefs.HasKey(MusicVolum) && PlayerPrefs.HasKey(SoundVolum))
            {
                soundVolum = PlayerPrefs.GetFloat(SoundVolum);
                musicVolum = PlayerPrefs.GetFloat(MusicVolum);
                return true;
            }
            return false;
        }
        public void SaveVolum(float soundVolum, float musicVolum)
        {
            this.soundVolum = soundVolum;
            this.musicVolum = musicVolum;
            PlayerPrefs.SetFloat(MusicVolum, musicVolum);
            PlayerPrefs.SetFloat(SoundVolum, soundVolum);
            PlayerPrefs.Save();
        }
        private void Update()
        {
            if (PauseAction.WasPressedThisFrame())
            {
                Pause(Time.timeScale == 1 ? true : false);
            }
            //Debug.Log("Counting time :" + PlayTimeInSeconds);
            if (_isCountingTime)
            {
                PlayTimeInSeconds += Time.deltaTime;
            }
        }
        public void StartTimer(float Time, Action callback)
        {
            if (TimerText == null)
            {
                TimerText = GameObject.Find(TimerHUD).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            }
            TimerText.transform.parent.gameObject.SetActive(true);
            startTimer = GetElapsedTime();
            StartCoroutine(startTimerCouroutine(Time, callback));
        }
        private IEnumerator startTimerCouroutine(float Time, Action callback)
        {
            while (startTimer + Time > GetElapsedTime())
            {
                yield return new WaitForSeconds(1);
                TimeSpan time = TimeSpan.FromSeconds(startTimer + Time - GetElapsedTime());
                TimerText.text = time.ToString(@"hh\:mm\:ss");
            }
            TimerText.transform.parent.gameObject.SetActive(false);
            callback?.Invoke();
            TimeOverEvent?.Invoke();
        }
        public void Pause(bool pause)
        {
            if (SceneManager.GetActiveScene().name.Equals(MainScene) || SceneManager.GetActiveScene().name.Equals(InternAreas))
                TryPause(pause, PauseHUD, out GameObject go);

        }
        private bool TryPause(bool pause, string hudName, out GameObject go)
        {
            if (GamePaused)
            {
                var pauseHud = GameObject.Find(PauseHUD).transform.GetChild(0).gameObject;
                if (pauseHud != null)
                {
                    if (hudName.Equals(SaveHUD) && pauseHud.activeInHierarchy)
                        SceneManager.LoadScene(MainMenu);
                    pauseHud.SetActive(false);
                }
                var saveHud = GameObject.Find(SaveHUD).transform.GetChild(0).gameObject;
                if (saveHud != null) saveHud.SetActive(false);
                GamePaused = false;
            }
            go = GameObject.Find(hudName).transform.GetChild(0).gameObject;
            if (go == null)
                return false;
            if (GameObject.Find("AudioManager").TryGetComponent<MusicSource>(out MusicSource musicSource))
            {
                float musicVol = pause ? -80f : musicVolum;
                float soundVol = pause ? -80f : soundVolum;
                musicSource.SetMusicVolume(musicVol);
                musicSource.SetSFXVolume(soundVol);
                Time.timeScale = pause ? 0 : 1;
                go.SetActive(pause);
                GetPlayerScript.AllInputsOn(!pause);
                GamePaused = pause;
                return true;
            }
            return false;
        }

        public void OnCallSave(bool active)
        {
            TryPause(active, SaveHUD, out GameObject go);
        }
        private void OnApplicationPause(bool pause)
        {
            Pause(pause);
        }
        public void StartCountingTime()
        {
            _isCountingTime = true;
        }

        public void StopCountingTime()
        {
            _isCountingTime = false;
        }

        public void SetElapsedTime(float savedTime)
        {
            PlayTimeInSeconds = savedTime;
        }

        public float GetElapsedTime()
        {
            return PlayTimeInSeconds;
        }
        private SceneStartType sceneStartType;
        //Este metodo é chamado pelo script UI/Menu.cs
        public void LoadGame(SceneStartType type, int saveGameIndex = 0)
        {
            StartCoroutine(LoadGame(true, type, saveGameIndex));
        }
        public IEnumerator LoadGame(bool courotine, SceneStartType type, int saveGameIndex = 0)
        {
            yield return new WaitForEndOfFrame();
            Debug.LogWarning("LoadGame type:" + type);
            sceneStartType = type;
            if (type.Equals(SceneStartType.Intern))
            {
                StartCoroutine(ChangeScene(InternAreas));
                yield return new WaitForEndOfFrame();
            }
            if (type == SceneStartType.Main)
            {
                StartCoroutine(ChangeScene(MainScene));
                yield return new WaitForEndOfFrame();
            }
            if (type == SceneStartType.Start || type == SceneStartType.New || type == SceneStartType.Continue)
                eventManager.Reset();
            if (type == SceneStartType.Start)
            {
                SceneManager.LoadScene(FroggerScene);
                yield return new WaitForEndOfFrame();
            }
            if (type == SceneStartType.New)
            {
                SaveStates(0);
                SceneManager.LoadScene(MainScene);
                yield return new WaitForEndOfFrame();
            }
            if (type == SceneStartType.Continue)
            {
                this.EnvironmentStates = LoadStates(saveGameIndex);
                //neste momento o horario ainda esta correto bug:2051
                //Debug.Log("[GameManager][LoadGame] hour: " + this.EnvironmentStates.playerStates.Hour);
                this.PlayerStates = this.EnvironmentStates.playerStates;
                if (saveGameIndex == 0)
                    this.PlayerStates.Hearts = this.playerStates.MaxHearts;
                continueGame = true;
                //vamos colocar a carga dos eventos completos aqui, não sei se é o melhor lugar, mas parece resolver o problema dos eventos carregarem após a cena
                //já estar carregada
                if (eventManager == null)
                    eventManager = GetComponent<EventsManager>();
                eventManager.LoadEvents(this.PlayerStates.CompletedGameEvents);
                //Debug.Log("[GameManager][LoadGame] 2 hour: " + this.EnvironmentStates.playerStates.Hour);
                SceneManager.LoadScene(MainScene);
                //ChangeGameToState(this.PlayerStates);
                yield return new WaitForEndOfFrame();
            }
        }

        private IEnumerator ChangeScene(string sceneName)
        {
            //Debug.Log("[GameManager][ChangeScene] player hour:" + this.EnvironmentStates.playerStates.Hour);
           
            ScreenEffects se = FindAnyObjectByType<ScreenEffects>();
            if (se != null)
            {
                se.FadeOut(1f);
                yield return new WaitForSeconds(1f);
                // se.screenFader.fadeImage.color = Color.black;
            }

            SceneManager.LoadScene(sceneName);
            se = FindAnyObjectByType<ScreenEffects>();
            if (se != null)
            {
                se.screenFader.fadeImage.color = Color.black;
                se.FadeIn(1f);
            }
            yield return null;
        }
        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            ServiceLocator.Instance.GetAsync<MusicSource>((musicSource) =>
            {
                musicSource.SetMusicVolume(this.musicVolum);
                musicSource.SetSFXVolume(this.soundVolum);
            });
            if (arg0.name.Equals(MainScene))
            {
                if (sceneStartType.Equals(SceneStartType.Main))
                {
                    //Debug.Log("Topoint index:" + ToPoint);
                    GameObject.Find("PlayerPointsEntry").GetComponent<PlayerPointsEntry>().Activate();
                    ChangeGameToState(this.EnvironmentStates);
                    // GameManager.Instance.GetPlayer.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    return;
                }
                StartCountingTime();
#if UNITY_EDITOR
                //ChangeGameToState(this.EnvironmentStates);
                //return;

#endif
              //  Debug.Log($"[GameManager] SceneManager_sceneLoaded {MainScene} continue:{continueGame} ");
                if (continueGame)
                {
                    //Aqui o horario ja esta alterado bug:2051
                   // Debug.Log("[GameManager][SceneManager_sceneLoaded] hour: " + this.EnvironmentStates.playerStates.Hour);
                    ChangeGameToState(this.EnvironmentStates);
                    PlayerStartPosition = playerStates.PlayerPosition.Position;
                    continueGame = false;
                }
                else
                {
                    LoadStartGamePoint(0);
                }
            }
            if (arg0.name.Equals(InternAreas))
            {
                GameObject.Find("PlayerPointsEntry").GetComponent<PlayerPointsEntry>().Activate();
                ChangeGameToState(this.EnvironmentStates);
                //GameManager.Instance.GetPlayer.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            }

        }
        private void SceneManager_sceneUnloaded(Scene arg0)
        {
            Debug.Log("[GameManager] sceneUnload name: " + arg0.name);
            if(arg0.name.Equals(MainScene))
                ServiceLocator.Instance.ResetService();
        }
        private void LoadStartGamePoint(int index)
        {
            //StartGamePosition = GameObject.Find(StartPointBuilder).gameObject.transform.position;
            StartGamePosition = new Vector3(-110.73f, -4.34f, 0f);
            this.PlayerStates.PlayerPosition.Position = StartGamePosition;
            PlayerStartPosition = StartGamePosition;
            // Debug.Log("[GameManager] playerstartposition:" + PlayerStartPosition);
            GameManager.Instance.UpdateHearts(this.playerStates.Hearts);
            GameManager.Instance.SaveStates(index);
            // DebugUtils.Log($"walljumptutorial: {this.environmentStates.NPC_WallJump_Tutorial}");
        }
        public void UpdateScore()
        {
            var hud = GameObject.Find(CollecteblesHUD);
            var score = hud.transform.GetChild(0).gameObject;
            scoreText = score.GetComponent<TextMeshProUGUI>();
            var image = hud.transform.GetChild(1).gameObject;
            if (playerStates.Collectables > 0)
            {
                score.SetActive(true);
                image.SetActive(true);
            }
            scoreText.text = playerStates.Collectables.ToString("000");
        }
        public void UpdateShurykens()
        {
            bool active = playerStates.Shurykens >= 0;
            GameObject hud = GameObject.Find(ShurykenHUD);

            if (active)
            {
                var shurykens = hud.transform.GetChild(1).gameObject;
                hud.transform.GetChild(0).gameObject.SetActive(active);
                shurykens.SetActive(active);
                shurykens.GetComponent<TextMeshProUGUI>().text = playerStates.Shurykens.ToString("00");
            }

        }
        public bool IsCollected(string itemID)
        {
            return playerStates.CollectablesID.Contains(itemID);
        }
        public bool IsOpened(string chestID)
        {
            return playerStates.ChestsID.Contains(chestID);
        }
        public bool IsActived(string ActivatorID)
        {
            return environmentStates.Activeds.Contains(ActivatorID);
        }
        public void SetActived(string ActivatorID, bool actived)
        {
            if (actived)
                GameManager.Instance.EnvironmentStates.Activeds.Add(ActivatorID);
            else
                GameManager.Instance.EnvironmentStates.Activeds.Remove(ActivatorID);
            ActiveItemChanged?.Invoke(ActivatorID, actived);
        }
        public void UpdateProjectil(Color color, Sprite sprite)
        {
            GameObject hud = GameObject.Find(SpiritHUD);
            if (hud != null)
            {
                hud.SetActive(true); // garante que o pai está ativo

                Image image = hud.transform.GetChild(0).GetComponent<Image>();
                image.gameObject.SetActive(true);
                image.sprite = sprite;

                image = hud.transform.GetChild(1).GetComponent<Image>();
                image.gameObject.SetActive(true);
                image.color = color;
            }
            else
            {
                Debug.LogWarning("HUD não encontrado!");
            }
        }
        #region vamos passar tudo isso para o script HeartHUD
        int maxColHearts = 10;
        public void UpdateHeart(int hearts)
        {
            GameObject hud = GameObject.Find(HeartHUD).transform.GetChild(0).gameObject;
            GetPlayerScript.CurrentLife += hearts;
            this.PlayerStates.Hearts += hearts;

            if (hearts > 0)
            {
                StartCoroutine(AddHeart(hud, hearts));
            }

            if (hearts < 0)
            {
                StartCoroutine(RemoveHeart(hud, hearts));
            }
        }
        public void UpdateMaxHearts(int hearts)
        {
            this.PlayerStates.MaxHearts += hearts;
            UpdateHeart(hearts);
        }
        private void UpdateHearts(int hearts)
        {
            GameObject hud = GameObject.Find(HeartHUD).transform.GetChild(0).gameObject;
            StartCoroutine(AddHeart(hud, hearts - 1));
        }
        IEnumerator AddHeart(GameObject hud, int hearts)
        {
            int heartCount = hud.transform.childCount;
            int total = hearts + heartCount;
            GameObject heart = hud.transform.GetChild(0).gameObject;
            GameObject lastHeart = hud.transform.GetChild(heartCount - 1).gameObject;
            var rect = hud.GetComponent<RectTransform>();
            var heartRect = heart.GetComponent<RectTransform>();
            int col = heartCount % maxColHearts;
            int row = heartCount / maxColHearts;

            while (total > hud.transform.childCount)
            {
                var gb = Instantiate(heart, rect, false);
                //Debug.Log("Col: " + col + " Row: " + row);
                float offsetX = (heartRect.sizeDelta.x + 0.5f) * col;
                float offsetY = (-heartRect.sizeDelta.y - 0.5f) * row;
                gb.GetComponent<RectTransform>().anchoredPosition = gb.GetComponent<RectTransform>().anchoredPosition + new Vector2(offsetX, offsetY);
                col++;
                if (col >= maxColHearts)
                {
                    row++;
                    col = 0;
                }
                yield return new WaitForSeconds(0.05f);
            }
        }
        IEnumerator RemoveHeart(GameObject hud, int hearts)
        {
            int toRemove = hearts;
            while (toRemove < 0)
            {
                Destroy(hud.transform.GetChild(hud.transform.childCount - 1).gameObject);
                toRemove++;
                yield return new WaitForSeconds(0.05f);
            }
        }
        #endregion
        public bool CanContinue()
        {
            SavesManager sm = new SavesManager();
            return sm.CanContinue();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">Use zero to temp save</param>
        public bool SaveStates(int index)
        {
            try
            {
                // OnCallSave(false);
                EnvironmentStates.GameTimeInSeconds = GetElapsedTime();
                SavesManager sm = new SavesManager();
                return sm.Save(index, this.PlayerStates, this.EnvironmentStates, FindAnyObjectByType<CamerasController>().ThumbCamera.GetComponent<Camera>());
            }
            catch
            {
                return false;
            }
        }
        public EnvironmentStates LoadStates(int index)
        {
            SavesManager sm = new SavesManager();
            SaveStates save = sm.Load(index);
            return save.environmentStates;
        }
        public void ChangeGameToState(EnvironmentStates state)
        {
            //Debug.Log("[GameManager][ChangeGameToState] state index: " + state.index);
           // this.EnvironmentStates = LoadStates(saveGameIndex);
            //aqui o horario ja esta alterado, e antes em scene_loaded tbm ja esta anterado devo investigar antes disso bug:2051
           // Debug.Log("[GameManager] ChangeGameToState");
            SetElapsedTime(EnvironmentStates.GameTimeInSeconds);
            GameManager.Instance.UpdateScore();
            GameManager.Instance.UpdateHearts(state.playerStates.Hearts);
            GameManager.Instance.UpdateShurykens();
           // Debug.Log("ChangeGameToState hour: " + state.playerStates.Hour);
            ServiceLocator.Instance.LogRegistredsServicesNames();
            FindAnyObjectByType<CameraBackground>().InitializeDayByHour(state.playerStates.Hour);
            //Debug.Log("[GameManager] GhangeGameToState hour:"+ServiceLocator.Instance.Get<IHourProvider>().Hour);
            GameStatesRestaured?.Invoke();
        }
        public void GameOver()
        {
            StopCountingTime();
            ServiceLocator.Instance.Get<MusicSource>().StopAll();
            this.EnvironmentStates = LoadStates(0);
            this.PlayerStates = this.EnvironmentStates.playerStates;
            this.PlayerStates.numDies++;
            SaveStates(0);
            SceneManager.LoadScene(GameOverScene);
        }
        public void EventCompleted(GameEventName gameEvent, bool playSound = true)
        {
            if (!eventManager.EventCompleted(gameEvent, playSound))
                return;
            PlayerStates.CompletedGameEvents.Add(gameEvent.ToString());
            //events.GetEvent(gameEvent).Completed = true;
            switch (gameEvent)
            {
                case GameEventName.KillPig:
                    CinemachineConfiner confiner = GameObject.FindAnyObjectByType<CinemachineVirtualCamera>().GetComponent<CinemachineConfiner>();
                    confiner.m_BoundingShape2D = (PolygonCollider2D)GameObject.Find(CameraContainer).transform.GetChild(1).gameObject.GetComponentAtIndex(1);
                    break;
                //case GameEventName.HeartContainer:
                //    GameObject gameObject = GameObject.Find(HeartHUD).transform.GetChild(0).gameObject;
                //    gameObject.SetActive(true);
                //    break;
                case GameEventName.Shuryken:
                    GameObject o = GameObject.Find("ShurykenPoint");
                    if (o != null)
                        o.SetActive(false);
                    break;
                case GameEventName.DuckPath:
                    confiner = GameObject.FindAnyObjectByType<CinemachineVirtualCamera>().GetComponent<CinemachineConfiner>();
                    confiner.m_BoundingShape2D = (PolygonCollider2D)GameObject.Find(CameraContainer).transform.GetChild(2).gameObject.GetComponentAtIndex(1);
                    break;
                //case GameEventName.Gravity:
                //    var hud = GameObject.Find(SkillsHUD).transform.GetChild(0).gameObject;
                //    hud.SetActive(true);
                //    break;
                case GameEventName.MysticScroll:
                    confiner = GameObject.FindAnyObjectByType<CinemachineVirtualCamera>().GetComponent<CinemachineConfiner>();
                    confiner.m_BoundingShape2D = (PolygonCollider2D)GameObject.Find(CameraContainer).transform.GetChild(3).gameObject.GetComponentAtIndex(1);
                    //gameObject = GameObject.Find(ToSkyPoint).transform.GetChild(0).gameObject;
                    //gameObject.SetActive(true);
                    break;

            }
        }
        public bool IsEventCompleted(GameEventName gameEvent)
        {
            try
            {
                return eventManager.GetEvent(gameEvent).Completed;
            }
            catch
            {
                return false;
            }
        }
        private int currentSkill = 0;
        public void ActiveSkill(bool active)
        {
            var text = GameObject.Find(SkillsHUD).transform.GetChild(0).transform.GetChild(currentSkill).gameObject.GetComponent<TextMeshProUGUI>();
            if (active)
                text.color = Color.red;
            else
                text.color = Color.gray;
        }

        public void ResetEnvironment()
        {
            GameManager.Instance.eventManager.Reset();
            GameManager.Instance.PlayerStates.CollectablesID.Clear();
            GameManager.Instance.PlayerStates.ChestsID.Clear();
            GameManager.Instance.EnvironmentStates.Activeds.Clear();
            GameManager.Instance.environmentStates.NPCVirtualGuyApples = 0;
            GameManager.Instance.environmentStates.NPCVirtualGuyDialogue = 0;
            GameManager.Instance.environmentStates.NPC_WallJump_Tutorial = 0;
            GameManager.Instance.PlayerStates.PlayerPosition.Position = StartGamePosition;
        }
        private void OnApplicationQuit()
        {
            // Debug.Log("Application Quit");
            PlayerPrefs.Save();
        }
        //private void OnApplicationFocus(bool focus)
        //{
        //    Cursor.visible = !focus;
        //}
        #region testes e debugs
        //public void UpdatePlayer()
        //{

        //    GetPlayerScript.Speed += 0.1f;
        //    GetPlayerScript.JumpForce += 0.1f;
        //    this.PlayerStates.Speed += 0.1f;
        //    this.playerStates.JumpForce += 0.1f;

        //}
        //public void SetSceneData<T>(T data) where T : class, new()
        //{
        //    GameObject dataScene = Instantiate(DataScenePreserverGameObject);
        //    DataScenePreserver.Instance.Set<T>(data);
        //}
        //public T GetSceneData<T>() where T : class, new()
        //{
        //    if(DataScenePreserver.Instance!=null)
        //    {
        //        T data = DataScenePreserver.Instance.Get<T>();
        //        DataScenePreserver.Instance.Clear();
        //        return data;
        //    }
        //    return null;
        //}
        public void TesteThumb()
        {
            var t = new ThumbGenerator(0.1f);
            string file = t.CreateEncodeThumb(FindAnyObjectByType<CamerasController>().ThumbCamera.GetComponent<Camera>(), GetPlayer);
            byte[] buffert = Convert.FromBase64String(file);
            File.WriteAllBytes(@"C:\Users\bonus630\Desktop\teste\p.png", buffert);
        }
        private void LoadEventsAndStates()
        {
#if UNITY_EDITOR
            //////Time.timeScale = 0.5f;
            //playerStates.HasGravity = true;
            playerStates.HasVision = true;
            playerStates.Collectables = 21;
            //playerStates.HasFireball = true;
            //playerStates.HasLightning = true;
            //playerStates.HasWallJump = true;
            //////playerStates.HasDoubleJump = true;
            //playerStates.FallsControl = true;
            //playerStates.HasDash = true;
            //playerStates.Shurykens = 100;
            //this.EventCompleted(GameEventName.HeartContainer, false);
            //this.EventCompleted(GameEventName.PlayerCheckWall, false);
            //this.EventCompleted(GameEventName.NPCFirstTalk, false);
            //this.EventCompleted(GameEventName.KillPig, false);
            //this.EventCompleted(GameEventName.LightningBolt, false);
            //this.EventCompleted(GameEventName.MagicGlass, false);
            //this.EventCompleted(GameEventName.Gravity, false);
            //this.EventCompleted(GameEventName.FeatherTouch, false);
            //this.EventCompleted(GameEventName.FireBall, false);
            //////this.EventCompleted(GameEventName.RollingWind, false);
            //////this.EventCompleted(GameEventName.PrisionerTip, false);
            //////this.EventCompleted(GameEventName.LadyLaments, false);
            ////this.EventCompleted(GameEventName.KoarFounded, false);

#endif
            #endregion
        }
    }
    public enum SceneStartType
    {
        Start,
        Continue,
        New,
        Intern,
        Main
    }
}


