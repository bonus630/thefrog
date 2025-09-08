using System;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Environment
{
    public class RustStairs : MonoBehaviour
    {
        [SerializeField] Theme stairTheme = Theme.Neutral;
        [SerializeField] [Range(-1,1)] int direction = 1;
        [SerializeField] IActivator teleporter;
        [SerializeField] GameObject Teleported;
        // Start is called once before the first execution of Update after the MonoBehaviour is created

        private void Awake()
        {
            
        }
        private void OnDestroy()
        {
                gameObject.transform.GetChild((int)stairTheme).GetComponent<CollisionRelayEx>().OnTriggerEnterAction -= RustStairs_OnTriggerEnterAction;
        }
        private void RustStairs_OnTriggerEnterAction(ColliderData obj)
        {
            if(obj.ColliderOther.CompareTag(Teleported.tag))
                teleporter.Activate();
        }

        void Start()
        {
            SetTheme(stairTheme);
        }
        private void OnValidate()
        {
            for (int i = 0; i < Enum.GetValues(typeof(Theme)).Length; i++)
            {
                gameObject.transform.GetChild(i).gameObject.SetActive(false);
            }
            SetTheme(stairTheme);
        }

        private void SetTheme(Theme theme)
        {
            gameObject.transform.GetChild((int)theme).gameObject.SetActive(true);
            if(direction!=0)
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x).FlipIfNegative(direction), transform.localScale.y);
            gameObject.transform.GetChild((int)theme).GetComponent<CollisionRelayEx>().OnTriggerEnterAction += RustStairs_OnTriggerEnterAction;
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
