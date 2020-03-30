namespace Quickr.Models
{
    internal enum KeyType
    {
        String,
        List,
        Set,
        SortedSet,
        HashSet,
    }

    internal class CreateKeyModel
    {
        public string Name { get; set; }
        public KeyType Type { get; set; }
    }
}