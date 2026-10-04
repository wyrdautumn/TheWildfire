using BaseLib.Extensions;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Powers;

namespace TheWildfire.TheWildfireCode.Patches;

[HarmonyPatch (typeof(CardModel), "GetDescriptionForPile", typeof(PileType), typeof(CardModel.DescriptionPreviewType), typeof(Creature))]
public class NCardFlowTextPatch
{
    [HarmonyPostfix]
    public static void AddFlowTextPatch(CardModel __instance, ref string __result)
    {
        if (!__instance.Keywords.Contains(WildfireKeywords.AttackFlow) &&
            !__instance.Keywords.Contains(WildfireKeywords.SkillFlow) &&
            !__instance.Keywords.Contains(WildfireKeywords.PowerFlow))
            return;
        string addText = "";
        if (__instance.Keywords.Contains(WildfireKeywords.AttackFlow))
        {
            LocString attackFlow = new LocString("card_keywords", "THEWILDFIRE-ATTACKFLOW.title");
            if (__instance.IsInCombat && __instance.Owner.HasPower<AttackFlowPower>())
                addText += "[color=959492]" + attackFlow.GetFormattedText() + "[/color].\n";
            else
            {
                addText += "[color=D74040]" + attackFlow.GetFormattedText() + "[/color].\n";
            }
        }
        if (__instance.Keywords.Contains(WildfireKeywords.SkillFlow))
        {
            LocString skillFlow = new LocString("card_keywords", "THEWILDFIRE-SKILLFLOW.title");
            if (__instance.IsInCombat && __instance.Owner.HasPower<SkillFlowPower>())
                addText += "[color=959492]" + skillFlow.GetFormattedText() + "[/color].\n";
            else
            {
                addText += "[color=12AF1A]" + skillFlow.GetFormattedText() + "[/color].\n";
            }
        }
        if (__instance.Keywords.Contains(WildfireKeywords.PowerFlow))
        {
            LocString powerFlow = new LocString("card_keywords", "THEWILDFIRE-POWERFLOW.title");
            if (__instance.IsInCombat && __instance.Owner.HasPower<PowerFlowPower>())
                addText += "[color=959492]" + powerFlow.GetFormattedText() + "[/color].\n";
            else
            {
                addText += "[color=5D76EA]" + powerFlow.GetFormattedText() + "[/color].\n";
            }
        }
        __result = addText + __result;
    }
}