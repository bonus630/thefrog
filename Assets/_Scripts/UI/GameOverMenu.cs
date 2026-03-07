using System.Collections;
using br.com.bonus630.thefrog.Manager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace br.com.bonus630.thefrog.UI
{
    public class GameOverMenu : MonoBehaviour
    {
        [SerializeField] GameObject buttons;
        [SerializeField] GameObject saves;


        private void OnEnable()
        {
            StartCoroutine(WaitAndSelect());
        }

        IEnumerator WaitAndSelect()
        {

            GameObject firstButton = transform.GetChild(0).GetChild(0).gameObject;
            // Espera até o EventSystem e o InputSystemUIInputModule estarem prontos
            while (EventSystem.current == null ||
                   EventSystem.current.currentInputModule == null ||
                   !firstButton.activeInHierarchy)
            {
                Debug.Log("[GameOverMenu] EventSystems");
                yield return null;
            }

            yield return new WaitForEndOfFrame(); // garante que layout UI foi atualizado
            EventSystem.current.SetSelectedGameObject(firstButton);
            //Debug.Log("[GameOverMenu] EventSystems: " + FindObjectsByType<EventSystem>(FindObjectsSortMode.None).Length);
           // Debug.Log("[GameOverMenu] Raycasters: " + FindObjectsByType<GraphicRaycaster>(FindObjectsSortMode.None).Length);
           // Debug.Log("[GameOverMenu] Cursor Lockstate: " + Cursor.lockState);
        }

        public void ContinueButton_clicked()
        {
           //Debug.Log("[GameOverMenu] continue button clicked");
            GameManager.Instance.LoadGame(SceneStartType.Continue);
        }
        public void LoadButton_clicked()
        {
            buttons.SetActive(false);
            saves.SetActive(true);
        }
        public void QuitButton_clicked()
        {
            Application.Quit();
        }
    }
}
