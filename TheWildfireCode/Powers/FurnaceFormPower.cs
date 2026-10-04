using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Firepower;

namespace TheWildfire.TheWildfireCode.Powers;

public class FurnaceFormPower : TheWildfirePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.Static(WildfireKeywords.IgniteStatic),HoverTipFactory.FromPower<RagingFirePower>()];

    public override async Task AfterEnergyReset(Player player)
    {
        if (player.Creature == Owner)
            await FirepowerController.Ignite(new BlockingPlayerChoiceContext(), Amount, player);
    }

    public override async Task AfterEnergyResetLate(Player player)
    {
        if (player.Creature == Owner)
        {
            int fire = FirepowerController.CalculateOverheatDamage(player);
            await PowerCmd.Apply<RagingFirePower>(new BlockingPlayerChoiceContext(), Owner, fire, Owner, null);
        }
    }
}