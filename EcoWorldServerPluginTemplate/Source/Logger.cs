using Eco.Core.Utils.Logging;
using static Eco.EM.Framework.Utils.ConsoleColors;

namespace Eco.Plugins.EcoWorldServerPluginTemplate
{
    public static class Logger
    {
        public enum LogLevel
        {
            DebugVerbose,
            Debug,
            Warning,
            Information,
            Error,
            Silent,
        }

        private static EWPlugin Plugin = null;
        private static readonly NLogWriter PluginLog = NLogManager.GetLogWriter("EwPlugin");
        private static readonly System.ConsoleColor ConsoleColor = System.ConsoleColor.Green;
        private static readonly string Tag = "[EwPlugin]";

        public static void Initialize(EWPlugin plugin)
        {
            Plugin = plugin;
        }

        public static void DebugVerbose(string message)
        {
            if (Plugin.ConfigData.LogLevel <= LogLevel.DebugVerbose)
                PluginLog.Write($"DEBUG: {message}"); // Verbose debug log messages are only written to the log file if enabled via configuration
        }

        public static void Debug(string message)
        {
            if (Plugin.ConfigData.LogLevel <= LogLevel.Debug)
                PrintConsoleMultiColored(Tag, ConsoleColor, $" {message}", System.ConsoleColor.Gray);
            PluginLog.Write($"DEBUG: {message}");
        }

        public static void Warning(string message)
        {
            if (Plugin.ConfigData.LogLevel <= LogLevel.Warning)
                PrintConsoleMultiColored(Tag, ConsoleColor, $" {message}", System.ConsoleColor.Yellow);
            PluginLog.WriteWarning(message);
        }

        public static void Info(string message, System.ConsoleColor textColor = System.ConsoleColor.White)
        {
            if (Plugin.ConfigData.LogLevel <= LogLevel.Information)
                PrintConsoleMultiColored(Tag, ConsoleColor, $" {message}", textColor);
            PluginLog.Write(message);
        }

        public static void Error(string message)
        {
            if (Plugin.ConfigData.LogLevel <= LogLevel.Error)
                PrintConsoleMultiColored(Tag, ConsoleColor, $" {message}", System.ConsoleColor.Red);
            PluginLog.WriteError(message);
        }
    }
}
