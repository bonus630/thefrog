using System.Collections.Generic;

namespace br.com.bonus630.thefrog.Shared
{
    public class ListStorage<T>
    {
        private string name;
        private float extraData;
        private List<T> values;

        public string Name { get => name; set => name = value; }
        public float ExtraData { get => extraData; set => extraData = value; }
        public List<T> Values { get => values; set => values = value; }
    }
}
