using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Rooms;
using TheWildfire.TheWildfireCode.Firepower;

namespace TheWildfire.TheWildfireCode.Patches;

[HarmonyPatch (typeof(CombatManager), nameof(CombatManager.Reset))]
public class ResetNodeListPatch
{
    [HarmonyPrefix]
    public static void NodeListPrefix()
    {
        FirepowerController.FirepowerNodes.Clear();
    }
}

[HarmonyPatch (typeof(CombatRoom), nameof(CombatRoom.StartCombat))]
public class SetupNodeListPatch
{
    [HarmonyPrefix]
    public static void SetupNodeListPrefix()
    {
        FirepowerController.FirepowerNodes.Clear();
    }
}

[HarmonyPatch (typeof(CombatRoom), nameof(CombatRoom.StartPreFinishedCombat))]
public class SetupNodeListSecondPatch
{
    [HarmonyPrefix]
    public static void SetupNodeListSecondPrefix()
    {
        FirepowerController.FirepowerNodes.Clear();
    }
}