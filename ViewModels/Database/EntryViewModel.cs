namespace Quickr.ViewModels.Database
{
    internal abstract class EntryViewModel
    {
        public string Name { get; set; }

        protected EntryViewModel(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}