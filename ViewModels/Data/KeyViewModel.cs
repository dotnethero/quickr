using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Quickr.Utils;

namespace Quickr.ViewModels.Data
{
    class KeyViewModel : BaseViewModel
    {
        public PropertiesViewModel Properties { get; }
        public BaseValueViewModel Value { get; }

        public ICommand SaveCommand { get; }

        public bool IsUnsaved => Properties.IsUnsaved || Value.IsUnsaved;
        public bool IsKeyRemoved => Value.IsKeyRemoved;

        public KeyViewModel(PropertiesViewModel properties, BaseValueViewModel value)
        {
            Properties = properties;
            Properties.PropertyChanged += OnPropertiesPropertyChanged;
            Value = value;
            Value.PropertyChanged += OnValuePropertyChanged;

            SaveCommand = new Command(async () => await Save());
        }

        void OnPropertiesPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PropertiesViewModel.IsUnsaved)) 
                OnPropertyChanged(nameof(IsUnsaved));
        }

        void OnValuePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(BaseValueViewModel.IsUnsaved)) 
                OnPropertyChanged(nameof(IsUnsaved));

            if (e.PropertyName == nameof(BaseValueViewModel.IsKeyRemoved)) 
                OnPropertyChanged(nameof(IsKeyRemoved));
        }

        public async Task<bool> Save() =>
            await Properties.Save().ConfigureAwait(false) &&
            await Value.Save().ConfigureAwait(false);
    }
}