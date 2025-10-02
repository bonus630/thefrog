namespace br.com.bonus630.thefrog.Effects
{
    public interface IEffects 
    {
        void Activate();
        void Deactivate();  
        void UpdateEffects(float deltaTime);
        bool IsFinished { get; }
    }
}
