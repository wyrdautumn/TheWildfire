using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Firepower;
using TheWildfire.TheWildfireCode.Patches;

namespace TheWildfire.TheWildfireCode.Cards.Uncommon;

public class AdamantSoul() : TheWildfireCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(7),
        new CalculationExtraVar(7),
        new CalculatedBlockVar(ValueProp.Move).WithMultiplier(Calc)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [WildfireKeywords.PowerFlow];
    public override bool GainsBlock => true;
    private static decimal Calc(CardModel card, Creature? arg2)
    {
        if (OverdriveGlowPatch.CheckFlow(card, CardType.Power))
            return 1;
        return 0;
    }
    
    protected override bool ShouldGlowGoldInternal => OverdriveGlowPatch.CheckFlow(this, CardType.Power);

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        decimal block = DynamicVars.CalculationBase.BaseValue;
        if (FlowSingleton.WillTriggerFlow(this))
            block += DynamicVars.CalculationExtra.BaseValue;
        await CreatureCmd.GainBlock(Owner.Creature, block, ValueProp.Move, play);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.CalculationBase.UpgradeValueBy(2);
        DynamicVars.CalculationExtra.UpgradeValueBy(2);
    }
}