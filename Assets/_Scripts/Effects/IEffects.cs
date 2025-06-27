namespace br.com.bonus630.thefrog.Effects
{
    public interface IEffects 
    {
        void UpdateEffects(float deltaTime);
        bool IsFinished { get; }
    }
}
