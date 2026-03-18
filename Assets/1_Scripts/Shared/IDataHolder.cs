using System;

namespace br.com.bonus630.thefrog.Shared
{
    public interface IDataHolder
    {
        Type DataHolderType { get; }
    }
    public class DataHolder<T> : IDataHolder where T : class, new()
    {
        private  T Data;
        public DataHolder(T data)
        {
            Data = data;
            DataHolderType = typeof(T);
        }

        public Type DataHolderType { get; private set; }

        public  T GetData()
        {
            return Data;
        }

        public void SetData(T data)
        {
            Data = data;
        }
    }
}
