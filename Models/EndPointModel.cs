namespace Quickr.Models
{
    internal class EndPointModel
    {
        public string Server { get; set; }
        public int Port { get; set; }

        public EndPointModel(string server, int port)
        {
            Server = server;
            Port = port;
        }
    }
}