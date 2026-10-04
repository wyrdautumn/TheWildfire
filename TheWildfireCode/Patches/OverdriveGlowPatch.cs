using BaseLib.Extensions;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
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
            if (card == null || !card.CanPlay() || card.ShouldGlowRed || card.ShouldGlowGold)
                return;
            if (card.Owner.HasPower<OverdrivePower>() && card.Tags.Contains(WildfireKeywords.ExertTag))
            {
                __instance.CardNode.CardHighlight.Modulate = new Color(1f, 0.608f, 1f, 0.98f);
                return;
            }
            if (card.Keywords.Contains(WildfireKeywords.AttackFlow) && CheckFlow(card, CardType.Attack))
            {
                __instance.CardNode.CardHighlight.Modulate = new Color(0.99f, 0.56f, 0.56f, 0.98f);
                return;
            }
            if (card.Keywords.Contains(WildfireKeywords.SkillFlow) && CheckFlow(card, CardType.Skill))
            {
                __instance.CardNode.CardHighlight.Modulate = new Color(0.071f, 0.686f, 0.102f, 0.98f);
                return;
            }
            if (card.Keywords.Contains(WildfireKeywords.PowerFlow) && CheckFlow(card, CardType.Power))
            {
                __instance.CardNode.CardHighlight.Modulate = new Color(0.178f, 0.285f, 0.865f, 0.98f);
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
            if (card.Owner.HasPower<OverdrivePower>() && card.Tags.Contains(WildfireKeywords.ExertTag))
            {
                flash.Modulate = new Color(1f, 0.608f, 1f, 0.98f);
                return;
            }
            if (card.Keywords.Contains(WildfireKeywords.AttackFlow) && CheckFlow(card, CardType.Attack))
            {
                flash.Modulate = new Color(0.99f, 0.56f, 0.56f, 0.98f);
                return;
            }
            if (card.Keywords.Contains(WildfireKeywords.SkillFlow) && CheckFlow(card, CardType.Skill))
            {
                flash.Modulate = new Color(0.071f, 0.686f, 0.102f, 0.98f);
                return;
            }
            if (card.Keywords.Contains(WildfireKeywords.PowerFlow) && CheckFlow(card, CardType.Power))
            {
                flash.Modulate = new Color(0.118f, 0.235f, 0.865f, 0.98f);
            }
        }
    }

    public static bool CheckFlow(CardModel card, CardType type)
    {
        if (CombatManager.Instance == null || card.CombatState == null || !card.IsInCombat)
            return false;
        if (type == CardType.Attack && card.Owner.HasPower<AttackFlowPower>())
            return false;
        if (type == CardType.Skill && card.Owner.HasPower<SkillFlowPower>())
            return false;
        if (type == CardType.Power && card.Owner.HasPower<PowerFlowPower>())
            return false;
        return CombatManager.Instance.History
            .CardPlaysStarted
            .LastOrDefault(e =>
                e.CardPlay.Card.Owner == card.Owner && e.HappenedThisTurn(card.CombatState))?.CardPlay.Card.Type == type;
    }
}