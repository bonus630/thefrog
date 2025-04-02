
using UnityEngine;
using UnityEngine.SceneManagement;
namespace br.com.bonus630.thefrog.Caracters
{
    public class NPCBacksmith : NPCBase, INPC
    {
        [SerializeField] AudioClip audioClip;
        [SerializeField] AudioSource audioSource;


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
        }
        public override void SetFinishDialogue()
        {
            SceneManager.LoadScene("Credit");
        }
    }
}

