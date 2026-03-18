using System;
namespace br.com.bonus630.thefrog.Manager
{
    [Serializable]
    public class SaveStates
    {
        public int index;
        public string thumb;
        public EnvironmentStates environmentStates;
   
        public SaveStates()
        {

        }

        public SaveStates(int index)
        {
            this.index = index;
        }

        public SaveStates(int index, EnvironmentStates environmentStates) : this(index)
        {
            this.environmentStates = environmentStates;
        }

        public SaveStates(int index, string thumb, EnvironmentStates environmentStates) : this(index)
        {
            this.thumb = thumb;
            this.environmentStates = environmentStates;
        }
    }
}
