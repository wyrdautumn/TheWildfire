using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

namespace TheWildfire.TheWildfireCode.Patches;

[HarmonyPatch]
public class WildfireCardPatches
{
    [HarmonyPatch(typeof(CardModel), nameof(CardModel.PortraitPath), MethodType.Getter)]
    [HarmonyPostfix]
    public static void WildfireCardPortraitPostfix(CardModel __instance, ref string __result)
    {
        if (__instance is Joy && __instance.IsInCombat && __instance.Owner.Character is Character.TheWildfire)
        {
            __result = "res://TheWildfire/images/card_portraits/wildfire_joy.png";
        }
    }
    
    [HarmonyPatch(typeof(CardModel), "PortraitPngPath", MethodType.Getter)]
    [HarmonyPostfix]
    public static void SovereignBladeSorceressBigPortraitPostfix(CardModel __instance, ref string __result)
    {
        if (__instance is Joy && __instance.IsInCombat && __instance.Owner.Character is Character.TheWildfire)
        {
            __result = "res://TheWildfire/images/card_portraits/big/wildfire_joy.png";
        }
    }
}