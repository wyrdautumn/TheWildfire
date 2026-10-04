using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Powers;

namespace TheWildfire.TheWildfireCode.Cards.Uncommon;

public class StokeTheFire() : TheWildfireCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(3),
        new CalculationExtraVar(1),
        new CalculatedVar("RagingFire").WithMultiplier(Calc)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<RagingFirePower>(),HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];
    
    private static decimal Calc(CardModel card, Creature? arg2)
    {
        CardPile? pile = CardPile.Get(PileType.Exhaust, card.Owner);
        if (pile != null)
            return pile.Cards.Count;
        return 0;
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<RagingFirePower>(choiceContext, Owner.Creature,
            ((CalculatedVar)DynamicVars["RagingFire"]).Calculate(null), Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}