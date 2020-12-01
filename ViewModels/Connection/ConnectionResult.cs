namespace Quickr.ViewModels.Connection
{
    class ConnectionResult
    {
        public bool IsSuccess { get; }
        public string Message { get; }

        public ConnectionResult(bool success, string message)
        {
            IsSuccess = success;
            Message = message;
        }
    }
}