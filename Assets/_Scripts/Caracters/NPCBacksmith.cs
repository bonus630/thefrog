
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
namespace br.com.bonus630.thefrog.Caracters
{
    public class NPCBacksmith : NPCBase, INPC
    {
        [SerializeField] AudioClip audioClip;
        [SerializeField] AudioSource audioSource;
        [SerializeField] Light2D light2;

        public void CheckInitialDialogue(int dialogue)
        {
        }

        public override Transform GetTransform()
        {
            return transform;
        }

        public override void Interact()
        {
        }
        public void PlaySound()
        {
            audioSource.PlayOneShot(audioClip);
            //light2.gameObject.SetActive(true);
            //yield return new WaitForSeconds(0.05f);
            //light2.gameObject.SetActive(false);

        }
        //private void DisableLight()
        //{
        //    light2.gameObject.SetActive(false);
        //}
        public override void SetFinishDialogue()
        {
            dialogueCounter = 0;
            
        }
    }
}

