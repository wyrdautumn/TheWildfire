using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Firepower;
using TheWildfire.TheWildfireCode.Powers;

namespace TheWildfire.TheWildfireCode.Cards.Uncommon;

public class HarmoniousDiscipline() : TheWildfireCard(1,
    CardType.Power, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
    new CalculationBaseVar(4),
    new CalculationExtraVar(-1),
    new CalculatedVar("StrengthPower").WithMultiplier(Calc)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [WildfireKeywords.Discipline];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<StrengthPower>()];
    
    private static decimal Calc(CardModel card, Creature? arg2)
    {
        if (card.Owner.PlayerCombatState == null)
            return 0;
        int firepower = FirepowerController.Firepower.Get(card.Owner.PlayerCombatState) - card.Owner.Creature.GetPowerAmount<WalkThroughTheConflagrationPower>();
        if (firepower > card.DynamicVars.CalculationBase.BaseValue)
            return card.DynamicVars.CalculationBase.BaseValue;
        return firepower;
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.ApplySelf<StrengthPower>(choiceContext, this,
            ((CalculatedVar)DynamicVars["StrengthPower"]).Calculate(null));
    }

    protected override void OnUpgrade()
    {
        DynamicVars.CalculationBase.UpgradeValueBy(2);
    }
}