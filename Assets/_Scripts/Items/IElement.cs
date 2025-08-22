using UnityEngine;
using br.com.bonus630.thefrog.Shared;

namespace br.com.bonus630.thefrog.Items
{
    public interface IElement 
    {
        Elements GetElement();
        Elements CanActiveBy();
        Elements CanDeactiveBy();

        void ActiveBy(Elements element);
        void DeactiveBy(Elements element);

    }
}
