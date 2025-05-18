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
        [SerializeField] InputAction MenuAction;

        float timer = 0f;
        float turnOnTime = 6f;
        float turnOffTime = 2f;
        float totalTime = 0;
        int currentCompose = 0;
        int maxCompose = 0;
        bool showTurn = true;
        bool canMenu = false;
       
        void Start()
        {
            maxCompose = creditsCompose.transform.childCount;
            MenuAction.Enable();
            MenuAction.performed += MenuAction_performed;
            states.text = GetStatesString();
        }
        private void OnDestroy()
        {
            MenuAction.performed -= MenuAction_performed;
        }
        private void MenuAction_performed(InputAction.CallbackContext obj)
        {
            if (canMenu)
                SceneManager.LoadScene("MainMenu");
        }
        private string GetStatesString()
        {
            string result = string.Empty;
            TimeSpan time = TimeSpan.FromSeconds(GameManager.Instance.EnvironmentStates.GameTimeInSeconds);
            string text = time.ToString(@"hh\:mm\:ss");
            result = $"Estatisticas\n\r\n\r*Tempo de Jogo {text}\n\r*Mortes {GameManager.Instance.PlayerStates.numDies}\n\r " +
                $"* Maçãs {GameManager.Instance.PlayerStates.Collectables}/54\n\r*Corações {GameManager.Instance.PlayerStates.Hearts}/12\n\r" +
                $"* Espiritos {(GameManager.Instance.PlayerStates.HasFireball ? 1 : 0)}/1";
            return result;
        }
        void Update()
        {
            if(showTurn && timer > turnOnTime)
            {
                timer = 0f;
                showTurn = false;
            }
            if(!showTurn && timer > turnOffTime)
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
            if(totalTime > 237.5f)
            {
                StartCoroutine(Exit());
            }
            
        }
        public void MenuButton_clicked()
        {
            if(canMenu)
                SceneManager.LoadScene("MainMenu");
        }
        IEnumerator Exit()
        {
            screenFader.FadeOut();
            yield return new WaitForSeconds(1);
            MenuButton_clicked();

        }
    }
}
