using System;
using Unity.VisualScripting;
using UnityEngine;

namespace br.com.bonus630.thefrog.Shared
{
    public class CollisionRelayEx : MonoBehaviour
    {
        [SerializeField] int index;
        [SerializeField] string colliderName;


        ColliderData data;
        private void Start()
        {
            data = new ColliderData(gameObject, index, colliderName);
        }


        public event Action<ColliderData> OnTriggerEnterAction;
        public event Action<ColliderData> OnTriggerExitAction;
     
        private void OnTriggerEnter2D(Collider2D collision)
        {
            data.Collider = collision;
            OnTriggerEnterAction?.Invoke(data);
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            data.Collider = collision;
            OnTriggerExitAction?.Invoke(data);
        }
    }
    public class ColliderData
    {
        public Collider2D Collider { get; set; }
        public GameObject GameObject { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }

        public ColliderData(GameObject gameObject, int index, string name)
        {
            
            GameObject = gameObject;
            Index = index;
            Name = name;
        }
    }
}
