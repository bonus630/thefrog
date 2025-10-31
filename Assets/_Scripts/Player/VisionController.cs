using System;
using System.Collections;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using br.com.bonus630.thefrog.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace br.com.bonus630.thefrog.Player
{
    public class VisionController : MonoBehaviour
    {
        [SerializeField] float visionTime = 10f;
        [SerializeField] float maxOffSetY = 5f;
        float nextVisionTime = 0;
        bool activeVision = false;
        float activeVisionBarValue = 0;
        float ElapseTime = 0f;
        float gravityDirection;
        IBarUI activeVisionBar;
        float maxValue = 100;

        float offsetY = 0;

        InputAction interactUp;
        InputAction interactDown;
        ScreenEffects effects;
        private void Start()
        {
            var input = ServiceLocator.Instance.Get<PlayerInput>();
            var map = input.actions.FindActionMap("Global", true);
            interactUp = map.FindAction("InteractUp", true);
            interactDown = map.FindAction("InteractDown", true);
            interactUp.Enable();
            interactDown.Enable();
            effects = ServiceLocator.Instance.Get<ScreenEffects>();
            ServiceLocator.Instance.Get<IPlayer>().GravityChanged += VisionController_GravityChanged;
        }

        private void VisionController_GravityChanged(float obj)
        {
            this.gravityDirection = obj;
            Debug.Log("graviti" + obj);
        }

        private void Update()
        {
            if (activeVision)
            {
                switch (true)
                {
                    case bool _ when interactUp.IsPressed():

                        offsetY += 0.05f; 
                        offsetY = Mathf.Clamp(offsetY, -maxOffSetY, maxOffSetY);
                        break;
                    case bool _ when interactDown.IsPressed():

                        offsetY -= 0.05f; 
                        offsetY = Mathf.Clamp(offsetY, -maxOffSetY, maxOffSetY);
                        break;
                    default:
                        if (offsetY > 0)
                            offsetY -= 0.1f;
                        else if(offsetY < 0)
                            offsetY += 0.1f;
                        if (offsetY >= -0.1f && offsetY <= 0.1f)
                            offsetY = 0;
                    break;
                }
                MoveVision();
            }
            
        }
        private void OnDestroy()
        {
            ServiceLocator.Instance.Get<IPlayer>().GravityChanged -= VisionController_GravityChanged;
        }
        private void MoveVision()
        {
            Vector2 offset;
            if (activeVision)
                offset = Vector2.up  * offsetY;
            else
                offset = Vector2.zero;
            effects.CameraOffSet(offset);
            transform.localPosition = offset / 2 * -gravityDirection;


        }
        public void ActiveVision(BarManager barManager, float gravityDirection)
        {
            this.gravityDirection = gravityDirection;
            if (Time.time > nextVisionTime && !activeVision)
            {
                //Debug.Log("[Player] visionBar ElapseTime:" + ElapseTime);
               //Debug.Log("[Player] visionBar minValue:" + activeVisionBarValue);
                activeVisionBar = barManager.CreateBar(Color.magenta, 0, barManager.transform, gravityDirection);
                activeVisionBar.BarDestroyed += ActiveVisonBar_BarDestroyed;
                activeVisionBar.MaxValue = maxValue;
                activeVisionBar.GoToValue(activeVisionBarValue, 0);
                activeVisionBar.GoToValue(100, visionTime - ElapseTime);
                activeVisionBar.DestroyBar(visionTime - ElapseTime);
                nextVisionTime = (Time.time + visionTime) - ElapseTime;
                activeVision = true;
                GetComponent<SpriteMask>().enabled = true;
                GetComponent<SpriteRenderer>().enabled = true;
                return;
            }
            if (activeVision)
            {
                activeVisionBar.DestroyBar();
            }
        }

        private void ActiveVisonBar_BarDestroyed(GameObject arg1, bool arg2)
        {
            activeVisionBar.BarDestroyed -= ActiveVisonBar_BarDestroyed;
            activeVisionBarValue = activeVisionBar.CurrentValue;
            ElapseTime = activeVisionBar.ElapsedTime;
            activeVision = false;
            if (activeVisionBarValue >= activeVisionBar.MaxValue)
                ElapseTime = visionTime;
            //{
            //    ResetVision();
            //}
            //else
            StartCoroutine(VisionRecover());
            nextVisionTime = 0;
            MoveVision();
            //Debug.Log("[Player] visionBar minValue:" + activeVisionBarValue);
            GetComponent<SpriteMask>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
        }
        //private void ResetVision()
        //{
        //    ElapseTime = 0;
        //    activeVisionBarValue = 0;
        //}
        private IEnumerator VisionRecover()
        {
            while (activeVisionBarValue > 0 && !activeVision)
            {
                yield return new WaitForSeconds(0.5f);
                activeVisionBarValue--;
                ElapseTime -= visionTime / maxValue;
                if (ElapseTime < 0)
                    ElapseTime = 0;
                if (activeVisionBarValue < 0)
                    activeVisionBarValue = 0;
               // Debug.Log("[Player] visionBar minValue:" + activeVisionBarValue);
               // Debug.Log("[Player] visionBar ElapseTime:" + ElapseTime);
            }
        }
    }
}
