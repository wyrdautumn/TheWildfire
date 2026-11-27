using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using TheWildfire.TheWildfireCode.Cards.ModInterop.HeroExpansion;

namespace TheWildfire.TheWildfireCode.Powers;

public class BlazingTalonUpgradePower : TheWildfirePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromCard<TalonSurge>(true)];

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
    {
        if (player.Creature != Owner)
            return;
        Flash();
        IEnumerable<CardModel> surges = await TalonSurge.CreateInHand(player, Amount, combatState, player);
        CardCmd.Upgrade(surges, CardPreviewStyle.None);
    }
}