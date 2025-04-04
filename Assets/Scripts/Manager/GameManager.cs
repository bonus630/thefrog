using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using br.com.bonus630.thefrog.Activators;
using br.com.bonus630.thefrog.Caracters;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace br.com.bonus630.thefrog.Manager
{
    [DefaultExecutionOrder(-1)]
    public class GameManager : MonoBehaviour
    {
        private TextMeshProUGUI scoreText;
        private bool continueGame = false;
        //public int Hour { get; set; }
        //public int Shurykens { get; set; }
        private PlayerStates playerStates;
        public PlayerStates PlayerStates { get { return playerStates; } private set { playerStates = value; } }

        private EnvironmentStates environmentStates;
        public EnvironmentStates EnvironmentStates { get { return environmentStates; } private set { environmentStates = value; } }
        public GameObject GetPlayer { get { return GameObject.FindGameObjectWithTag("Player"); } }
        public static GameManager Instance;
        public EventsManager eventManager;
        public Vector3 StartGamePosition { get; private set; }

        //Scenes Names
        public readonly string MainScene = "SampleScene";
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


        //Env Names
        public string SaveDataFilePath { get; private set; }



        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            SaveDataFilePath = Path.Combine(Application.persistentDataPath, "FrogData.json");
            playerStates = new PlayerStates(new PlayerPosition(gameObject.transform.position), new Datas(), new Datas(), new Datas());
            environmentStates = new EnvironmentStates(playerStates);
            //Debug
#if UNITY_EDITOR
           // playerStates.HasGravity = true;
            playerStates.HasFireball = true;
            playerStates.HasWallJump = true;
            playerStates.HasFireball = true;
            playerStates.Shurykens = 100;
            playerStates.Collectables = 31;

#endif
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (arg0.name.Equals(MainScene))
            {
#if UNITY_EDITOR
                ChangeGameToState(this.EnvironmentStates);
                return;

#endif
                if (!continueGame)
                    LoadStartGamePoint();
                else
                    ChangeGameToState(this.EnvironmentStates);
            }

        }
        private void LoadStartGamePoint()
        {
            StartGamePosition = GameObject.Find(StartPointBuilder).gameObject.transform.position;
            this.PlayerStates.PlayerPosition.Position = StartGamePosition;
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
        public void UpdateHeart(int hearts)
        {
            GameObject hud = GameObject.Find(HeartHUD).transform.GetChild(0).gameObject;
            if (hearts > 0)
            {
                StartCoroutine(AddHeart(hud, hearts));
            }

            if (hearts < 0)
            {
                StartCoroutine(RemoveHeart(hud, hearts));
            }
        }
        public void LoadGame(StartType type)
        {
           // Debug.LogWarning("LoadGame");
            eventManager.Reset();
            if (type == StartType.Start)
            {
                SceneManager.LoadScene(FroggerScene);
            }
            if (type == StartType.New)
            {
                SceneManager.LoadScene(MainScene);


            }
            if (type == StartType.Continue)
            {
                this.EnvironmentStates = LoadStates();
                this.PlayerStates = this.EnvironmentStates.playerStates;
                continueGame = true;
                SceneManager.LoadScene(MainScene);
                //ChangeGameToState(this.PlayerStates);
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
            //Debug.Log("Save States");
            // jairson esta vindo vazio 
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

            GameManager.Instance.playerStates.CompletedGameEvents.Add(GameEventName.HeartContainer.ToString());
            GameManager.Instance.playerStates.CompletedGameEvents.Add(GameEventName.FireBall.ToString());
            GameManager.Instance.playerStates.CompletedGameEvents.Add(GameEventName.Gravity.ToString());
#endif
            GameManager.Instance.UpdateScore();
            GameManager.Instance.UpdateHeart(state.playerStates.Hearts);
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
                        case GameEventName.NPCFirstTalk:
                            FindAnyObjectByType<NPC_WallJump_Tutorial>().firstTalk = true;
                            break;
                        case GameEventName.NPCTutorial:
                            FindAnyObjectByType<NPC_WallJump_Tutorial>().GoToFinal();
                            break;
                        case GameEventName.KillPig:
                            FindAnyObjectByType<KiilPig>().ExecuteKillPig();
                            if (IsEventCompleted(GameEventName.NPCFirstTalk))
                            {
                                FindAnyObjectByType<NPC_WallJump_Tutorial>().MoveToWallJump();
                            }
                            break;
                        case GameEventName.Shuriken:
                            GameObject.Find("ShurikenChest").SetActive(false);
                            break;
                        case GameEventName.HeartContainer:
                            GameObject.Find("HeartContainerChest").SetActive(false);
                            break;
                        //case GameEventName.Gravity:
                        //    FindAnyObjectByType<NPCDuck>().Dancing();
                        //    break;
                    }
                }

            }
        }
        public void GameOver()
        {
            //Debug.Log("Game Over");
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
                        FindAnyObjectByType<NPC_WallJump_Tutorial>().FirstTalk = true;
                    }
                    UpdatePlayer();
                    //FindAnyObjectByType<NPC_WallJump_Tutorial>().PrepareToNext();
                    break;
                case GameEventName.NPCFirstTalk:
                    if (eventManager.GetEvent(GameEventName.KillPig).Completed)
                        FindAnyObjectByType<NPC_WallJump_Tutorial>().KillPig = true;
                    //FindAnyObjectByType<NPC_WallJump_Tutorial>().PrepareToNext();
                    break;
                case GameEventName.NPCTutorial:
                    playerStates.HasWallJump = true;
                    UpdatePlayer();
                    break;
                case GameEventName.HeartContainer:
                    GameObject gameObject = GameObject.Find(HeartHUD).transform.GetChild(0).gameObject;
                    gameObject.SetActive(true);
                    GameManager.Instance.PlayerStates.Hearts++;
                    UpdatePlayer();
                    break;
                case GameEventName.DuckPath:
                    NPC_WallJump_Tutorial nPC_WallJump_Tutorial = FindAnyObjectByType<NPC_WallJump_Tutorial>();
                    nPC_WallJump_Tutorial.Dash();
                    confiner = GameObject.FindAnyObjectByType<CinemachineVirtualCamera>().GetComponent<CinemachineConfiner>();
                    confiner.m_BoundingShape2D = (PolygonCollider2D)GameObject.Find(CameraContainer).transform.GetChild(2).gameObject.GetComponentAtIndex(1);
                    break;
                case GameEventName.Gravity:
                    var hud = GameObject.Find(SkillsHUD).transform.GetChild(0).gameObject;
                    PlayerStates.HasGravity = true;
                    hud.SetActive(true);
                    UpdatePlayer();
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

                    //GameObject.Find("SkyPit").transform.position += Vector3.up * 20f;
                    break;
                case GameEventName.FeatherTouch:
                    PlayerStates.FallsControl = true;
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
        private void UpdatePlayer()
        {
            if (GameManager.Instance.GetPlayer.TryGetComponent<Player>(out Player player))
            {
                player.Speed += 0.1f;
                player.JumpForce += 0.1f;
            }
        }
    }
    public enum StartType
    {
        Start,
        Continue,
        New
    }
}


