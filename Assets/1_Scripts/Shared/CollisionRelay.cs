using System;
using Unity.VisualScripting;
using UnityEngine;

namespace br.com.bonus630.thefrog.Shared
{
    public class CollisionRelay : MonoBehaviour
    {

        public event Action<Collision2D> OnCollisionEnterAction;
        public event Action<Collider2D> OnTriggerEnterAction;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            
            OnCollisionEnterAction?.Invoke(collision);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            OnTriggerEnterAction?.Invoke(collision);
        }
    }
}
