using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheWildfire.TheWildfireCode.Powers;

public class FistOfTheThousandthSunPower : TheWildfirePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<HeatExhaustionPower>()];
    
    protected override object InitInternalData() => new Data();

    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        List<DamageResult> results = CombatManager.Instance.History.Entries.OfType<CreatureAttackedEntry>().Where(c => c.Actor == Owner)
            .SelectMany(r => r.DamageResults.Where(r => r.UnblockedDamage > 0)).ToList();
        GetInternalData<Data>().creaturesHit.AddRange(results.Select(r => r.Receiver).Distinct().ToList());
        return Task.CompletedTask;
    }

    public override async Task AfterDamageGiven(
        PlayerChoiceContext choiceContext,
        Creature? dealer,
        DamageResult result,
        ValueProp props,
        Creature target,
        CardModel? cardSource)
    {
        if (dealer != this.Owner || !props.IsPoweredAttack() || result.UnblockedDamage <= 0 || GetInternalData<Data>().creaturesHit.Contains(target))
            return;
        await PowerCmd.Apply<HeatExhaustionPower>(choiceContext, target, Amount, Owner, null);
        GetInternalData<Data>().creaturesHit.Add(target);
    }
    
    public override Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (!participants.Contains(Owner))
            return Task.CompletedTask;
        GetInternalData<Data>().creaturesHit.Clear();
        return Task.CompletedTask;
    }
    
    private class Data
    {
        public List<Creature> creaturesHit = new List<Creature>();
    }
}