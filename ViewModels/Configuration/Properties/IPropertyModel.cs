namespace Quickr.ViewModels.Configuration
{
    interface IPropertyModel
    {
        bool IsPropertyChanged { get; }
        bool IsSaveFailed { get; set; }
        void ApplyCurrentValue();
        string Serialize();
    }
}