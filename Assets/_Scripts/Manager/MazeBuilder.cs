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
        
        [SerializeField] IActivator entrace;
        [SerializeField] IActivator exit;
        [SerializeField] GameObject[] teleportPoints;
        [SerializeField] GameObject[] exitPoints;
        [SerializeField] ScreenFader fader;
        [SerializeField] GameObject Probs;
        [SerializeField] GameObject Enemies;

        public bool Completed { get; private set; }
        int current = 0;
        bool blocked = false;
        
        Vector3 newPos;
        private void Start()
        {
            fader.fadeDuration = 0.4f;
            Randomize(Probs);
            Randomize(Enemies);
            //entrace.GetComponent<Collider2D>().enabled = false;
            for (int i = 0; i < teleportPoints.Length; i++)
            {
                teleportPoints[i].GetComponent<CollisionRelayEx>().OnTriggerEnterAction += CheckTriggerEnter;
                teleportPoints[i].GetComponent<CollisionRelayEx>().OnTriggerExitAction += CheckTriggerExit;
            }
        }
        public void ActiveEntrace()
        {
            entrace.GetComponent<Collider2D>().enabled = true;
        }
        private void ChangeCurrent(int _value)
        {

        }
        private void CheckTriggerExit(ColliderData data)
        {
            if (data.ColliderOther.CompareTag("Player"))
            {
                Debug.Log("Data.index trigger exiti: " + data.Index);
                blocked = false;
            }
        }
        private void CheckTriggerEnter(ColliderData data)
        {
            if (blocked)
                return;

            if (!data.ColliderOther.CompareTag("Player"))
                return;

            blocked = true;
           // Debug.Log("Data.index: " + data.Index);

            bool isCorrect = CorrectPath[current] == data.Index;

            if (isCorrect)
            {
                current++;

                if (current == CorrectPath.Count)
                {
                    Exit();
                    return;
                }
            }
            else
            {
                // Jogador errou o caminho. Com 50% de chance, faz rollback (volta 1 passo)
                if (current > 0 &&
                    UnityEngine.Random.Range(0, 2) == 0 &&
                    CorrectPath[current - 1] != data.Index)
                {
                    current--; // ← rollback aqui
                    Entrace(); // volta ao ponto de entrada
                    //StartCoroutine(ScreenFader(data.Collider.gameObject));
                    //return;
                }
            }

            // Teleporta para uma nova posição aleatória (exceto a porta de entrada atual)
            List<GameObject> points = exitPoints.ToList();
            points.RemoveAt(data.Index);

            newPos = points[UnityEngine.Random.Range(0, points.Count)].transform.position;

            StartCoroutine(ScreenFader(data.ColliderOther.gameObject));
        }

        private IEnumerator ScreenFader(GameObject obj)
        {
            yield return fader.FadeOut();
            obj.transform.position = newPos;
            Randomize(Probs);
            Randomize(Enemies);
            yield return fader.FadeIn();

        }

        private void Randomize(GameObject o)
        {
            for (int i = 0; i < o.transform.childCount; i++)
            {
                o.transform.GetChild(i).gameObject.SetActive(UnityEngine.Random.Range(0, 2) == 0 ? true : false);
            }
        }

        //preciso criar um game object para cada um dos metodos, com scene mover!
        private void Exit()
        {
            Completed = true;
            exit.Activate();
        }
        private void Entrace()
        {
            entrace.Activate();
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
