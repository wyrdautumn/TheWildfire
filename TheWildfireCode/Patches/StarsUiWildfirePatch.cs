using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace TheWildfire.TheWildfireCode.Patches;

[HarmonyPatch(typeof(NStarCounter), nameof(NStarCounter.RefreshVisibility), new Type[] { })]
public class StarsUiEngineerPatch
{
    [HarmonyPostfix]
    public static void Postfix(NStarCounter __instance)
    {
        if (__instance._player?.Character is not Character.TheWildfire)
            return;
        
        if (__instance.HasMeta("WildfirePositionAdjusted"))
            return;

        __instance.Position += new Vector2(0, 6);
        __instance.SetMeta("WildfirePositionAdjusted", true);
    }
}