using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Firepower;

namespace TheWildfire.TheWildfireCode.Cards.Rare;

public class GoAllOut() : TheWildfireCard(0,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(0),
        new ExtraDamageVar(1),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier(Calc)];
    protected override bool HasEnergyCostX => true;
    protected override HashSet<CardTag> CanonicalTags
    {
        get => new HashSet<CardTag>() { WildfireKeywords.ExertTag };
    }

    private static decimal Calc(CardModel card, Creature? arg2)
    {
        if (card is not TheWildfireCard)
            return 0;
        TheWildfireCard thisCard = (TheWildfireCard)card;
        return thisCard.ResolveExertAll();
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (play.Target == null)
            return;
        int hits = ResolveEnergyXValue();
        if (IsUpgraded)
            hits += 1;
        int exert = ResolveExertAll();
        await CommonActions.CardAttack(this, play, hits, vfx: "vfx/vfx_attack_slash")
            .Execute(choiceContext);
        await FirepowerController.ExertAll(choiceContext, Owner, false);
    }

    protected override void OnUpgrade()
    {

    }
}