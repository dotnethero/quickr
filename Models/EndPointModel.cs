﻿namespace Quickr.Models
{
    public class EndPointModel
    {
        public string Name { get; set; }
        public string Server { get; set; }
        public int? Port { get; set; }
        public string Password { get; set; }
        public bool UseSsl { get; set; }
        public bool IsNew { get; set; } = true;
    }
}