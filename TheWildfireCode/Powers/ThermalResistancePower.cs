using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Cards;
using TheWildfire.TheWildfireCode.Cards.Status;

namespace TheWildfire.TheWildfireCode.Powers;

public class ThermalResistancePower : TheWildfirePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromCard<Burn>(),HoverTipFactory.FromCard<Afterburn>(),HoverTipFactory.FromPower<ConstitutionPower>()];

    public override async Task AfterBurnStatusTrigger(PlayerChoiceContext choiceContext, Player burnee)
    {
        if (burnee.Creature == Owner)
            await PowerCmd.Apply<ConstitutionPower>(choiceContext, Owner, Amount, Owner, null);
    }
}