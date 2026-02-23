using System.Collections.Generic;
using System.Linq;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    
    public class PasswordReceiver : MonoBehaviour
    {
        [SerializeField] IActivator ItemToActive;
        [field: SerializeField] public List<int> Password { get; set; }
        List<int> received;
        [SerializeField] List<ActivatorSlot> activatorSlots;
        [field:SerializeField] public int ID { get; set; }

        AudioSource audioSource;
        private void Start()
        {
            received = new List<int>();
            audioSource = GetComponent<AudioSource>();
            for (int i = 0; i < activatorSlots.Count; i++)
            {
                activatorSlots[i].Activated += PasswordReceiver_Activated;
            }
            if (GameManager.Instance.IsActived(this.ID.ToString()))
            {
                ItemToActive.Activate();
            }
        }

        private void PasswordReceiver_Activated(int id, bool activade)
        {
            if (Password == null || Password.Count == 0)
                return;
            //Debug.Log($"id: {id} ativo: {activade}");
            if (activade && !received.Contains(id))
                received.Add(id);
            if (!activade && received.Contains(id))
                received.Remove(id);
            if (Password.SequenceEqual<int>(received))
            {
                if(audioSource!=null)
                    audioSource.Play();
                if (!ItemToActive.Actived)
                {
                    ItemToActive.Activate();
                    GameManager.Instance.SetActived(this.ID.ToString(),true);
                }
            }
            else
            {
                if (ItemToActive.Actived)
                {
                    ItemToActive.Deactive();
                    GameManager.Instance.SetActived(this.ID.ToString(), false);
                }
            }


        }
    }
}
