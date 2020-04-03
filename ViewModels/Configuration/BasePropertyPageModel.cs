using System.Collections.Generic;
using System.Linq;
using Quickr.Models.Configuration;
using Quickr.Services;

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
        
        protected void Save()
        {
            foreach (var (key, value) in _mapping.Where(x => x.Value.IsPropertyChanged))
            {
                _connection.ConfigSet(key, value.Serialize());
            }
        }
    }
}