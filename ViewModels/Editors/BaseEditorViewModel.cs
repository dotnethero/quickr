using System;
using System.Windows.Input;
using Quickr.Utils;

namespace Quickr.ViewModels.Editors
{
    internal abstract class BaseEditorViewModel: BaseViewModel
    {
        public event EventHandler ValueSaved;
        public event EventHandler ValueDiscarded;
        
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        protected BaseEditorViewModel()
        {
            SaveCommand = new Command(Save);
            CancelCommand = new Command(Cancel);
        }
        
        protected virtual void Save()
        {
            ValueSaved?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void Cancel()
        {
            ValueDiscarded?.Invoke(this, EventArgs.Empty);
        }
    }
}