using System;

namespace EnergyTrading.Logging
{
    [Flags]
    public enum LogFilterLevel
    {
        None = 0,
        Debug = 1,
        Info = 2,
        Warn = 4,
        Error = 8,
        Fatal = 16,
        BelowInfo = Debug,
        BelowWarn = Debug | Info,
        BelowError = Debug | Info | Warn,
        BelowFatal = Debug | Info | Warn | Error,
        All = Debug | Info | Warn | Error | Fatal,
    }
}