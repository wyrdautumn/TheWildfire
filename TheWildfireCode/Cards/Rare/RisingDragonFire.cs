using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Cards.Token;
using TheWildfire.TheWildfireCode.Firepower;

namespace TheWildfire.TheWildfireCode.Cards.Rare;

public class RisingDragonFire() : TheWildfireCard(2,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
    new CalculationBaseVar(1),
    new CalculationExtraVar(1),
    new CalculatedVar("fireballs").WithMultiplier(Calc)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [..HoverTipFactory.FromCardWithCardHoverTips<Fireball>(IsUpgraded),HoverTipFactory.Static(WildfireKeywords.Firepower)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    
    private static decimal Calc(CardModel card, Creature? arg2)
    {
        return FirepowerController.CalculateOverheatDamageWithoutMitigation(card.Owner);
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (CombatState == null)
            return;
        int overheat = (int) ((CalculatedVar) DynamicVars["fireballs"]).Calculate(null);
        List<CardModel> fireballs = new List<CardModel>();
        for (int i = overheat; i > 0; i--)
            fireballs.Add(CombatState.CreateCard<Fireball>(Owner));
        await CardPileCmd.AddGeneratedCardsToCombat(fireballs, PileType.Draw, Owner, CardPilePosition.Random);
        if (IsUpgraded)
            foreach (CardModel card in fireballs)
                CardCmd.Upgrade(card);
    }

    protected override void OnUpgrade()
    {

    }
}