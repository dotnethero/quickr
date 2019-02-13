namespace Quickr.Models
{
    internal class KeyEntry : TreeEntry
    {
        public string FullName { get; }

        public KeyEntry(int dbIndex, string name, string fullname): base(dbIndex, name)
        {
            FullName = fullname;
        }

        public override string ToString()
        {
            return $"Key: {FullName}";
        }
    }
}