using UnityEngine;
using System;
using UnityEngine.Events;

namespace br.com.bonus630.thefrog.Shared
{
    public class Element : MonoBehaviour ,IElement
    {
        [SerializeField] Elements element;
        [SerializeField] Elements activeBy;
        [SerializeField] Elements deactiveBy;
        [SerializeField] Color elementColor;

        [SerializeField] UnityEvent active;
        [SerializeField] UnityEvent deactive;

        public Elements GetElement() => element;

        public Elements CanActiveBy() => activeBy;

        public Elements CanDeactiveBy() => deactiveBy;

        public void ActiveBy(Elements element)
        {
            active?.Invoke();
        }

        public void DeactiveBy(Elements element)
        {
            deactive?.Invoke();
        }

        public Color GetElementColor() => elementColor;
    }

    public enum Elements
    {
        Normal = 0,
        Fire = 1,
        Water = 2,
        Earth = 4,
        Wind = 8,
        Lightining = 16
    }
}
