namespace br.com.bonus630.thefrog.Shared
{
    public interface IEnemy
    {
        void Hit(float amount);
        void DestroySelf();

        bool IsDied { get; }

    }
}
