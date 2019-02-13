namespace Quickr.Models
{
    internal abstract class TreeEntry
    {
        public string Name { get; }
        public int DbIndex { get; }

        protected TreeEntry(int dbIndex, string name)
        {
            DbIndex = dbIndex;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}