using BaseLib.Extensions;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Powers;

namespace TheWildfire.TheWildfireCode.Patches;

[HarmonyPatch]
public class OverdriveGlowPatch
{
    [HarmonyPatch(typeof(NHandCardHolder), nameof(NHandCardHolder.UpdateCard))]
    public class OverdrivePowerGlowPatch
    {
        [HarmonyPostfix]
        private static void Postfix(NHandCardHolder __instance)
        {
            if (__instance.CardNode == null)
                return;
            CardModel? card = __instance.CardNode.Model;
            if (card == null || !card.CanPlay() || card.ShouldGlowRed)
                return;
            if (card.Owner.HasPower<OverdrivePower>() && card.Tags.Contains(WildfireKeywords.FlareTag))
            {
                __instance.CardNode.CardHighlight.Modulate = new Color(1f, 0.608f, 1f, 0.98f);
            }
        }
    }
    
    [HarmonyPatch(typeof(NHandCardHolder), nameof(NHandCardHolder.Flash))]
    internal static class WildfireNHandCardHolderFlashHandOutlinePatch
    {
        [HarmonyPostfix]
        public static void Postfix(NHandCardHolder __instance)
        {
            if (__instance.CardNode == null)
                return;
            CardModel? card = __instance.CardNode.Model;
            if (AccessTools.Field(typeof(NHandCardHolder), "_flash")?.GetValue(__instance) is not Control flash ||
                !GodotObject.IsInstanceValid(flash) || card == null || !card.CanPlay() || card.ShouldGlowGold || card.ShouldGlowRed)
                return;
            if (card.Owner.HasPower<OverdrivePower>() && card.Tags.Contains(WildfireKeywords.FlareTag))
            {
                flash.Modulate = new Color(1f, 0.608f, 1f, 0.98f);
            }
        }
    }
}