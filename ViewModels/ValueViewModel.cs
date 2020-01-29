using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Quickr.Utils;
using StackExchange.Redis;

namespace Quickr.ViewModels
{
    internal class ValueViewModel : INotifyPropertyChanged
    {
        private string _currentValue;

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public string OriginalValue { get; }

        public string CurrentValue
        {
            get => _currentValue;
            set
            {
                _currentValue = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ValueChanged));
            }
        }

        public bool ValueChanged => CurrentValue != OriginalValue;

        public ValueViewModel(RedisValue originalValue)
        {
            OriginalValue = originalValue;
            CurrentValue = originalValue;
            SaveCommand = new Command(Save);
            CancelCommand = new Command(Cancel);
        }

        private void Save()
        {
            OnValueSaved?.Invoke(this, EventArgs.Empty);
        }

        private void Cancel()
        {
            CurrentValue = OriginalValue;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event ValueSavedEventHandler OnValueSaved;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public delegate void ValueSavedEventHandler(object sender, EventArgs e);
}
