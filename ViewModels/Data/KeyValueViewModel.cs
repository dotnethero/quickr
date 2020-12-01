using System.Threading.Tasks;
using Quickr.Models.Keys;

namespace Quickr.ViewModels.Data
{
    abstract class BaseValueViewModel : BaseViewModel
    {
        public KeyEntry Key { get; }

        public virtual bool IsUnsaved => false;
        public virtual bool IsKeyRemoved => false;

        protected BaseValueViewModel(KeyEntry key) => Key = key;

        public abstract Task<bool> Save();
    }
}