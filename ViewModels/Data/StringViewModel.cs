using System;
using Quickr.Models.Keys;
using Quickr.Services;

namespace Quickr.ViewModels.Data
{
    internal class StringViewModel: BaseKeyViewModel
    {
        public StringViewModel(RedisProxy proxy, KeyEntry key): base(proxy, key)
        {
            Value = new ValueViewModel(Proxy.GetString(Key));
        }

        protected void OnValueSaved(object sender, EventArgs e)
        {
            Proxy.SetString(Key, Value.CurrentValue);
        }
    }
}
