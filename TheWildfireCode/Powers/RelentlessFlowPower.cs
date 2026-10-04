using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;

namespace TheWildfire.TheWildfireCode.Powers;

public class RelentlessFlowPower : TheWildfirePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.Static(WildfireKeywords.FlowStatic)];

    public override int DisplayAmount {
        get
        {
            int flows = 1;
            if (Owner.HasPower<AttackFlowPower>())
                flows += 1;
            if (Owner.HasPower<SkillFlowPower>())
                flows += 1;
            if (Owner.HasPower<PowerFlowPower>())
                flows += 1;
            int display = flows * Amount;
            return Math.Min(display, Amount * 3);
        }
    }

    public override async Task FlowAchieved(PlayerChoiceContext choiceContext, Player achiever, CardType type)
    {
        decimal flows = 0;
        if (Owner.HasPower<AttackFlowPower>())
            flows += 1;
        if (Owner.HasPower<SkillFlowPower>())
            flows += 1;
        if (Owner.HasPower<PowerFlowPower>())
            flows += 1;
        decimal damage = flows * Amount;
        await CreatureCmd.Damage(choiceContext, CombatState.HittableEnemies, damage, ValueProp.Unpowered, Owner);
        InvokeDisplayAmountChanged();
    }

    public override Task AfterSideTurnEndLate(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        InvokeDisplayAmountChanged();
        return Task.CompletedTask;
    }
}