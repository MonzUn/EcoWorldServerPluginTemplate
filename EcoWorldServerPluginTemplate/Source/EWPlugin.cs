#define UseWorldGenerationCallback // Set this if you need to react to the world generation event

using Eco.Core;
using Eco.Core.Plugins;
using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils;
using Eco.EM.Framework.VersioningTools;
using Eco.Gameplay.GameActions;
using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

#if UseWorldGenerationCallback
using Eco.WorldGenerator;
#endif

namespace Eco.Plugins.EcoWorldServerPluginTemplate
{
#if UseWorldGenerationCallback
    [Priority(PriorityAttribute.High)]    // If you want to react to world generation events, the plugin needs to have to have high priority.
                                          // Note that this limits other features from being used during initialization.
#else
    [Priority(PriorityAttribute.Normal)]
#endif
    public class EWPlugin :
        IModKitPlugin,          // Required in order for the Eco server to load the plugin.
        IInitializablePlugin,   // Allows you to run code when the plugin is initialized via the Initialize function.
        IShutdownablePlugin,    // Allows you to run code when the plugin shuts down via the ShutdownAsync function.
        IConfigurablePlugin,    // Adds a config file and the config tab in the server GUI.
        IDisplayablePlugin,     // Adds a display tab to the server GUI.
        IGameActionAware        // Allows the plugin to react to game actions.
    {
        public readonly Version PluginVersion = Assembly.GetExecutingAssembly().GetName().Version;
        const string PluginName = "EWPlugin";

        public static EWPlugin Obj { get { return PluginManager.GetPlugin<EWPlugin>(); } }
        public EWPluginConfig ConfigData => config.Config;
        public IPluginConfig PluginConfig => config;
        public ThreadSafeAction<object, string> ParamChanged { get; set; }

        public override string ToString() => PluginName;
        public string GetStatus() => StatusDescription;
        public string GetDisplayText() => BuildDisplayString();

        private readonly PluginConfig<EWPluginConfig> config = new PluginConfig<EWPluginConfig>(PluginName);

        // These are used for EM versioning.
        private const string ModIOAppID = "";           // This is the Mod ID on https://eco.mod.io/. It can be found on the main page for your mod.
        private const string ModIODeveloperToken = "";  // This is the ID of your user on https://eco.mod.io/. It can be found on your profile page --> API Access.
                                                        // NOTE: Keep this token secret! Only add it to release builds and avoid storing it or uploading it to version control.

        public StatusState Status
        {
            get { return status; }
            private set
            {
                Logger.Debug($"Plugin status changed from \"{StatusDescription}\" to \"{Utilities.GetEnumDescription(value)}\"");
                status = value;
                StatusDescription = Utilities.GetEnumDescription(value);
            }
        }
        private StatusState status = StatusState.Uninitialized;
        private string StatusDescription = Utilities.GetEnumDescription(StatusState.Uninitialized);
        public enum StatusState
        {
            [Description("Uninitialized")]
            Uninitialized,

            [Description("Initializing")]
            Initializing,

            [Description("Awaiting Server Post Init")]
            AwaitingPostServerInit,

            [Description("Initialization Failed")]
            InitFailed,

            [Description("Shutting Down")]
            ShuttingDown,

            [Description("Starting")]
            Starting,

            [Description("Running")]
            Running,

            [Description("Stopping")]
            Stopping,

            [Description("Stopped")]
            Stopped,
        }

        private string BuildDisplayString()
        {
            StringBuilder displayBuilder = new StringBuilder();

            displayBuilder.AppendLine($"Status: {StatusDescription}");

            // Here you can append more lines with custom information you want to show in the server GUI display tab.

            return displayBuilder.ToString();
        }

        public Result ShouldOverrideAuth(GameAction action)
        {
            return new Result(ResultType.None);
        }

        public object GetEditObject()
        {
            return config.Config;
        }

        public void OnEditObjectChanged(object o, string param)
        {
            // Here you can react to config parameter changes that come in via the server GUI

            if (param == nameof(EWPluginConfig.Enabled))
            {
                if (ConfigData.Enabled)
                    Start();
                else
                    Stop();
            }

            ConfigData.OnConfigChanged(param);
        }

        public void Initialize(TimedTask timer)
        {
            Logger.Initialize(this);

            Status = StatusState.Initializing;

            if (!Validate())
            {
                Logger.Error("Initialization aborted due to validation error(s)");
                return;
            }

#if UseWorldGenerationCallback
            WorldGeneratorPlugin.OnFinishGenerate.AddUnique(HandleWorldReset);
#endif
            PluginManager.Controller.RunIfOrWhenInited(PostServerInitialize); // Defer some initialization for when the server initialization is completed.


            if (!string.IsNullOrWhiteSpace(ModIOAppID) && !string.IsNullOrWhiteSpace(ModIODeveloperToken)) // Only check for mod versioning if the data required for it exists.
                ModVersioning.GetModInit(ModIOAppID, ModIODeveloperToken, "PluginAuthorName", PluginName, ConsoleColor.Green, PluginName);
            else
                Logger.Info($"Plugin version is {PluginVersion}");

            Storage.Instance.Initialize();

#if !UseWorldGenerationCallback
            Start();
#else
            status = StatusState.AwaitingPostServerInit;
#endif
        }

        private void PostServerInitialize()
        {
#if UseWorldGenerationCallback
            Start();
#endif

            // Here you can execute code that requires the server to have finished initialization.
            // If UseWorldGenerationCallback is set, most initialization that requires reading the server state should be done here.
        }

        private bool Validate()
        {
            // Check conditions that need to be correct before starting the plugin here.
            return true;
        }

        public Task ShutdownAsync()
        {
            Stop();
            Status = StatusState.ShuttingDown;
            Storage.Instance.Shutdown();
            return Task.CompletedTask;
        }

        private void Start()
        {
            Status = StatusState.Starting;

            // Ensure config path exists
            if (!Directory.Exists(ConfigData.StorageDirectoryAbs))
                Directory.CreateDirectory(ConfigData.StorageDirectoryAbs);

            // Add init code that should be executed when the plugin is restarted here.

            Status = StatusState.Running;
        }

        private void Stop()
        {
            Status = StatusState.Stopping;

            // Add shutdown code that should be executed when the plugin is restarted here.

            Status = StatusState.Stopped;
        }

#if UseWorldGenerationCallback
        private void HandleWorldReset()
        {
            Logger.Info("New world generated - Removing storage data for previous world");
            Storage.Instance.ResetWorldData();
        }
#endif

        public void ActionPerformed(GameAction action)
        {
            switch (action)
            {
                // Example of action handling - Add more cases here to react to more game actions.
                //case DigOrMine digOrMine:
                //    Logger.Info($"{digOrMine.Citizen.Name} mined a {digOrMine.ItemUsed}");
                //    break;
            }
        }
    }
}
