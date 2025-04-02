using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
namespace br.com.bonus630.thefrog.DialogueSystem
{
    public class TextAnimation : MonoBehaviour
    {
        private float textTimer = 0.05f;

        [SerializeField] TextMeshProUGUI text;
        public string FullText { get; set; }


        Coroutine coroutine;
        public bool Finish { get; protected set; } = true;

        void Start()
        {

        }
        private void Update()
        {


        }
        public void StartTyping()
        {

            Finish = false;
            coroutine = StartCoroutine(Animation());
        }
        public void Skip()
        {
            StopCoroutine(coroutine);
            text.maxVisibleCharacters = FullText.Length;
            Finish = true;
        }
        IEnumerator Animation()
        {
            text.text = FullText;
            text.maxVisibleCharacters = 0;
            for (int i = 0; i <= text.text.Length; i++)
            {
                text.maxVisibleCharacters = i;
                yield return new WaitForSeconds(textTimer);
            }
            //if (text.maxVisibleCharacters == text.text.Length)
            //    Finish = true;
        }

    }
}
