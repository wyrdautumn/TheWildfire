using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using TheWildfire.TheWildfireCode.Cards.Token;

namespace TheWildfire.TheWildfireCode.Powers;

public class CrowsMomentumUpgradePower : TheWildfirePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [..HoverTipFactory.FromCardWithCardHoverTips<SolarPower>(true)];

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
    {
        if (player.Creature == Owner)
        {
            var solarPowers = await SolarPower.CreateInHand(player, Amount, CombatState);
            foreach (CardModel card in solarPowers)
                CardCmd.Upgrade(card);
        }
    }
}