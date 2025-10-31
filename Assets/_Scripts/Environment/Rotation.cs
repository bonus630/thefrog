using UnityEngine;

namespace br.com.bonus630.thefrog.Environment
{
    public class Rotation : MonoBehaviour
    {
        Vector3 currentScale;
        float time = 0;

        private void Start()
        {
            currentScale = transform.localScale;
        }
        void Update()
        {
            float ping = Mathf.PingPong(time,2f);
            Vector3 v = new Vector3(currentScale.x - ping, currentScale.y - ping, currentScale.z);
            if (time>0.5f)
            {
                time = 0;

            }
            time += Time.deltaTime; 
            transform.localScale = v;
        }
    }
}
