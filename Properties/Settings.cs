using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Quickr.Models;

namespace Quickr.Properties
{
    internal class Settings
    {
        private static Settings _settings;

        public const string FileName = "App.settings";

        public List<EndPointModel> Endpoints { get; set; } = new List<EndPointModel>();

        public void Save() => File.WriteAllText(FileName, JsonConvert.SerializeObject(this, Formatting.Indented));

        public static Settings Current => _settings ??= Load();

        private static Settings Load() =>
            !File.Exists(FileName)
                ? new Settings()
                : JsonConvert.DeserializeObject<Settings>(File.ReadAllText(FileName));
    }
}
