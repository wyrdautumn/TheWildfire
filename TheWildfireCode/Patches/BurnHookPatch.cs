using HarmonyLib;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Cards;
using TheWildfire.TheWildfireCode.Firepower;

namespace TheWildfire.TheWildfireCode.Patches;

[HarmonyPatch]
public class BurnHookPatch
{
    [HarmonyPatch(typeof(Burn), "OnTurnEndInHand")]
    [HarmonyPostfix]
    public static void AddHookToBurnPatch(Burn __instance, PlayerChoiceContext choiceContext)
    {
        if (__instance.Owner.Creature.CombatState != null)
        {
            TaskHelper.RunSafely(
                FirepowerHooks.AfterBurnStatusTrigger(__instance.Owner.Creature.CombatState, choiceContext, __instance.Owner));
        }
    }
}