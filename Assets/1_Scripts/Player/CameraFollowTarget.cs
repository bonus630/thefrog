using UnityEngine;

namespace br.com.bonus630.thefrog.Player
{

    public class CameraFollowTarget : MonoBehaviour
    {
        public Player player;
        public float minVelocityY = -15f;
        public float maxVelocityY = 15f;

        [Header("Animation Curve")]

        public AnimationCurve velocityX;
        public AnimationCurve velocityY;


        float limitTime = 1.5f;
        float timerX = 0;
        float timerY = 0;

        float playerLookFor;
        float playerFallDirection;
        float offsetX;
        float offsetY;
        float rbSpeedTolerance = 1f;

        private void Start()
        {
            playerLookFor = player.LookFor;
            playerFallDirection = Mathf.Sign(player.RigibodyLinearVelocityY);
        }

        void LateUpdate()
        {
            CalcOffsetX();
            CalcOffsetY();
            transform.position = player.transform.position + (new Vector3(offsetX, offsetY, 0));
        }

        private void CalcOffsetY()
        {
            float rbY = player.RigibodyLinearVelocityY;

            float signY = Mathf.Sign(rbY);

            if (Mathf.Abs(rbY) < rbSpeedTolerance)
            {
                playerFallDirection = 0;
                timerY = 0;
            }
            else
            {
                if (signY == playerFallDirection)
                {
                    timerY += Time.deltaTime;
                }
                else
                {
                    timerY -= Time.deltaTime;
                    playerFallDirection = signY;
                }

                timerY = Mathf.Clamp(timerY, 0, limitTime);
            }

            offsetY = velocityY.Evaluate(timerY) * playerFallDirection;
        }

        private void CalcOffsetX()
        {
            if (playerLookFor == player.LookFor)
            {
                timerX += Time.deltaTime;
            }
            else
            {
                timerX -= Time.deltaTime;
            }
            if (timerX < 0)
            {
                timerX = 0;
                playerLookFor *= -1;
            }
            if (timerX > limitTime)
            {
                timerX = limitTime;
            }
            offsetX = velocityX.Evaluate(timerX) * playerLookFor;

        }
    }

}
