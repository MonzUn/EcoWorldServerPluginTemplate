using Eco.EM.Framework.FileManager;

namespace Eco.Plugins.EcoWorldServerPluginTemplate
{
    public class Storage
    {
        public static readonly Storage Instance = new Storage();
        public static PersistantStorageData PersistentData { get; private set; } = new PersistantStorageData();
        public static WorldStorageData WorldData { get; private set; } = new WorldStorageData();

        static Storage() { }

        private Storage() { }

        public void Initialize()
        {
            Read();
        }

        public void Shutdown()
        {
            Write();
        }

        public void ResetWorldData()
        {
            WorldData = new WorldStorageData();
            Write(); // Make sure we don't read old data in case of an ungraceful shutdown
        }

        public void Write()
        {
            FileManager<PersistantStorageData>.WriteTypeHandledToFile(PersistentData, EWPlugin.Obj.ConfigData.StorageDirectory, Constants.STORAGE_PERSISTANT_FILE_NAME);
            FileManager<WorldStorageData>.WriteTypeHandledToFile(WorldData, EWPlugin.Obj.ConfigData.StorageDirectory, Constants.STORAGE_WORLD_FILE_NAME);
        }

        public void Read()
        {
            PersistentData = FileManager<PersistantStorageData>.ReadTypeHandledFromFile(EWPlugin.Obj.ConfigData.StorageDirectory, Constants.STORAGE_PERSISTANT_FILE_NAME);
            WorldData = FileManager<WorldStorageData>.ReadTypeHandledFromFile(EWPlugin.Obj.ConfigData.StorageDirectory, Constants.STORAGE_WORLD_FILE_NAME);
        }
    }

    public class WorldStorageData
    {
        // Here you can add variables that should be parsed and stored between server restarts, but removed when the world is reset.
        // You can tag data that should not be stored using the [JsonIgnore] attribute.
    }

    public class PersistantStorageData
    {
        // Here you can add variables that should be parsed and stored between server restarts, and kept when the world is reset.
        // You can tag data that should not be stored using the [JsonIgnore] attribute.
    }
}
