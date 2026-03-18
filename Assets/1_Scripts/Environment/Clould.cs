using br.com.bonus630.thefrog.Shared;
using br.com.bonus630.thefrog.Utils;
using UnityEngine;

namespace br.com.bonus630.thefrog.Environment
{
    public class Clould : MonoBehaviour
    {

        [SerializeField] float speed;

        float leftX = -180f;
        float rightX = 380;
        float time = 0;
        bool transition = false;
        float alpha = 0;
        float sAlpha;
        float tempAlpha;
        float sTime, eTime;


        SpriteRenderer sr;
        IHourProvider hourProvider;
        void Start()
        {
            sr = GetComponent<SpriteRenderer>();
            sAlpha = sr.color.a;
            tempAlpha = sAlpha;
            hourProvider = ServiceLocator.Instance.Get<IHourProvider>();
            hourProvider.OnHourChanged += HourProvider_OnHourChanged;
            HourProvider_OnHourChanged(hourProvider.Hour);
        }


        void FixedUpdate()
        {
            OverlayMovement();
        }
        float a;
        void OverlayMovement()
        {
            if (transition)
            {
                time = Mathf.InverseLerp(sTime, eTime, hourProvider.cycleTime);
                a = Mathf.Lerp(tempAlpha, alpha, time);
                sr.SetAlpha(a);
            }

            transform.position += Vector3.left * Time.deltaTime * speed;
            //leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
            //rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
            if (transform.position.x < leftX)
                transform.position = new Vector3(rightX, transform.position.y, 0);
        }
        private void HourProvider_OnHourChanged(int obj)
        {
            if (hourProvider.IsInRange(6, 8))
            {
                sTime = hourProvider.HourToCycleTime(6);
                eTime = hourProvider.HourToCycleTime(8);
                alpha = 1;
                transition = true;
            }
            else if (hourProvider.IsInRange(18, 20))
            {
                sTime = hourProvider.HourToCycleTime(18);
                eTime = hourProvider.HourToCycleTime(20);
                alpha = 0;
                transition = true;
            }
            else if (hourProvider.IsInRange(21, 5))
            {
                transition = false;
                time = 0;
                tempAlpha = 0;
                sr.SetAlpha(0);
            }
            else if(hourProvider.IsInRange(9,17))
            {
                transition = false;
                time = 0;
                tempAlpha = sAlpha;
                sr.SetAlpha(1);
            }

        }
        private void OnDisable()
        {
            hourProvider.OnHourChanged -= HourProvider_OnHourChanged;
        }

    }
}
