using System;
using System.Reflection;

namespace Quickr.Utils
{
    internal static class Finders
    {
        public static ConstructorInfo[] Internal(Type t)
        {
            return t.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
        }
    }
}