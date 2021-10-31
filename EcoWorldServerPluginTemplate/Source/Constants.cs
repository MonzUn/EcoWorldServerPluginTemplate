using System;
using System.IO;

namespace Eco.Plugins.EcoWorldServerPluginTemplate
{
    public static class Constants
    {
        public const string CONFIG_PATH_RELATIVE            = "Configs\\Mods\\EWPlugin";
        public static readonly string CONFIG_PATH_ABSOLUTE  = $"{Directory.GetCurrentDirectory()}\\{CONFIG_PATH_RELATIVE}";
        public const string STORAGE_PERSISTANT_FILE_NAME    = "PersistantStorage";
        public const string STORAGE_WORLD_FILE_NAME         = "WorldStorage";

        public static readonly DateTime INVALID_TIME = DateTime.MinValue;
    }
}
