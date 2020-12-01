using System;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Quickr.Annotations;
using Quickr.Utils;

namespace Quickr.ViewModels.Data
{
    abstract class BaseEntryViewModel : BaseViewModel
    {
        public event EventHandler ValueSaved;
        public event EventHandler ValueDiscarded;

        string _originalValue;
        string _currentValue;

        public string OriginalValue
        {
            get => _originalValue;
            set
            {
                if (_originalValue == value) return;
                _originalValue = value;
                OnValuePropertyChanged();
                OnPropertyChanged(nameof(IsNew));
            }
        }

        public string CurrentValue
        {
            get => _currentValue;
            set
            {
                if (_currentValue == value) return;
                _currentValue = value;
                OnValuePropertyChanged();
            }
        }

        public virtual bool IsValueChanged => OriginalValue != CurrentValue;
        public virtual bool IsValueSaved => !IsValueChanged;
        public virtual bool IsNew => OriginalValue == null;
        
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        protected BaseEntryViewModel()
        {
            SaveCommand = new Command(Save);
            CancelCommand = new Command(Cancel);
        }

        protected BaseEntryViewModel(string value): this()
        {
            _originalValue = value;
            _currentValue = value;
        }
        
        protected virtual void Save()
        {
            ValueSaved?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void Cancel()
        {
            ValueDiscarded?.Invoke(this, EventArgs.Empty);
        }
        
        [NotifyPropertyChangedInvocator]
        protected virtual void OnValuePropertyChanged([CallerMemberName] string propertyName = null)
        {
            OnPropertyChanged(propertyName);
            OnPropertyChanged(nameof(IsValueSaved));
            OnPropertyChanged(nameof(IsValueChanged));
        }
    }
}