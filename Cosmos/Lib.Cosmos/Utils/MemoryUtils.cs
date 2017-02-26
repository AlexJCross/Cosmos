namespace Lib.Cosmos.Utils
{
    using System;

    public class MemoryUtils
    {
        public static void LogGc<T>()
        {
            var name = typeof(T).Name;
            Console.WriteLine($"GC: {name}");
        }

        public static void LogCtor<T>()
        {
            var name = typeof(T).Name;
            Console.WriteLine($"CTOR: {name}");
        }
    }
}
