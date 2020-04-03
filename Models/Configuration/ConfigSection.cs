namespace Quickr.Models.Configuration
{
    internal class ConfigSection
    {
        public string Section { get; set; }
        public ConfigKey[] Configs { get; set; }

        public override string ToString() => $"{Section} ({Configs.Length})";
    }
}
