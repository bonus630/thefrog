using UnityEngine;

namespace br.com.bonus630.thefrog.Shared
{
    public static class GetInterfaceExtension
    {
        public static T GetInterface<T>(this GameObject gameObject) where T : class
        {
            Component[] components = gameObject.GetComponents<MonoBehaviour>();

            foreach (Component component in components)
            {
                if(component is T t)
                    return t;
            }
            return null;
        }
    }
}
