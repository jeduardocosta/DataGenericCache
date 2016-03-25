using System;

namespace DataGenericCache
{
    internal static class Clock
    {
        public static Func<DateTime> Now = () => DateTime.Now;

        public static void ResetClock()
        {
            Now = () => DateTime.Now;
        }
    }
}