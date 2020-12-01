using System.ComponentModel;
using System.Threading.Tasks;

namespace Quickr.ViewModels.Data
{
    class KeyViewModel : BaseViewModel
    {
        public PropertiesViewModel Properties { get; }
        public BaseValueViewModel Value { get; }

        public bool IsUnsaved => Properties.IsUnsaved || Value.IsUnsaved;
        public bool IsKeyRemoved => Value.IsKeyRemoved;

        public KeyViewModel(PropertiesViewModel properties, BaseValueViewModel value)
        {
            Properties = properties;
            Properties.PropertyChanged += OnPropertiesPropertyChanged;
            Value = value;
            Value.PropertyChanged += OnValuePropertyChanged;
        }

        void OnPropertiesPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == nameof(PropertiesViewModel.IsUnsaved)) OnPropertyChanged(nameof(IsUnsaved));
        }

        void OnValuePropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == nameof(BaseValueViewModel.IsUnsaved)) OnPropertyChanged(nameof(IsUnsaved));
            if (args.PropertyName == nameof(BaseValueViewModel.IsKeyRemoved)) OnPropertyChanged(nameof(IsKeyRemoved));
        }

        public async Task<bool> Save() =>
            await Properties.Save().ConfigureAwait(false) &&
            await Value.Save().ConfigureAwait(false);
    }
}