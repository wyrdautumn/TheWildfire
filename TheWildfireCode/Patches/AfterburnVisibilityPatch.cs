using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Cards;
using TheWildfire.TheWildfireCode.Nodes;

namespace TheWildfire.TheWildfireCode.Patches;

[HarmonyPatch(typeof(NCard), nameof(NCard.UpdateVisuals))]
public class AfterburnVisibilityPatch
{
    [HarmonyPostfix]
    public static void FixSorceryStampNode(NCard? __instance)
    {
        if (__instance == null || __instance.GetChild(0) == null)
            return;
        AfterburnCounter? counter = __instance.GetChild(0).GetNodeOrNull<AfterburnCounter>("AfterburnCounter");
        if (counter == null)
            return;
        counter.UpdateVisibility();
    }
}