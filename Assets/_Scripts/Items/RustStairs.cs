using System;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;
using UnityEngine.InputSystem;

namespace br.com.bonus630.thefrog.Items
{
    public class RustStairs : MonoBehaviour
    {
        [SerializeField] Theme stairTheme = Theme.Neutral;
        [SerializeField] [Range(-1,1)] int direction = 1;
        [SerializeField] IActivator teleporter;
        [SerializeField] GameObject Teleported;
        
        InputSystem_Actions GlobalActions;
        private bool inside = false;

        
        private void OnValidate()
        {
            for (int i = 0; i < Enum.GetValues(typeof(Theme)).Length; i++)
            {
                gameObject.transform.GetChild(i).gameObject.SetActive(false);
            }
            SetTheme(stairTheme);
        }
        private void Awake()
        {
            GlobalActions = new InputSystem_Actions();
            GlobalActions.Enable();
        }
        void Start()
        {
            SetTheme(stairTheme);
          
        }
        private void Update()
        {
            if (GlobalActions.Global.InteractUP.WasPressedThisFrame() && inside)
                teleporter.Activate();
        }
        private void OnDestroy()
        {
                gameObject.transform.GetChild((int)stairTheme).GetComponent<CollisionRelayEx>().OnTriggerEnterAction -= RustStairs_OnTriggerEnterAction;
                gameObject.transform.GetChild((int)stairTheme).GetComponent<CollisionRelayEx>().OnTriggerExitAction -= RustStairs_OnTriggerExitAction;
        }
        private void RustStairs_OnTriggerEnterAction(ColliderData obj)
        {
            if(obj.ColliderOther.CompareTag(Teleported.tag))
                inside = true;
                //teleporter.Activate();
        }
        private void RustStairs_OnTriggerExitAction(ColliderData obj)
        {
            if(obj.ColliderOther.CompareTag(Teleported.tag))
                inside = false;
                //teleporter.Activate();
        }


        private void SetTheme(Theme theme)
        {
            gameObject.transform.GetChild((int)theme).gameObject.SetActive(true);
            if(direction!=0)
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x).FlipIfNegative(direction), transform.localScale.y);
            gameObject.transform.GetChild((int)theme).GetComponent<CollisionRelayEx>().OnTriggerEnterAction += RustStairs_OnTriggerEnterAction;
            gameObject.transform.GetChild((int)theme).GetComponent<CollisionRelayEx>().OnTriggerExitAction += RustStairs_OnTriggerExitAction;
        }
    }
    public enum Theme
    {
        Orange = 0,
        Blue = 1,
        OrangeLarge=2,
        BlueLarge=3,
        Neutral=4,
        PrisonExit = 5,
        PrisonEntry=6
    }
}
