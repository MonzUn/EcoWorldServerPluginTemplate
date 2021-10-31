using System.ComponentModel;
using System.IO;

namespace Eco.Plugins.EcoWorldServerPluginTemplate
{
    public class EWPluginConfig
    {
        public static class DefaultValues
        {
            public static Logger.LogLevel LogLevel = Logger.LogLevel.Information;
            public static readonly string StorageDirectory = "Storage\\Mods\\EWPlugin";
        }

        [Description("Determines whether or not the plugin should be enabled")]
        public bool Enabled { get; set; } = true;

        [Description("Determines what message types will be printed to the server log. All message types below the selected one will be printed as well.")]
        public Logger.LogLevel LogLevel { get; set; } = DefaultValues.LogLevel;

        [Description("Determines the directory used for all file storage. The path is relative to the server root directory. Changes to this setting requires a server restart to take effect.")]
        public string StorageDirectory { get; set; } = DefaultValues.StorageDirectory;

        [Browsable(false)]
        public string StorageDirectoryAbs { get { return $"{Directory.GetCurrentDirectory()}\\{StorageDirectory}"; } }

        public void OnConfigChanged(string param)
        {
            if (param == nameof(LogLevel))
            {
                // Here you can react to config changes where one parameter change should affect another parameter as well.
                // You can also perform validation here to warn the user if the config is wrong after their change.
            }
        }
    }
}