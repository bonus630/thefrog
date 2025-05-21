using System.Collections;
using br.com.bonus630.thefrog.Manager;
using Unity.VisualScripting;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    public class ShurykenChestAlert : TipsBase
    {
        IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            if (GameManager.Instance.IsEventCompleted(GameEventName.Shuryken))
                Destroy(gameObject);
        }
        protected override void OnEventCompleted(GameEvent obj)
        {
            if(obj.Name.Equals(GameEventName.Shuryken))
                Destroy(gameObject);
        }
    }
}
