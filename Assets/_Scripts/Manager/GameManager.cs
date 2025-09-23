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

namespace br.com.bonus630.thefrog.Manager
{
    [DefaultExecutionOrder(-1)]
    public class GameManager : MonoBehaviour
    {
        [SerializeField] InputAction PauseAction;
       
        private TextMeshProUGUI scoreText;
        private TextMeshProUGUI TimerText = null;
        public event Action TimeOverEvent;
        private bool continueGame = false;
        private bool _isCountingTime = false;
        float startTimer = 0;
        public float PlayTimeInSeconds { get; private set; }
        public int ToPoint { get; set; }
        private PlayerStates playerStates;
        public PlayerStates PlayerStates { get { return playerStates; } private set { playerStates = value; } }

        private EnvironmentStates environmentStates;
        public EnvironmentStates EnvironmentStates { get { return environmentStates; } private set { environmentStates = value; } }
        private GameObject player;
        public GameObject GetPlayer { get { if (player == null) player = GameObject.Find("Player"); return player; }}
        private ScreenEffects screenEffects;
        public ScreenEffects ScreenEffects { get { if (screenEffects == null) screenEffects = FindAnyObjectByType<ScreenEffects>(); return screenEffects; } }
        public static GameManager Instance;
        public EventsManager eventManager;

        public Vector3 StartGamePosition { get; private set; }
        public Vector3 PlayerStartPosition { get; set; }
        public IPlayer GetPlayerScript { get { return GetPlayer.GetComponent<IPlayer>(); } }
        //Scenes Names
        public readonly string MainScene = "SampleScene";
        public readonly string InternAreas = "InternAreas";
        public readonly string FroggerScene = "Frogger";
        public readonly string GameOverScene = "GameOver";

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

            playerStates = new PlayerStates(new PlayerPosition(gameObject.transform.position), new Datas(), new Datas(), new Datas());
            environmentStates = new EnvironmentStates(playerStates);
            //Debug
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            Instance = this;
#if UNITY_EDITOR
            //Time.timeScale = 0.5f;
            playerStates.HasGravity = true;
            playerStates.HasFireball = true;
            playerStates.HasWallJump = true;
 //           playerStates.HasDoubleJump = true;
            playerStates.FallsControl = true;
            playerStates.HasDash = true;
            playerStates.Shurykens = 100;
            eventManager.EventCompleted(GameEventName.Gravity, false);
            eventManager.EventCompleted(GameEventName.FeatherTouch, false);
            eventManager.EventCompleted(GameEventName.LightningBolt, false);
            eventManager.EventCompleted(GameEventName.FireBall, false);
            eventManager.EventCompleted(GameEventName.RollingWind, false);
            eventManager.EventCompleted(GameEventName.PrisionerTip, false);
            eventManager.EventCompleted(GameEventName.LadyLaments, false);

#endif
            DontDestroyOnLoad(gameObject);
        }
        private void Start()
        {
            PauseAction.Enable();
            //Time.timeScale = 0.5f;
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
        public void StartTimer(float Time)
        {
            if (TimerText == null)
            {
                TimerText = GameObject.Find(TimerHUD).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            }
            TimerText.transform.parent.gameObject.SetActive(true);
            startTimer = GetElapsedTime();
            StartCoroutine(startTimerCouroutine(Time));
        }
        private IEnumerator startTimerCouroutine(float Time)
        {
            while (startTimer + Time > GetElapsedTime())
            {
                yield return new WaitForSeconds(1);
                TimeSpan time = TimeSpan.FromSeconds(startTimer + Time - GetElapsedTime());
                TimerText.text = time.ToString(@"hh\:mm\:ss");
            }
            TimerText.transform.parent.gameObject.SetActive(false);
            TimeOverEvent?.Invoke();
        }
        private void Pause(bool pause)
        {
            if (SceneManager.GetActiveScene().name.Equals("SampleScene") || SceneManager.GetActiveScene().name.Equals("InternAreas"))
                TryPause(pause, PauseHUD, out GameObject go);

        }
        private bool TryPause(bool pause, string hudName, out GameObject go)
        {
            go = GameObject.Find(hudName).transform.GetChild(0).gameObject;
            if (go == null)
                return false;
            if (GameObject.Find("AudioManager").TryGetComponent<MusicSource>(out MusicSource musicSource))
            {
                float vol = pause ? -80f : 0f;
                musicSource.SetMasterVolume(vol);
                Time.timeScale = pause ? 0 : 1;
                go.SetActive(pause);
                GetPlayerScript.AllInputsOn(!pause);
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
        public void LoadGame(SceneStartType type, int index = 0)
        {
           // Debug.LogWarning("LoadGame type:" + type);
            sceneStartType = type;
            if (type.Equals(SceneStartType.Intern))
            {
                StartCoroutine(ChangeScene(InternAreas));
                return;
            }
            if (type == SceneStartType.Main)
            {
                StartCoroutine(ChangeScene(MainScene));
                return;
            }
            eventManager.Reset();
            if (type == SceneStartType.Start)
            {
                SceneManager.LoadScene(FroggerScene);
            }
            if (type == SceneStartType.New)
            {
                SceneManager.LoadScene(MainScene);
            }
            if (type == SceneStartType.Continue)
            {
                this.EnvironmentStates = LoadStates(index);
                this.PlayerStates = this.EnvironmentStates.playerStates;
                continueGame = true;
                SceneManager.LoadScene(MainScene);
                //ChangeGameToState(this.PlayerStates);
            }
        }

        private IEnumerator ChangeScene(string sceneName)
        {
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
            if (arg0.name.Equals(MainScene))
            {
                if (sceneStartType.Equals(SceneStartType.Main))
                {
                    Debug.Log("Topoint index:" + ToPoint);
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
                if (!continueGame)
                    LoadStartGamePoint(0);
                else
                {
                    ChangeGameToState(this.EnvironmentStates);
                    PlayerStartPosition = playerStates.PlayerPosition.Position;
                }
            }
            if (arg0.name.Equals(InternAreas))
            {
                GameObject.Find("PlayerPointsEntry").GetComponent<PlayerPointsEntry>().Activate();
                ChangeGameToState(this.EnvironmentStates);
                //GameManager.Instance.GetPlayer.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            }

        }
        private void LoadStartGamePoint(int index)
        {
            StartGamePosition = GameObject.Find(StartPointBuilder).gameObject.transform.position;
            this.PlayerStates.PlayerPosition.Position = StartGamePosition;
            PlayerStartPosition = StartGamePosition;
            GameManager.Instance.UpdateHearts(this.playerStates.Hearts);
            GameManager.Instance.SaveStates(index);
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
            scoreText.text = playerStates.Collectables.ToString("0000");
        }
        public void UpdateShurykens()
        {
            bool active = playerStates.Shurykens > 0;
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
        public void SetActived(string ActivatorID,bool actived)
        {
            if (actived)
                GameManager.Instance.EnvironmentStates.Activeds.Add(ActivatorID);
            else
                GameManager.Instance.EnvironmentStates.Activeds.Remove(ActivatorID);
        }
        private void UpdateHearts(int hearts)
        {
            GameObject hud = GameObject.Find(HeartHUD).transform.GetChild(0).gameObject;
            StartCoroutine(AddHeart(hud, hearts - 1));
        }
        public void UpdateHeart(int hearts)
        {
            GameObject hud = GameObject.Find(HeartHUD).transform.GetChild(0).gameObject;
            GetPlayerScript.CurrentLife += hearts;
            if (hearts > 0)
            {
                playerStates.Hearts += hearts;
                StartCoroutine(AddHeart(hud, hearts));
            }

            if (hearts < 0)
            {
                StartCoroutine(RemoveHeart(hud, hearts));
            }
        }
        public void UpdateProjectil(Color color)
        {
            Debug.Log("Projectil color:" + color);
        }

        IEnumerator AddHeart(GameObject hud, int hearts)
        {
            int heartCount = hud.transform.childCount;
            int total = hearts + heartCount;
            GameObject heart = hud.transform.GetChild(0).gameObject;
            GameObject lastHeart = hud.transform.GetChild(heartCount - 1).gameObject;
            var rect = hud.GetComponent<RectTransform>();
            var heartRect = heart.GetComponent<RectTransform>();
            int col = heartCount % 5;
            int row = heartCount / 5;

            while (total > hud.transform.childCount)
            {
                var gb = Instantiate(heart, rect, false);
                //Debug.Log("Col: " + col + " Row: " + row);
                float offsetX = (heartRect.sizeDelta.x + 0.5f) * col;
                float offsetY = (-heartRect.sizeDelta.y - 0.5f) * row;
                gb.GetComponent<RectTransform>().anchoredPosition = gb.GetComponent<RectTransform>().anchoredPosition + new Vector2(offsetX, offsetY);
                col++;
                if (col > 4)
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
        public bool CanContinue()
        {
            SavesManager sm = new SavesManager();
            return sm.CanContinue();

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">Use zero to temp save</param>
        public void SaveStates(int index)
        {
            OnCallSave(false);
            EnvironmentStates.GameTimeInSeconds = GetElapsedTime();
            SavesManager sm = new SavesManager();
            sm.Save(index, this.PlayerStates, this.EnvironmentStates, FindAnyObjectByType<CamerasController>().ThumbCamera.GetComponent<Camera>());
        }
        public EnvironmentStates LoadStates(int index)
        {
            SavesManager sm = new SavesManager();
            SaveStates save = sm.Load(index);
            return save.environmentStates;
        }
        public void ChangeGameToState(EnvironmentStates state)
        {
#if UNITY_EDITOR
            //Aqui nao chama ao dar play normalmente, utilize o awake para debugar os eventos completos adicionados
            //GameManager.Instance.playerStates.CompletedGameEvents.Add(GameEventName.HeartContainer.ToString());
            //GameManager.Instance.playerStates.CompletedGameEvents.Add(GameEventName.FireBall.ToString());
            //GameManager.Instance.playerStates.CompletedGameEvents.Add(GameEventName.LightningBolt.ToString());
            //GameManager.Instance.playerStates.CompletedGameEvents.Add(GameEventName.RollingWild.ToString());
            //GameManager.Instance.playerStates.CompletedGameEvents.Add(GameEventName.PurifyWater.ToString());
            ////GameManager.Instance.playerStates.CompletedGameEvents.Add(GameEventName.Gravity.ToString());
            ////GameManager.Instance.playerStates.CompletedGameEvents.Add(GameEventName.KillPig.ToString());
            ////GameManager.Instance.playerStates.CompletedGameEvents.Add(GameEventName.NPCFirstTalk.ToString());
            //GameManager.Instance.playerStates.CompletedGameEvents.Add(GameEventName.FeatherTouch.ToString());

#endif
            SetElapsedTime(EnvironmentStates.GameTimeInSeconds);
            GameManager.Instance.UpdateScore();
            GameManager.Instance.UpdateHearts(state.playerStates.Hearts);
            GameManager.Instance.UpdateShurykens();
            Debug.Log("ChangeGameToState hour: " + state.playerStates.Hour);
            FindAnyObjectByType<CameraBackground>().InitializeDayByHour(state.playerStates.Hour);
            eventManager.LoadEvents(GameManager.Instance.playerStates.CompletedGameEvents);
     
        }
        public void GameOver()
        {
            StopCountingTime();
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
                    if (eventManager.GetEvent(GameEventName.NPCFirstTalk).Completed)
                    {
                        // FindAnyObjectByType<NPC_WallJump_Tutorial>().FirstTalk = true;
                    }

                    break;
                //case GameEventName.NPCFirstTalk:
                //    if (eventManager.GetEvent(GameEventName.KillPig).Completed)
                //     //   FindAnyObjectByType<NPC_WallJump_Tutorial>().KillPig = true;

                //    break;
                case GameEventName.NPCTutorial:
                    playerStates.HasWallJump = true;

                    break;
                case GameEventName.HeartContainer:
                    GameObject gameObject = GameObject.Find(HeartHUD).transform.GetChild(0).gameObject;
                    gameObject.SetActive(true);
                    break;
                case GameEventName.Shuryken:
                    GameObject o = GameObject.Find("ShurykenPoint");
                    if (o != null)
                        o.SetActive(false);

                    break;
                case GameEventName.DuckPath:
                    //NPC_WallJump_Tutorial nPC_WallJump_Tutorial = FindAnyObjectByType<NPC_WallJump_Tutorial>();
                    //nPC_WallJump_Tutorial.Dash();
                    confiner = GameObject.FindAnyObjectByType<CinemachineVirtualCamera>().GetComponent<CinemachineConfiner>();
                    confiner.m_BoundingShape2D = (PolygonCollider2D)GameObject.Find(CameraContainer).transform.GetChild(2).gameObject.GetComponentAtIndex(1);
                    break;
                case GameEventName.Gravity:
                    var hud = GameObject.Find(SkillsHUD).transform.GetChild(0).gameObject;
                    PlayerStates.HasGravity = true;
                    hud.SetActive(true);
                    break;
                case GameEventName.FireBall:
                    //var hud = GameObject.Find(SkillsHUD).transform.GetChild(0).gameObject;
                    PlayerStates.HasFireball = true;
                    //hud.SetActive(true);
                    break;
                case GameEventName.MysticScroll:
                    confiner = GameObject.FindAnyObjectByType<CinemachineVirtualCamera>().GetComponent<CinemachineConfiner>();
                    confiner.m_BoundingShape2D = (PolygonCollider2D)GameObject.Find(CameraContainer).transform.GetChild(3).gameObject.GetComponentAtIndex(1);
                    gameObject = GameObject.Find(ToSkyPoint).transform.GetChild(0).gameObject;
                    gameObject.SetActive(true);

                    break;
                case GameEventName.FeatherTouch:
                    PlayerStates.FallsControl = true;
                    break;
                case GameEventName.Dash:
                    PlayerStates.HasDash = true;
                    break;
            }
        }
        public bool IsEventCompleted(GameEventName gameEvent)
        {
            return eventManager.GetEvent(gameEvent).Completed;
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
        public void UpdatePlayer()
        {

            GetPlayerScript.Speed += 0.1f;
            GetPlayerScript.JumpForce += 0.1f;
            this.PlayerStates.Speed += 0.1f;
            this.playerStates.JumpForce += 0.1f;

        }
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


