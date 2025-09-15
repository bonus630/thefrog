
using UnityEngine;

namespace br.com.bonus630.thefrog.Shared
{
    public interface IElement 
    {
        Elements GetElement { get; }

        Elements CanActiveBy();
        Elements CanDeactiveBy();

        Color ElementColor { get; }

        void ActiveBy(Elements element);
        void DeactiveBy(Elements element);

    }
}
