using System;
using System.Threading.Tasks;
using Quickr.Models.Keys;
using Quickr.ViewModels.Editors;

namespace Quickr.ViewModels.Data
{
    internal abstract class BaseKeyViewModel : BaseViewModel
    {
        public KeyEntry Key { get; }
        public PropertiesViewModel Properties { get; }

        public virtual bool IsUnsaved => false;
        public virtual bool IsKeyRemoved => false;

        protected BaseKeyViewModel(KeyEntry key, TimeSpan? ttl)
        {
            Key = key;
            Properties = CreatePropertiesViewModel(ttl);
        }

        public abstract Task Save();

        private PropertiesViewModel CreatePropertiesViewModel(TimeSpan? ttl)
        {
            var name = Key.FullName;
            var props = new PropertiesViewModel(name, ttl);
            props.ValueSaved += async (sender, args) => await OnPropertiesSaved();
            props.ValueDiscarded += (sender, args) => OnPropertiesDiscarded();
            return props;
        }

        private async Task OnPropertiesSaved()
        {
            if (Properties.Expiration < TimeSpan.Zero)
            {
                Properties.Expiration = null;
            }
            if (Properties.Expiration != Properties.OriginalExpiration)
            {
                await Key.SetTimeToLive(Properties.Expiration);
                Properties.OriginalExpiration = Properties.Expiration;
            }
            if (Properties.Name != Properties.OriginalName)
            {
                await Key.Rename(Properties.Name);
                Properties.OriginalName = Properties.Name;
            }
        }
        
        private void OnPropertiesDiscarded()
        {
            Properties.Name = Properties.OriginalName;
            Properties.Expiration = Properties.OriginalExpiration;
        }
    }
}