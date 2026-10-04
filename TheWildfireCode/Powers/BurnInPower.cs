using BaseLib.Abstracts;
using BaseLib.Hooks;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;

namespace TheWildfire.TheWildfireCode.Powers;

public class BurnInPower() : TheWildfirePower, IHasSecondAmount
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(0, ValueProp.Unblockable | ValueProp.Unpowered)];
    
    public override IEnumerable<HealthBarForecastSegment> GetHealthBarForecastSegments(HealthBarForecastContext context)
    {
        return [new HealthBarForecastSegment(DynamicVars.Damage.IntValue, Color.FromHtml("#7a37a8"), HealthBarForecastDirection.FromRight)];
    }
    
    public void SetDamage(Decimal damage)
    {
        this.AssertMutable();
        this.DynamicVars.Damage.BaseValue = damage;
    }

    public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (participants.Contains(Owner))
            return;
        NCombatRoom? instance = NCombatRoom.Instance;
        if (instance != null)
            instance.CombatVfxContainer.AddChildSafely(NFireSmokePuffVfx.Create(Owner));
        await CreatureCmd.Damage(choiceContext, Owner, DynamicVars.Damage.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered, null, null);
        await PowerCmd.Decrement(this);
    }

    public string GetSecondAmount()
    {
        return DynamicVars.Damage.IntValue.ToString();
    }
}