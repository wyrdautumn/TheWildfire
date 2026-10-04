using BaseLib.Utils;
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
using TheWildfire.TheWildfireCode.Powers;

namespace TheWildfire.TheWildfireCode.Cards.Common;

public class ScatterEmbers() : TheWildfireCard(0,
    CardType.Attack, CardRarity.Common,
    TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(3, ValueProp.Move), new ExertVar(3),
        new CalculationBaseVar(0),
        new CalculationExtraVar(1),
        new CalculatedVar("Debuff").WithMultiplier(Calc)];
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
        if (CombatState == null)
            return;
        int exert = ResolveExert();
        bool fullExert = false;
        await CommonActions.CardAttack(this, play, vfx:"vfx/vfx_attack_slash").Execute(choiceContext);
        await PowerCmd.Apply<ScatterEmbersPower>(choiceContext, CombatState.HittableEnemies,
            ((CalculatedVar)DynamicVars["Debuff"]).Calculate(null), Owner.Creature, this);
        if (exert >= DynamicVars["Exert"].IntValue)
            fullExert = true;
        await FirepowerController.Exert(choiceContext, exert, Owner, fullExert);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Debuff"].UpgradeValueBy(1);
    }
}