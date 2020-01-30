namespace Quickr.Models.Keys
{
    internal class KeyEntry : TreeEntry
    {
        public string FullName { get; }

        public KeyEntry(int dbIndex, string name, string fullname, FolderEntry parent): base(dbIndex, name, parent)
        {
            FullName = fullname;
        }

        public override string ToString()
        {
            return $"Key: {FullName}";
        }
    }
}