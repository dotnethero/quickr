﻿namespace Quickr.Models.Keys
{
    internal class DatabaseEntry: FolderEntry
    {
        public DatabaseEntry(int dbIndex): base(dbIndex, $"db{dbIndex}", $"db{dbIndex}", null)
        {
        }
    }
}