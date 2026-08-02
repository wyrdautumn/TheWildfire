using BaseLib.Cards.Variables;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Firepower;
using TheWildfire.TheWildfireCode.Powers;

namespace TheWildfire.TheWildfireCode.Cards.Rare;

public class DesertWindFlurry() : TheWildfireCard(2,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4, ValueProp.Move),
        new CalculationBaseVar(6),
        new CalculationExtraVar(-1),
        new CalculatedVar("hits").WithMultiplier(Calc)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [WildfireKeywords.Discipline];
    
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
        await CommonActions.CardAttack(this, play, (int) ((CalculatedVar) DynamicVars["hits"]).Calculate(null), vfx: "vfx/vfx_attack_slash").Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1);
        DynamicVars.CalculationBase.UpgradeValueBy(1);
    }
}