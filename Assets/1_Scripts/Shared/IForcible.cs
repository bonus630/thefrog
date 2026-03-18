using UnityEngine;

namespace br.com.bonus630.thefrog.Shared
{
    public interface IForcible
    {
        public void AddForce(Vector2 force, ForceMode2D mode = ForceMode2D.Impulse, float time = 1f, bool removeInput = true);
    }
}
