using UnityEngine;

namespace br.com.bonus630.thefrog.Shared
{
    public class Getter : MonoBehaviour
    {
        [SerializeField] int ComponentsCount;
        public T GetInterface<T>() where T : class
        {
            Component[] components = GetComponents<Component>();
            foreach (Component comp in components)
            {
                if (comp is T t)
                    return t;
            }
            return null;
        }
        private void OnValidate()
        {
            ComponentsCount = gameObject.GetComponentCount();
        }
        //public void Update()
        //{
        //    if (Input.GetKeyUp(KeyCode.W))
        //    {
        //        var c = gameObject.GetComponent<IBarUI>();
        //        Debug.Log("c : " + c);
        //        c.GoToValue(50, 0.5f);
        //    }
        //}

    }
}
