using System.Collections;
using System.Collections.Generic;
using br.com.bonus630.thefrog.Activators;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Environment
{
    public class WindTarget : MonoBehaviour
    {
        [SerializeField] PasswordGenerator passwordGenerator;
        [SerializeField] List<IActivator> candelablus;
        [SerializeField] CameraFocus cameraFocus;
        [SerializeField] AudioClip clip;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            //Debug.Log("Collision target: " + collision.gameObject.layer);
            if (collision.gameObject.layer == 12)
            {
                Debug.Log("Collision target");
                passwordGenerator.Activate();
                for (int i = 0; i < candelablus.Count; i++)
                {
                    candelablus[i].Deactive();
                }
                Destroy(collision.gameObject);
                ServiceLocator.Instance.Get<AudioEffects>().Play(clip);
                StartCoroutine(RunDemo());
            }
        }
        private IEnumerator RunDemo()
        {

           // Debug.Log(passwordGenerator.password.Count);
            List<GameObject> order = new List<GameObject>();

            for (int i = 0; i < passwordGenerator.password.Count; i++)
            {
                order.Add(candelablus[passwordGenerator.password[i]].gameObject);
            }
            cameraFocus.gameObjects = order.ToArray();
            cameraFocus.Activate();
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < passwordGenerator.password.Count; i++)
            {
                //cameraFocus.gameObjects = new GameObject[1] { candelablus[passwordGenerator.password[i]].gameObject };
                candelablus[passwordGenerator.password[i]].Activate();
                yield return new WaitForSeconds(1f);
                candelablus[passwordGenerator.password[i]].Deactive();
                yield return new WaitForSeconds(0.5f);
            }


        }
    }
}
