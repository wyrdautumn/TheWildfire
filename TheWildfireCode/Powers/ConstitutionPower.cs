using BaseLib.Hooks;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Firepower;

namespace TheWildfire.TheWildfireCode.Powers;

public class ConstitutionPower : TheWildfirePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override object InitInternalData() => new Data();
    
    public override IEnumerable<HealthBarForecastSegment> GetHealthBarForecastSegments(HealthBarForecastContext context)
    {
        int amount = GetAbsorbRemaining();
        if (Owner.Player != null)
            amount -= FirepowerController.CalculateOverheatDamage(Owner.Player);
        if (amount < 0)
            amount = 0;
        return [new HealthBarForecastSegment(amount, Color.FromHtml("#7a37a8"), HealthBarForecastDirection.FromLeft, 0, null, null, HealthBarForecastLeftOriginLayout.Chained, 0, false)];
    }

    public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (side != Owner.Side)
            return Task.CompletedTask;
        GetInternalData<Data>().DamageAbsorbed = 0;
        return Task.CompletedTask;
    }
    
    public override decimal ModifyHpLostAfterOsty(Creature target, decimal amount, ValueProp props, Creature? dealer,
        CardModel? cardSource)
    {
        int absorb = GetAbsorbRemaining();
        if (target != Owner || absorb <= 0)
            return amount;
        if (absorb >= amount)
        {
            GetInternalData<Data>().DamageAbsorbed += (int) amount;
            return 0;
        }
        GetInternalData<Data>().DamageAbsorbed += absorb;
        return amount - absorb;
    }

    public int GetAbsorbRemaining()
    {
        return Math.Max(0, Amount - GetInternalData<Data>().DamageAbsorbed);
    }

    private class Data
    {
        public int DamageAbsorbed = 0;
    }
}