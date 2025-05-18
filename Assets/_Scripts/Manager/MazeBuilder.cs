using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;
namespace br.com.bonus630.thefrog.Manager
{
    public class MazeBuilder : MonoBehaviour
    {
        [field: SerializeField] public List<int> CorrectPath { get; set; }

        [SerializeField] GameObject entrace;
        [SerializeField] GameObject exit;
        [SerializeField] GameObject[] teleportPoints;
        [SerializeField] GameObject[] exitPoints;

        int current = 0;
        bool blocked = false;
        ScreenFader fader;
        Vector3 newPos;
        private void Start()
        {
            fader = FindAnyObjectByType<ScreenFader>();
            fader.fadeDuration = 0.4f;
            entrace.GetComponent<CircleCollider2D>().enabled = false;
            for (int i = 0; i < teleportPoints.Length; i++)
            {
                teleportPoints[i].GetComponent<CollisionRelayEx>().OnTriggerEnterAction += CheckTriggerEnter;
                teleportPoints[i].GetComponent<CollisionRelayEx>().OnTriggerExitAction += CheckTriggerExit;
            }
        }
        public void ActiveEntrace()
        {
            entrace.GetComponent<CircleCollider2D>().enabled = true;
        }
        private void ChangeCurrent(int _value)
        {

        }
        private void CheckTriggerExit(ColliderData data)
        {
            if (data.Collider.CompareTag("Player"))
            {
                Debug.Log("Data.index trigger exiti: " + data.Index);
                blocked = false;
            }
        }
        private void CheckTriggerEnter(ColliderData data)
        {
            if (blocked)
                return;
            if (data.Collider.CompareTag("Player"))
            {
                blocked = true;
                Debug.Log("Data.index: " + data.Index);
                if (CorrectPath[current] == data.Index)
                {
                    current++;
                }
                if (current == CorrectPath.Count)
                    newPos = exit.transform.position;
                else
                {
                    List<GameObject> points = exitPoints.ToList();
                    points.RemoveAt(data.Index);
                    if (UnityEngine.Random.Range(0, 2) == 0 && CorrectPath[current - 1] != data.Index)
                    {
                        newPos = entrace.transform.position;
                        current = 0;
                    }
                    else
                        newPos = points[UnityEngine.Random.Range(0, points.Count)].transform.position;
                }
                StartCoroutine(ScreenFader(data.Collider.gameObject));
                // data.Collider.gameObject.transform.position = newPos;
                // 
            }
        }
        private IEnumerator ScreenFader(GameObject obj)
        {
            yield return fader.FadeOut();
            obj.transform.position = newPos;
            yield return fader.FadeIn();

        }
    }
    public enum MazeDirections
    {
        Esquerda = 0,
        Cima = 1,
        Direita = 2,
        Baixo = 3
    }
}
