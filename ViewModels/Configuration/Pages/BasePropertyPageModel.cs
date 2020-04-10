using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Quickr.Models.Configuration;
using Quickr.Models.Keys;
using StackExchange.Redis;

namespace Quickr.ViewModels.Configuration
{
    internal abstract class BasePropertyPageModel
    {
        private readonly EndpointEntry _entry;
        private readonly Dictionary<string, ConfigKeyValue> _config;
        private readonly Dictionary<string, IPropertyModel> _mapping;

        protected BasePropertyPageModel(EndpointEntry entry, Dictionary<string, ConfigKeyValue> config)
        {
            _entry = entry;
            _config = config;
            _mapping = new Dictionary<string, IPropertyModel>();
        }
        
        protected bool HasKey(string key)
        {
            return _config.ContainsKey(key);
        }

        protected StringPropertyModel MapToString(string key)
        {
            var model = new StringPropertyModel(_config[key]);
            _mapping.Add(key, model);
            return model;
        }
        
        protected YesNoPropertyModel MapToYesNo(string key)
        {
            var model = new YesNoPropertyModel(_config[key]);
            _mapping.Add(key, model);
            return model;
        }
        
        public void Save()
        {
            var connection = _entry.Connection;
            var endpoint = _entry.Endpoint;
            foreach (var (key, value) in _mapping.Where(x => x.Value.IsPropertyChanged))
            {
                try
                {
                    connection.ConfigSet(endpoint, key, value.Serialize());
                    value.ApplyCurrentValue();
                    if (key == "requirepass")
                    {
                        MessageBox.Show("You need to reconnect to server after requirepass has been changed!", "New password", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                catch (RedisServerException ex)
                {
                    Debug.WriteLine(ex.Message);
                    value.IsSaveFailed = true;
                }
            }
        }
    }
}