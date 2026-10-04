using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Cards.Variables;
using TheWildfire.TheWildfireCode.Firepower;

namespace TheWildfire.TheWildfireCode.Cards.Common;

public class BurningJet() : TheWildfireCard(2,
    CardType.Skill, CardRarity.Common,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new ExertVar(9),
        new CalculationBaseVar(0),
        new CalculationExtraVar(1),
        new CalculatedBlockVar(ValueProp.Move).WithMultiplier(Calc)];

    public override bool GainsBlock => true;

    protected override HashSet<CardTag> CanonicalTags
    {
        get => new HashSet<CardTag>() { WildfireKeywords.ExertTag };
    }
    
    private static decimal Calc(CardModel card, Creature? arg2)
    {
        if (card is not TheWildfireCard)
            return 0;
        TheWildfireCard thisCard = (TheWildfireCard) card;
        return thisCard.ResolveExert();
    }


    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        int exert = ResolveExert();
        bool fullExert = false;
        Decimal amount = await CreatureCmd.GainBlock(Owner.Creature, exert, ValueProp.Move, play);
        await PowerCmd.Apply<BlockNextTurnPower>(choiceContext, Owner.Creature, amount, Owner.Creature, this);
        if (exert >= DynamicVars["Exert"].IntValue)
            fullExert = true;
        await FirepowerController.Exert(choiceContext, exert, Owner, fullExert);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Exert"].UpgradeValueBy(3);
    }
}