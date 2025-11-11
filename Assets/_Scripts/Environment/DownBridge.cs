using UnityEngine;
using br.com.bonus630.thefrog.Shared;
using br.com.bonus630.thefrog.Manager;

namespace br.com.bonus630.Environment
{
    public class DownBridge : IActivator
    {

        private int chainAngleOffset = 0;
        public float speed = 0.1f;
        private int direction = 1;
        private float bridgeBodyFineAngleAjust = -0.69f;

        private Vector3 wallClosedPos;
        private Vector3 wallOpenedPos;
        Vector3 wallInitialOffset;

        [Header("Referências")]
        public Transform body;                  // corpo da ponte
        public Transform chainConnectorBody;   // ponto de ancoragem no corpo
        public Transform chainConnectorWall;     // ponto fixo na parede
        public Transform chainConnectorWallPosA;     // ponto fixo inicial
        public Transform chainConnectorWallPosB;     // ponto fixo final
       // public Transform chain;                 // sprite ou objeto visual da corrente
        public float chainUnitLength = 1f;      // comprimento da sprite da corrente

        [Header("Animação da ponte")]
        public bool operating = false;
        public bool opened = false;                  // true = alvo 0°, false = alvo -90°
        public float angle = 0f;                     // ângulo atual da ponte
        public float rotationSpeedDegPerSec = 90f;   // velocidade de rotação em graus por segundo
        public float time = 1f;
        private float t = 0f;
        //private void init()
        //{
        //    // define posições inicial (fechada) e final (aberta) do ponto da parede
        //    wallClosedPos = chainConnectorWall.position;

        //    // calcula posição final quando a ponte estiver aberta
        //    float angleRad = opened ? 0f : -90f; // ângulo de abertura
        //    Vector3 offset = chainConnectorBody.position - chainConnectorWall.position;
        //    wallOpenedPos = chainConnectorBody.position + offset; // ajuste se necessário
        //}

 
         Vector3 start, end;
        void Update()
        {
            if (!operating) return;

            // 1) Atualiza ângulo do corpo
            float targetAngle = opened ? 0f : -90f;

            angle = Mathf.MoveTowards(angle, targetAngle, rotationSpeedDegPerSec * Time.deltaTime);
            body.rotation = Quaternion.Euler(0f, 0f, angle);
            float angleRad = angle * Mathf.Deg2Rad;
            t += Time.deltaTime / time; // aumenta proporcional ao tempo
            t = Mathf.Clamp01(t);           // garante que fique entre 0 e 1

          

            chainConnectorWall.position = Vector3.Lerp(start, end, t);
            //Debug.Log("V:" + chainConnectorWall.position);
           //// 4) Finaliza operação
            if (Mathf.Approximately(angle, targetAngle))
            {
                angle = targetAngle;
                operating = false;
                //chainConnectorWall.position = opened ? wallOpenedPos : wallClosedPos;
            }

        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(chainConnectorBody.position, chainConnectorWall.position);
            float a =Vector3.Angle(chainConnectorBody.position, chainConnectorWall.position);
            
            //Debug.Log("Angle: " + a * Mathf.Rad2Deg);
        }



        // public Vector3 chainOffset = new Vector3(1, 1, 0);

        private void Start()
        {
            rotationSpeedDegPerSec = 90 / time;
            if (opened)
            {
                start = chainConnectorWallPosB.position;
                end = chainConnectorWallPosA.position;
            }
            else
            {
                start = chainConnectorWallPosA.position;
                end = chainConnectorWallPosB.position;
            }
            //offset = chainConnectorBody.position - chainConnectorWall.position;
            //if (opened)
            //    angle = 0;
            //else
            //    angle = -90;
            wallInitialOffset = chainConnectorWall.position - chainConnectorBody.position;
        }

        //void UpdateOld()
        //{
        //    if(operating)
        //    {
        //        if(opened)
        //        {
        //            angle++;
        //            direction = -1;
        //            if (angle >= 0)
        //            {
        //                angle = 0;
        //                operating = false;
        //            }
        //        }
        //        else
        //        {
        //            angle--;
        //            direction = 1;
        //            if (angle <= -90)
        //            {
        //                angle = -90;
        //                operating = false;
        //            }
        //        }
        //        chain.transform.position += (new Vector3(direction, direction, 0)).normalized * speed * Time.deltaTime;
        //        body.transform.rotation = Quaternion.Euler(0, 0, angle);

        //        // Vector3 worldOffset = body.transform.TransformDirection(chainOffset);
        //       // chain.transform.rotation = Quaternion.Euler(0, 0, angle + chainAngleOffset);
        //    }
        //}

        //void Update()
        //{
        //    if (operating)
        //    {
        //        float targetAngle = opened ? 0f : -90f; // alvo
        //        angle = Mathf.MoveTowards(angle, targetAngle, speed * Time.deltaTime * 90f);
        //        // speed controla o tempo que leva para mudar

        //        // Atualiza direção para o movimento da corrente
        //        direction = opened ? -1 : 1;

        //        // Move a corrente
        //       // chain.transform.position += (new Vector3(direction, direction, 0)).normalized * speed * Time.deltaTime;

        //        // Aplica rotação no corpo
        //        body.transform.rotation = Quaternion.Euler(0, 0, angle);

        //        // Se chegou no destino, para de operar
        //        if (Mathf.Approximately(angle, targetAngle))
        //        {
        //            angle = targetAngle;
        //            operating = false;
        //        }
        //    }
        //}



        public override void Activate()
        {
           // Debug.Log("Ponto ativa, abrindo");
            opened = true;
            operating = true;
            t = 0f;
          //  init();
            try
            {
                if(!GameManager.Instance.IsEventCompleted(GameEventName.LightningBolt))
                    FindAnyObjectByType<CamerasController>().GameObjectFocus(gameObject, 2);
            }
            catch { }
        }

        public override void Deactive()
        {
           // Debug.Log("Ponto desativada, fechando");
            opened = false;
            operating = true;
            t = 0f;
           // init();
            try
            {
                if (!GameManager.Instance.IsEventCompleted(GameEventName.LightningBolt))
                    FindAnyObjectByType<CamerasController>().GameObjectFocus(gameObject, 2);
            }
            catch { }
        }
    }
}
