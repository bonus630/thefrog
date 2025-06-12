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

namespace br.com.bonus630.thefrog.Manager
{
    [DefaultExecutionOrder(-1)]
    public class GameManager : MonoBehaviour
    {
        [SerializeField] InputAction PauseAction;
        private TextMeshProUGUI scoreText;
        private bool continueGame = false;
        public float PlayTimeInSeconds { get; private set; }
        private bool _isCountingTime = false;
        //public int Hour { get; set; }
        //public int Shurykens { get; set; }
        private PlayerStates playerStates;
        public PlayerStates PlayerStates { get { return playerStates; } private set { playerStates = value; } }

        private EnvironmentStates environmentStates;
        public EnvironmentStates EnvironmentStates { get { return environmentStates; } private set { environmentStates = value; } }
        public GameObject GetPlayer { get { return GameObject.Find("Player"); } }
        public IPlayer GetPlayerScript { get { return GetPlayer.GetComponent<IPlayer>(); } }
        public static GameManager Instance;
        public EventsManager eventManager;

        public Vector3 StartGamePosition { get; private set; }
        public int ToPoint { get; set; }
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
        public readonly string TimerHUD = "TimerHUD";


        //Env Names
        public string SaveDataFilePath { get; private set; }

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
            SaveDataFilePath = Path.Combine(Application.persistentDataPath, "FrogData.json");
#if UNITY_EDITOR
            SaveDataFilePath = Path.Combine(Application.persistentDataPath, "FrogData-editor.json");
#endif
            playerStates = new PlayerStates(new PlayerPosition(gameObject.transform.position), new Datas(), new Datas(), new Datas());
            environmentStates = new EnvironmentStates(playerStates);
            //Debug
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            Instance = this;
//#if UNITY_EDITOR
            playerStates.HasGravity = true;
            playerStates.HasWallJump = true;
            playerStates.Shurykens = 100;
//#endif
            DontDestroyOnLoad(gameObject);
        }
        private void Start()
        {
            PauseAction.Enable();
        }
        private void Update()
        {
            if (PauseAction.WasPressedThisFrame())
            {
                if (SceneManager.GetActiveScene().name.Equals("SampleScene"))
                {
                    Pause(Time.timeScale == 1 ? true : false);
                }
            }
            if (_isCountingTime)
            {
                PlayTimeInSeconds += Time.deltaTime;
            }
        }
        TextMeshProUGUI TimerText = null;
        float startTimer = 0;
        public event Action TimeOverEvent;
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
            if (GameObject.Find("AudioManager").TryGetComponent<MusicSource>(out MusicSource musicSource))
            {
                float vol = pause ? -80f : 0f;
                musicSource.SetMasterVolume(vol);
                Time.timeScale = pause ? 0 : 1;
                GameObject go = GameObject.Find(PauseHUD).transform.GetChild(0).gameObject;
                Debug.Log(go);
                if (go != null)
                {
                    go.SetActive(pause);
                    go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = this.PlayerStates.Hour.ToString("00") + " HORAS";
                }
            }
        }
        private void OnApplicationPause(bool pause)
        {
            Debug.Log("Pause:" + pause);
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
        public void LoadGame(SceneStartType type)
        {
             Debug.LogWarning("LoadGame type:"+type);
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
                this.EnvironmentStates = LoadStates();
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
                if(sceneStartType.Equals(SceneStartType.Main))
                {
                    Debug.Log("Topoint index:" + ToPoint);
                    GetPlayer.transform.position = GameObject.Find("PlayerPointsEntry").GetComponent<PlayerPointsEntry>()[ToPoint];
                    return;
                }
                StartCountingTime();
#if UNITY_EDITOR
                //ChangeGameToState(this.EnvironmentStates);
                //return;

#endif
                if (!continueGame)
                    LoadStartGamePoint();
                else
                    ChangeGameToState(this.EnvironmentStates);
            }
            if (arg0.name.Equals(InternAreas))
            {
                Debug.Log("load scene");
                Debug.Log("Point:" +ToPoint);
                Debug.Log("Player:" +GetPlayer.name);
                GetPlayer.transform.position = GameObject.Find("PlayerPointsEntry").GetComponent<PlayerPointsEntry>()[ToPoint];
            }

        }
        private void LoadStartGamePoint()
        {
            StartGamePosition = GameObject.Find(StartPointBuilder).gameObject.transform.position;
            this.PlayerStates.PlayerPosition.Position = StartGamePosition;
            GameManager.Instance.UpdateHearts(this.playerStates.Hearts);
            GameManager.Instance.SaveStates();
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
            return File.Exists(SaveDataFilePath);
        }
        public void SaveStates()
        {
            environmentStates.playerStates = this.PlayerStates;
            string jason = JsonUtility.ToJson(environmentStates);
            File.WriteAllText(SaveDataFilePath, jason);
        }
        public EnvironmentStates LoadStates()
        {
            string json = File.ReadAllText(SaveDataFilePath);
            environmentStates = JsonUtility.FromJson<EnvironmentStates>(json);
            return this.environmentStates;
        }
        public void ChangeGameToState(EnvironmentStates state)
        {
#if UNITY_EDITOR

            //GameManager.Instance.playerStates.CompletedGameEvents.Add(GameEventName.HeartContainer.ToString());
            //GameManager.Instance.playerStates.CompletedGameEvents.Add(GameEventName.FireBall.ToString());
            //GameManager.Instance.playerStates.CompletedGameEvents.Add(GameEventName.Gravity.ToString());
            //GameManager.Instance.playerStates.CompletedGameEvents.Add(GameEventName.KillPig.ToString());
            //GameManager.Instance.playerStates.CompletedGameEvents.Add(GameEventName.NPCFirstTalk.ToString());
            //GameManager.Instance.playerStates.CompletedGameEvents.Add(GameEventName.FeatherTouch.ToString());

#endif
            SetElapsedTime(EnvironmentStates.GameTimeInSeconds);
            GameManager.Instance.UpdateScore();
            GameManager.Instance.UpdateHearts(state.playerStates.Hearts);
            GameManager.Instance.UpdateShurykens();
            FindAnyObjectByType<CameraBackground>().InitializeDayByHour(state.playerStates.Hour);
            for (int i = 0; i < state.playerStates.CompletedGameEvents.Count; i++)
            {
                GameEventName name = GameEventName.None;
                if (Enum.TryParse<GameEventName>(state.playerStates.CompletedGameEvents[i], true, out name))
                {
                    this.EventCompleted(name, false);
                    switch (name)
                    {
                        //case GameEventName.NPCFirstTalk:
                        //    FindAnyObjectByType<NPC_WallJump_Tutorial>().firstTalk = true;
                        //    break;
                        //case GameEventName.NPCTutorial:
                        //    FindAnyObjectByType<NPC_WallJump_Tutorial>().GoToFinal();
                        //    break;
                        //case GameEventName.KillPig:
                        //    FindAnyObjectByType<KiilPig>().ExecuteKillPig();
                        //    if (IsEventCompleted(GameEventName.NPCFirstTalk))
                        //    {
                        //        FindAnyObjectByType<NPC_WallJump_Tutorial>().MoveToWallJump();
                        //    }
                        //    break;
                        case GameEventName.Shuryken:
                            GameObject.Find("ShurikenChest").SetActive(false);
                            break;

                            //case GameEventName.HeartContainer:
                            //    GameObject.Find("HeartContainerChest").SetActive(false);
                            //    break;
                            //case GameEventName.Gravity:
                            //    FindAnyObjectByType<NPCDuck>().Dancing();
                            //    break;
                    }
                }

            }
        }
        public void GameOver()
        {
            StopCountingTime();
            this.EnvironmentStates = LoadStates();
            this.PlayerStates = this.EnvironmentStates.playerStates;
            this.PlayerStates.numDies++;
            EnvironmentStates.GameTimeInSeconds = GetElapsedTime();
            SaveStates();
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


