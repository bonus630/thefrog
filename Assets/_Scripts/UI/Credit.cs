using System;
using System.Collections;
using br.com.bonus630.thefrog.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace br.com.bonus630.thefrog
{
    public class Credit : MonoBehaviour
    {
        [SerializeField] ScreenFader screenFader;
        [SerializeField] GameObject creditsCompose;
        [SerializeField] TextMeshProUGUI states;
        [SerializeField] TextMeshProUGUI isEndText;
        [SerializeField] InputAction MenuAction;
        [SerializeField] InputAction pauseAction;

        float timer = 0f;
        float turnOnTime = 6f;
        float turnOffTime = 2f;
        float totalTime = 0;
        int currentCompose = 0;
        int maxCompose = 0;
        bool showTurn = true;
        bool canMenu = false;
        bool fakeEnd = false;
        bool newGamePlus = false;
        float timeMenu = 0;


        string startButton = "Pressione Enter!";

        private void Awake()
        {
            Debug.Log("Credit :" + GameManager.Instance.EnvironmentStates.GameTimeInSeconds);
            if (GameManager.Instance.EnvironmentStates.GameTimeInSeconds <= 0)
                fakeEnd = true;
            isEndText.gameObject.SetActive(fakeEnd);
        }
        void Start()
        {
            maxCompose = creditsCompose.transform.childCount;
            MenuAction.Enable();
            pauseAction.Enable();
            MenuAction.performed += MenuAction_performed;
            pauseAction.performed += PauseAction_performed;
            states.text = GetStatesString();
        }



        private void OnDestroy()
        {
            MenuAction.performed -= MenuAction_performed;
            pauseAction.performed -= PauseAction_performed;
            MenuAction.Disable();
            pauseAction.Disable();
            Debug.Log("[Credit][ondisable]");
        }
        private void MenuAction_performed(InputAction.CallbackContext obj)
        {
            if (newGamePlus && timeMenu > 0.4f)
                SceneManager.LoadScene("MainMenu");
            else
            {
                var device = obj.control.device;

                if (device is Gamepad)
                    startButton = "Pressione Start!";
                else if (device is Keyboard)
                    startButton = "Pressione Enter!";
                NewGamePlusMessage();
            }
        }
        private void PauseAction_performed(InputAction.CallbackContext obj)
        {
            if (newGamePlus && timeMenu > 0.4f)
                GameManager.Instance.NewGamePlus();
            else
            {
                var device = obj.control.device;

                if (device is Gamepad)
                    startButton = "Pressione Start!";
                else if (device is Keyboard)
                    startButton = "Pressione Enter!";
                NewGamePlusMessage();
            }
        }
    
        private void NewGamePlusMessage()
        {
            if (!canMenu)
                return;
            if (fakeEnd)
                SceneManager.LoadScene("MainMenu");
            else
            {
                Debug.Log("[Credit] newGamePlus");
                isEndText.gameObject.SetActive(true);
                isEndText.text = $"NOVO JOGO+\n\r{startButton}";
                newGamePlus = true;
                timeMenu = 0;
            }

        }
        private string GetStatesString()
        {
            string result = string.Empty;
            TimeSpan time = TimeSpan.FromSeconds(GameManager.Instance.EnvironmentStates.GameTimeInSeconds);
            string text = time.ToString(@"hh\:mm\:ss");
            result = $"Estatisticas\n\r\n\r*Concluído {GameManager.Instance.EnvironmentStates.run}\n\r*Tempo de Jogo {text}\n\r*Mortes {GameManager.Instance.PlayerStates.numDies}\n\r " +
                $"* Maçãs {GameManager.Instance.PlayerStates.CollectablesID.Count}/56\n\r*Corações {GameManager.Instance.PlayerStates.Hearts}/{GameManager.Instance.GameTotalHearts}\n\r" +
                $"* Espiritos {GetSpiritsStates()}";
            return result;
        }
        private string GetSpiritsStates()
        {
            int num = 0;
            num += GameManager.Instance.IsEventCompleted(GameEventName.FireBall) ? 1 : 0;
            num += GameManager.Instance.IsEventCompleted(GameEventName.LightningBolt) ? 1 : 0;
            num += GameManager.Instance.IsEventCompleted(GameEventName.RollingWind) ? 1 : 0;
            num += GameManager.Instance.IsEventCompleted(GameEventName.PurifyWater) ? 1 : 0;

            return $"{num}/3";

        }
        void Update()
        {
            if (showTurn && timer > turnOnTime)
            {
                timer = 0f;
                showTurn = false;
            }
            if (!showTurn && timer > turnOffTime)
            {
                timer = 0f;
                currentCompose++;
                showTurn = true;
            }
            if (currentCompose >= maxCompose)
            {
                currentCompose = 0;
                canMenu = true;
            }
            //if (showTurn)
            //{
            //    creditsCompose.transform.GetChild(currentCompose).gameObject.SetActive(showTurn);
            //    StartCoroutine(screenFader.FadeIn());

            //}
            //if(!showTurn)
            //{
            //    StartCoroutine(screenFader.FadeOut());
            creditsCompose.transform.GetChild(currentCompose).gameObject.SetActive(showTurn);
            // }
            timer += Time.deltaTime;
            totalTime += Time.deltaTime;
            timeMenu += Time.deltaTime;
            if (totalTime > 225f)
            {
                Exit();
                // StartCoroutine(Exit());
            }

        }
        public void MenuButton_clicked()
        {
            return;
            if (canMenu)
            {
                if (fakeEnd)
                    SceneManager.LoadScene("MainMenu");
                else
                    NewGamePlusMessage();
            }
        }
        void Exit()
        {
            void Handler()
            {
                screenFader.OnFadeOutCompleted -= Handler;
                MenuButton_clicked();
            }

            screenFader.OnFadeOutCompleted += Handler;
            screenFader.FadeOut(3f);
        }

    }
}
