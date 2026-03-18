namespace br.com.bonus630.thefrog.Effects
{
    public abstract class IEffects
    {

        protected IEffects(){ }
     
        public static T Create<T>()  where T : new() =>  new T();
        public T Build<T>() where T :IEffects => this as T;

        public abstract void Activate();
        public abstract void Deactivate();
        public abstract void UpdateEffects(float deltaTime);
        public  bool IsFinished { get; protected set; }
        public  ushort ID { get; set; }
    }
}
