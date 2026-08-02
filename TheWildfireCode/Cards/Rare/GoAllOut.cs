using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Cards.Token;
using TheWildfire.TheWildfireCode.Firepower;

namespace TheWildfire.TheWildfireCode.Cards.Rare;

public class GoAllOut() : TheWildfireCard(0,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
    new CalculationBaseVar(0),
    new ExtraDamageVar(1),
    new CalculatedDamageVar(ValueProp.Move).WithMultiplier(Calc).WithTooltip("THEWILDFIRE-EXERT-X")];

    protected override bool HasEnergyCostX => true;

    private static decimal Calc(CardModel card, Creature? arg2)
    {
        if (card.Owner.PlayerCombatState == null)
            return 0;
        return FirepowerController.Firepower.Get(card.Owner.PlayerCombatState);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        int hits = ResolveEnergyXValue();
        if (IsUpgraded)
        {
            hits += 1;
        }
        await CommonActions.CardAttack(this, play, hits, vfx:"vfx/vfx_attack_slash").Execute(choiceContext);
        int exert = 0;
        if (Owner.PlayerCombatState != null)
            exert = FirepowerController.Firepower.Get(Owner.PlayerCombatState);
        await FirepowerController.Exert(choiceContext, exert, Owner); 
    }

    protected override void OnUpgrade()
    {
        
    }
}