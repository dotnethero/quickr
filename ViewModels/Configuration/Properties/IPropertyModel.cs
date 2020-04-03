namespace Quickr.ViewModels.Configuration
{
    internal interface IPropertyModel
    {
        bool IsPropertyChanged { get; }
        string Serialize();
    }
}