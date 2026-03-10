using System;
using UnityEngine;

namespace br.com.bonus630.thefrog.Shared
{
    public interface IHitReaction 
    {
         void OnHit(Collision2D collision);
    }
}
