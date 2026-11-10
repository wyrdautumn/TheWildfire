using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Nodes.Combat;
using TheWildfire.TheWildfireCode.Firepower;
using TheWildfire.TheWildfireCode.Nodes;

namespace TheWildfire.TheWildfireCode.Interop.MintyFixes;

[HarmonyPatchCategory("MintyPatch")]
public class MintyPatches
{
    [HarmonyPatch("MintySpire2.MintySpire2Code.combat.SummedIncomingDamageRender", "CalculateIncomingDamage")]
    [HarmonyPostfix]
    public static void IncomingDamagePostfix(ref int __result, Creature creature)
    {
        if (creature.Player != null)
            __result += FirepowerController.CalculateOverheatDamage(creature.Player);
    }
    
    [HarmonyPatch("MintySpire2.MintySpire2Code.combat.SummedIncomingDamageRender", "RefreshVisibilityAndText")]
    [HarmonyPostfix]
    public static void RefreshVisibilityPostfix(NHealthBar bar)
    {
        WildfireHpTrackers node = bar.GetNodeOrNull<WildfireHpTrackers>("WildfireHPTrackers");
        node.AdjustMintyPreview();
    }
}