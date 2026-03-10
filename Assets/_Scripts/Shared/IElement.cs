
using UnityEngine;

namespace br.com.bonus630.thefrog.Shared
{
    public interface IElement 
    {
        bool isActived { get; set; }
        Elements GetElement { get; }

        Elements CanActiveBy();
        Elements CanDeactiveBy();

        Color ElementColor { get; }

        void ActiveBy(Elements element);
        void DeactiveBy(Elements element);

    }
}
