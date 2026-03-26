namespace br.com.bonus630.thefrog.Shared
{
    public interface IPlayerManager
    {
        void Registre(IGameEvents gameEvents);
        int NumDies { get; }
        //void SetSates<T>(T states) where T : class;
    }
}