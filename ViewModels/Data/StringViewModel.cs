﻿using System;
using System.Threading.Tasks;
using Quickr.Models.Keys;
using Quickr.Services;
using Quickr.ViewModels.Editors;

namespace Quickr.ViewModels.Data
{
    internal class StringViewModel: BaseKeyViewModel
    {
        public StringViewModel(RedisProxy proxy, KeyEntry key, TimeSpan? ttl): base(proxy, key, ttl)
        {
            SetupAsync();
        }

        private async void SetupAsync()
        {
            var str = await Proxy.GetStringAsync(Key);
            Value = new ValueViewModel(str);
            Value.ValueSaved += OnValueSaved;
        }

        protected void OnValueSaved(object sender, EventArgs e)
        {
            Proxy.SetString(Key, Value.CurrentValue);
            Value.OriginalValue = Value.CurrentValue;
        }
    }
}
