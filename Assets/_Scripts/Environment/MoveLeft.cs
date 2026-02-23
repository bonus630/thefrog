using System.Collections;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Environment
{
    public class MoveLeft : MonoBehaviour
    {
        [SerializeField] Vector3 speed;
        [SerializeField] float newScale = 1;
        float time = 0;
        float maxtime = 2;
        float scale = 1;
        bool resizing = false;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            ServiceLocator.Instance.Get<IHourProvider>().OnHourChanged += MoveLeft_OnHourChanged;
            Calc();
        }
        private void OnDisable()
        {
            ServiceLocator.Instance.Get<IHourProvider>().OnHourChanged -= MoveLeft_OnHourChanged;
        }
        private void MoveLeft_OnHourChanged(int obj)
        {
            Calc();
        }

        private void Calc()
        {
            GetComponent<SpriteRenderer>().sortingOrder = Random.Range(5, 12);
            scale = transform.localScale.x;
            newScale = Random.Range(0.5f , 3);
            float x = Random.Range(0,0.3f) * newScale;
            speed = new Vector3(x, Random.Range(0, 0.1f), 0);
            resizing = true;
        }
        void Resize()
        {
            float size = Mathf.Lerp(scale, newScale, time);
            transform.localScale = new Vector3(size,size,1);
            time += Time.deltaTime;
            if (time > maxtime)
            {
                time = 0;
                resizing = false;
            }

        }
        // Update is called once per frame
        void Update()
        {
            transform.position -= speed;
            if(resizing)
                Resize();
        }
    }
}
