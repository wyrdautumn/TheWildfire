using HarmonyLib;
using MegaCrit.Sts2.Core.Helpers;

namespace TheWildfire.TheWildfireCode.Patches;

[HarmonyPatch(typeof(OneTimeInitialization), "ExecuteEssential")]
public class InitializationPatch
{
    [HarmonyPostfix]
    internal static void InitializationPostfix()
    {
        var mintyType = AccessTools.TypeByName("MintySpire2.MintySpire2Code.combat.SummedIncomingDamageRender");
        if (mintyType == null)
            return;
        
        Harmony harmony = new("TheWildfire");

        harmony.PatchCategory("MintyPatch");
    }
}