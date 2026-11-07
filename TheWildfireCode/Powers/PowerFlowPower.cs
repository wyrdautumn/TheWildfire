using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using TheWildfire.TheWildfireCode.Firepower;
using TheWildfire.TheWildfireCode.Nodes;

namespace TheWildfire.TheWildfireCode.Powers;

public class PowerFlowPower : TheWildfirePower
{
    public override PowerType Type => PowerType.None;
    public override PowerStackType StackType => PowerStackType.Single;
    protected override bool IsVisibleInternal => false;
    
    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        await PowerCmd.Remove(this);
        if (!Owner.HasPower<SkillFlowPower>() && !Owner.HasPower<AttackFlowPower>() && Owner.Player != null && Owner.Player.Character is Character.TheWildfire)
            FlowSingleton.UpdateFlowVisuals(Owner);
    }
}