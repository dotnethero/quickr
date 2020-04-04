namespace Quickr.ViewModels.Configuration
{
    internal interface IPropertyModel
    {
        bool IsPropertyChanged { get; }
        bool IsSaveFailed { get; set; }
        void ApplyCurrentValue();
        string Serialize();
    }
}