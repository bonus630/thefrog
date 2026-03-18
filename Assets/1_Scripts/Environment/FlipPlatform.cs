using System.Collections.Generic;
using UnityEngine;
namespace br.com.bonus630.thefrog.Environment
{
    public class FlipPlatform : MonoBehaviour
    {
        [SerializeField] GameObject[] points;
        [SerializeField] float maxTime = 2f;
        public GameObject[] Points { get { return points; } set { points = value; } }
        public float MaxTime { get { return maxTime; } set { maxTime = value; } }

        public int StartPoint { get; set; } = 0;

        Vector2 startPoint;
        Vector2 endPoint;

        float time = 0f;
        int i = 0;
        private void Start()
        {
            i = StartPoint;
        }


        private void Update()
        {
            startPoint = Points[i].transform.position;
            if (i == Points.Length - 1)
                endPoint = Points[0].transform.position;
            else
                endPoint = Points[i + 1].transform.position;
            Vector2 result = Vector2.Lerp(startPoint, endPoint, time);
            transform.position = result;
            // Debug.Log("i: " + i + " X: " + result.x + " Y: " + result.y);

            time += Time.deltaTime;
            if (time > MaxTime)
            {
                time = 0;
                i++;
                if (i == Points.Length)
                    i = 0;
            }
        }
    }
}
