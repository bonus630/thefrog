using UnityEngine;

namespace br.com.bonus630.thefrog
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
           // Debug.Log("[Rotation] time: " + time);
          //  float clamp = Mathf.Clamp01(time);
           // Debug.Log("[Rotation] clamp: " + clamp);
            float ping = Mathf.PingPong(time,2f);
         //   Debug.Log("[Rotation] ping: " + ping);
            //Vector3 v = new Vector3(currentScale.x - 0.5f, currentScale.y - 0.5f, currentScale.z);
            Vector3 v = new Vector3(currentScale.x - ping, currentScale.y - ping, currentScale.z);
            if (time>0.5f)
            {
                time = 0;

            }
            time += Time.deltaTime; 
            //Mathf.PingPong
            transform.localScale = v;
            // Vector3 toScale = Vector3.Lerp(currentScale, v, ping);
            //transform.localScale = toScale;
        }
    }
}
