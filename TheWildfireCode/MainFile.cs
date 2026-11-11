using System.Reflection;
using BaseLib.Config;
using BaseLib.Hooks;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;
using TheWildfire.TheWildfireCode.Character;
using TheWildfire.TheWildfireCode.Firepower;
using TheWildfire.TheWildfireCode.Patches.EpochPatches;

namespace TheWildfire.TheWildfireCode;

[ModInitializer(nameof(Initialize))]
public partial class MainFile : Node
{
    public const string ModId = "TheWildfire"; //Used for resource filepath
    public const string ResPath = $"res://{ModId}";

    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } =
        new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);

    public static void Initialize()
    {
        EpochRegistration.RegisterEpochs();
        ModConfigRegistry.Register(ModId, new TheWildfireConfig());
        
        Harmony harmony = new(ModId);

        harmony.PatchAllUncategorized();
        
        var assembly = Assembly.GetExecutingAssembly();
        Godot.Bridge.ScriptManagerBridge.LookupScriptsInAssembly(assembly);

        HealthBarForecastRegistry.Register<FirepowerHealthForecast>(ModId);
    }
}