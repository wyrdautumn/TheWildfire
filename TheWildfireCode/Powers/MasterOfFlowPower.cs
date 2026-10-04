using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Cards.Uncommon;
using TheWildfire.TheWildfireCode.Patches;

namespace TheWildfire.TheWildfireCode.Powers;

public class MasterOfFlowPower : TheWildfirePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.Static(WildfireKeywords.FlowStatic)];

    public override bool TryModifyEnergyCostInCombat(CardModel card, decimal originalCost, out decimal modifiedCost)
    {
        modifiedCost = originalCost;
        if (card.Keywords.Contains(WildfireKeywords.AttackFlow) && OverdriveGlowPatch.CheckFlow(card, CardType.Attack))
        {
            modifiedCost -= Amount;
            return true;
        }
        if (card.Keywords.Contains(WildfireKeywords.SkillFlow) && OverdriveGlowPatch.CheckFlow(card, CardType.Skill))
        {
            modifiedCost -= Amount;
            return true;
        }
        if (card.Keywords.Contains(WildfireKeywords.PowerFlow) && OverdriveGlowPatch.CheckFlow(card, CardType.Power))
        {
            modifiedCost -= Amount;
            return true;
        }
        if (card is MasterfulSpiral spiral && spiral.IsFullExert())
        {
            modifiedCost -= Amount;
            return true;
        }
        return false;
    }
}