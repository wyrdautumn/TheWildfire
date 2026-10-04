using System.Reflection;
using BaseLib.Hooks;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;
using TheWildfire.TheWildfireCode.Firepower;

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
        Harmony harmony = new(ModId);

        harmony.PatchAll();
        
        var assembly = Assembly.GetExecutingAssembly();
        Godot.Bridge.ScriptManagerBridge.LookupScriptsInAssembly(assembly);

        HealthBarForecastRegistry.Register<FirepowerHealthForecast>(ModId);
    }
}