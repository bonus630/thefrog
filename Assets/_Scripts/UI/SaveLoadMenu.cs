using System;
using System.Collections;
using System.Collections.Generic;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace br.com.bonus630.thefrog.UI
{
    public class SaveLoadMenu : MonoBehaviour
    {
        [SerializeField] GameObject ObjectToEnableDisable;
        [SerializeField] Button goBackButton;
        [SerializeField] Button save01Button;
        [SerializeField] Button save02Button;
        [SerializeField] Button save03Button;
        [SerializeField] Button callerButton;
        [SerializeField] bool saveMode = true;

        public event Action SaveSucess;
        private List<SaveStates> list;
        
        private void OnEnable()
        {
            if (ObjectToEnableDisable != null && ObjectToEnableDisable.activeInHierarchy)
                ObjectToEnableDisable.SetActive(false);
            SavesManager sm = new SavesManager();
            list = sm.ListSaves();

            FillSaveButton(save01Button, list[0]);
            FillSaveButton(save02Button, list[1]);
            FillSaveButton(save03Button, list[2]);
            if (goBackButton.gameObject.activeInHierarchy)
                EventSystem.current.SetSelectedGameObject(goBackButton.gameObject);
            else
            {
                //Debug.Log("Ativando");
                EventSystem.current.SetSelectedGameObject(save01Button.gameObject);
            }
           // save01Button.onClick.AddListener(() => Save01Button_clicked());
        }
        //IEnumerator WaitAndSelect()
        //{
   
        //    while (EventSystem.current == null ||
        //           EventSystem.current.currentInputModule == null ||
        //           !save01Button.activeInHierarchy)
        //    {
        //        yield return null;
        //    }

        //    yield return new WaitForEndOfFrame(); // garante que layout UI foi atualizado
        //    EventSystem.current.SetSelectedGameObject(firstButton);
        //}
        private void OnDisable()
        {
            if(ObjectToEnableDisable!=null && !ObjectToEnableDisable.activeInHierarchy)
                ObjectToEnableDisable.SetActive(true);
            if (callerButton != null)
                EventSystem.current.SetSelectedGameObject(callerButton.gameObject);
            //save01Button.onClick.RemoveAllListeners();
      
        }
        public void Save01Button_clicked()
        {
            if (saveMode)
                Save(1);
            else
                LoadSave(1);
        }
        public void Save021Button_clicked()
        {
            if (saveMode)
                Save(2);
            else
                LoadSave(2);
        }
        public void Save03Button_clicked()
        {
            if (saveMode)
                Save(3);
            else
                LoadSave(3);
        }
        public void GoBackButton_clicked()
        {
            gameObject.SetActive(false);
        }
        public void GoBackButtonDisableParent_clicked()
        {
            GameManager.Instance.Pause(false);
           // gameObject.transform.parent.gameObject.SetActive(false);
        }
        private void LoadSave(int index)
        {
            //Debug.Log("Load");
            if (list[index-1]!=null)
                GameManager.Instance.LoadGame(SceneStartType.Continue, index);

        }
        private void Save(int index)
        {
           if(GameManager.Instance.SaveStates(index))
                SaveSucess?.Invoke();

            //gameObject.SetActive(false);
        }
    
        private void FillSaveButton(Button button, SaveStates saveStates)
        {
            GameObject thumb = button.gameObject.transform.GetChild(1).transform.GetChild(0).gameObject;
            GameObject time  = button.gameObject.transform.GetChild(1).transform.GetChild(1).gameObject;
            GameObject hours = button.gameObject.transform.GetChild(1).transform.GetChild(2).gameObject;
 //Debug.Log("saveStates: " + saveStates);
            if (saveStates != null)
            {
                ThumbGenerator thumbGenerator = new ThumbGenerator(0.1f);
                button.enabled = true;
                thumb.SetActive(true);
                hours.SetActive(true);
                Sprite sprite = thumbGenerator.DecodeThumb(saveStates.thumb);
               
                thumb.GetComponent<Image>().sprite = sprite;
                time.GetComponent<TextMeshProUGUI>().text = TimeSpan.FromSeconds(saveStates.environmentStates.GameTimeInSeconds).ToString(@"hh\:mm\:ss");
                hours.GetComponent<TextMeshProUGUI>().text = saveStates.environmentStates.playerStates.Hour.ToString("00") + " HORAS";
            }
            else
            {
                button.enabled = saveMode;
                thumb.SetActive(false);
                hours.SetActive(false);
                time.GetComponent<TextMeshProUGUI>().text = "NADA SALVO";
            }

        }
    }
}
