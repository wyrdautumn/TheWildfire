using BaseLib.Cards.Variables;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Cards.Variables;
using TheWildfire.TheWildfireCode.Firepower;

namespace TheWildfire.TheWildfireCode.Cards.Uncommon;

public class BurnIn() : TheWildfireCard(0,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new ExertVar(10),
    new CalculationBaseVar(0),
    new ExtraDamageVar(1),
    new CalculatedDamageVar(ValueProp.Move | ValueProp.Unblockable | ValueProp.Unpowered).WithMultiplier(Calc),
    new DynamicVar("ExhaustBase", 0),
    new DynamicVar("ExhaustExtra", 1),
    new CustomCalculatedVar("Exhaust").WithMultiplier(Calc2)];
    
    private static decimal Calc(CardModel card, Creature? arg2)
    {
        if (card is not TheWildfireCard)
            return 0;
        TheWildfireCard thisCard = (TheWildfireCard) card;
        return thisCard.ResolveExert();
    }
    
    private static decimal Calc2(CardModel card, Creature? arg2)
    {
        if (card is not TheWildfireCard)
            return 0;
        TheWildfireCard thisCard = (TheWildfireCard) card;
        return thisCard.ResolveExert() / 5;
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (play.Target == null)
            return;
        int exert = ResolveExert();
        await CreatureCmd.Damage(choiceContext, play.Target, DynamicVars.CalculatedDamage.Calculate(null), ValueProp.Move | ValueProp.Unblockable | ValueProp.Unpowered, this, play);
        int exhaust = exert / 5;
        await FirepowerController.Exert(choiceContext, exert, Owner);
        if (exhaust > 0)
        {
            var prefs = new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, exhaust);
            var selected = await CardSelectCmd.FromHand(choiceContext, Owner, prefs, null, this);
            foreach (CardModel card in selected)
            {
                await CardCmd.Exhaust(choiceContext, card);
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Exert"].UpgradeValueBy(5);
    }
}