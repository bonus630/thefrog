using System;
using UnityEngine;

namespace br.com.bonus630.thefrog.Shared
{
    public class CollisionRelayEx : MonoBehaviour
    {
        [SerializeField] public int index;
        [SerializeField] string colliderName;


        ColliderData data;
        private void Start()
        {
            data = new ColliderData(gameObject, index, colliderName);
        }


        public event Action<ColliderData> OnTriggerEnterAction;
        public event Action<ColliderData> OnTriggerExitAction;
     
        public void OnTriggerEnter2D(Collider2D collision)
        {
            data.ColliderOther = collision;
            OnTriggerEnterAction?.Invoke(data);
        }
        public void OnTriggerExit2D(Collider2D collision)
        {
            data.ColliderOther = collision;
            OnTriggerExitAction?.Invoke(data);
        }

    }
    public class ColliderData
    {
        public Collider2D ColliderOther { get; set; }
        public GameObject GameObjectOwner { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }

        public ColliderData(GameObject gameObject, int index, string name)
        {
            
            GameObjectOwner = gameObject;
            Index = index;
            Name = name;
        }
    }
}
