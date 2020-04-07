using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Quickr.Models.Configuration;
using Quickr.Services;
using StackExchange.Redis;

namespace Quickr.ViewModels.Configuration
{
    internal abstract class BasePropertyPageModel
    {
        private readonly RedisConnection _connection;
        private readonly Dictionary<string, ConfigKeyValue> _config;
        private readonly Dictionary<string, IPropertyModel> _mapping;

        protected BasePropertyPageModel(RedisConnection connection, Dictionary<string, ConfigKeyValue> config)
        {
            _connection = connection;
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
            foreach (var (key, value) in _mapping.Where(x => x.Value.IsPropertyChanged))
            {
                try
                {
                    _connection.ConfigSet(key, value.Serialize());
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