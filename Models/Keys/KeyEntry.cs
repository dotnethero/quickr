namespace Quickr.Models.Keys
{
    internal class KeyEntry : TreeEntry
    {
        private string _fullName;

        public string FullName
        {
            get => _fullName;
            set
            {
                _fullName = value;
                OnPropertyChanged();
            }
        }

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