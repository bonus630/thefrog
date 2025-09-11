
using UnityEngine;

namespace br.com.bonus630.thefrog.Shared
{
    public interface IElement 
    {
        Elements GetElement();
        Elements CanActiveBy();
        Elements CanDeactiveBy();

        Color GetElementColor();
        void ActiveBy(Elements element);
        void DeactiveBy(Elements element);

    }
}
