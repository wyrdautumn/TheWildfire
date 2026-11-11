using HarmonyLib;
using MegaCrit.Sts2.Core.Timeline;

namespace TheWildfire.TheWildfireCode.Patches.EpochPatches;

[HarmonyPatch]
public class WildfireEpochPortraitPatch
{
    private const string WildfirePrefix = "THEWILDFIRE-";
    private const string EpochImageDir = "res://TheWildfire/images/epochs/";

    [HarmonyPatch(typeof(EpochModel), nameof(EpochModel.ResolvedPortraitPath), MethodType.Getter)]
    [HarmonyPostfix]
    public static void PlaceholderPortraitPathPostfix(EpochModel __instance, ref string __result)
    {
        if (__instance.Id.StartsWith(WildfirePrefix))
        {
            __result = EpochImageDir + __instance.Id.ToLowerInvariant() + ".png";
        }
    }

    [HarmonyPatch(typeof(EpochModel), "PackedPortraitPath", MethodType.Getter)]
    [HarmonyPostfix]
    public static void SorceressPortraitPathPostfix(EpochModel __instance, ref string __result)
    {
        if (__instance.Id.StartsWith(WildfirePrefix))
        {
            __result = EpochImageDir + __instance.Id.ToLowerInvariant() + "_small.png";
        }
    }
}