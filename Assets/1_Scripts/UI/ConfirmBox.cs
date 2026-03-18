using System;
using System.Collections;
using System.Linq;
using br.com.bonus630.thefrog.Manager;
using UnityEngine;

namespace br.com.bonus630.thefrog.UI
{
    public class ConfirmBox : MonoBehaviour
    {
        [SerializeField]SaveLoadMenu menu;
        
        // [SerializeField] GameObject SaveBoxUI;
        [SerializeField] GameObject confirmBoxUI;

     

        private void Start()
        {
            menu.SaveSucess += ConfirmCallBack;
        }
        private void OnEnable()
        {
        }
        private void OnDisable()
        {
            menu.SaveSucess -= ConfirmCallBack;
        }
        private void ConfirmCallBack()
        {
            StartCoroutine(ConfirmSave());
        }
        IEnumerator ConfirmSave()
        {
           // Debug.Log("123 " + menu.gameObject);
            menu.transform.parent.gameObject.SetActive(false);
            confirmBoxUI.SetActive(true);
            yield return new WaitForSecondsRealtime(1.5f);
            confirmBoxUI.SetActive(false);
            GameManager.Instance.OnCallSave(true);
            
        }
    }
}
