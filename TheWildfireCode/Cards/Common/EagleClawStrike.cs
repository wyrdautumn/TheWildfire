using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Firepower;

namespace TheWildfire.TheWildfireCode.Cards.Common;

public class EagleClawStrike() : TheWildfireCard(1,
    CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(16, ValueProp.Move), 
        new CalculationBaseVar(0),
        new CalculationExtraVar(1),
        new CalculatedVar("SelfDamage").WithMultiplier(Calc)];
    
    private static decimal Calc(CardModel card, Creature? arg2)
    {
        if (card.Owner.PlayerCombatState == null)
            return 0;
        return FirepowerController.Firepower.Get(card.Owner.PlayerCombatState);
    }
    
    protected override HashSet<CardTag> CanonicalTags
    {
        get => new HashSet<CardTag>() { CardTag.Strike };
    }

    protected override bool ShouldGlowRedInternal
    {
        get
        {
            if (Owner.PlayerCombatState == null)
                return false;
            int damage = FirepowerController.Firepower.Get(Owner.PlayerCombatState);
            int health = Owner.Creature.CurrentHp + Owner.Creature.Block;
            if (damage >= health)
                return true;
            return false;
        }
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await FirepowerController.DealFirepowerDamage(choiceContext, Owner);
        await CommonActions.CardAttack(this, play, vfx:"vfx/vfx_attack_slash").Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(5);
    }
}