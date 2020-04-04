namespace Quickr.Models.Configuration
{
    internal class ConfigKey
    {
        public string Key { get; set; }
        public string Description { get; set; }
        public string DefaultValue { get; set; }
        public string[] PossibleValues { get; set; }
        public bool IsReadOnly { get; set; }

        public override string ToString() => Key;
    }
}