using UnityEngine;
using br.com.bonus630.thefrog.Shared;
using br.com.bonus630.thefrog.Manager;

namespace br.com.bonus630.Environment
{
    internal class DownBridge : IActivator
    {

        [SerializeField] GameObject body;
        [SerializeField] GameObject chain;
        

        [SerializeField] bool opened  = false;
        [SerializeField]private bool operating = false;
        private int angle = 0;
        private int chainAngleOffset = 0;
        public float speed = 0.1f;
        private int direction = 1;
        // public Vector3 chainOffset = new Vector3(1, 1, 0);

        private void Start()
        {
            if (opened)
                angle =0;
            else
                angle = -90;
        }

        void Update()
        {
            if(operating)
            {
                if(opened)
                {
                    angle++;
                    direction = -1;
                    if (angle >= 0)
                        operating = false;
                }
                else
                {
                    angle--;
                    direction = 1;
                    if(angle <= -90)
                        operating = false;
                }
                chain.transform.position += (new Vector3(direction, direction, 0)).normalized * speed * Time.deltaTime;
                body.transform.rotation = Quaternion.Euler(0, 0, angle);
                // Vector3 worldOffset = body.transform.TransformDirection(chainOffset);
               // chain.transform.rotation = Quaternion.Euler(0, 0, angle + chainAngleOffset);
            }
        }




        public override void Activate()
        {
            opened = true;
            operating = true;
            FindAnyObjectByType<CamerasController>().GameObjectFocus(gameObject,2);
        }

        public override void Deactive()
        {
            opened = false;
            operating = true;
            FindAnyObjectByType<CamerasController>().GameObjectFocus(gameObject,2);
        }
    }
}
