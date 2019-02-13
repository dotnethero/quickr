namespace Quickr.ViewModels.Database
{
    internal class KeyViewModel : EntryViewModel
    {
        public string FullName { get; }

        public KeyViewModel(string name, string fullname): base(name)
        {
            FullName = fullname;
        }

        public override string ToString()
        {
            return $"Key: {FullName}";
        }
    }
}