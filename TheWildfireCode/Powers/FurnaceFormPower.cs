using BaseLib.Hooks;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Firepower;

namespace TheWildfire.TheWildfireCode.Powers;

public class FurnaceFormPower : TheWildfirePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.Static(WildfireKeywords.IgniteStatic),HoverTipFactory.Static(WildfireKeywords.Overheat)];
    
    public override async Task AfterEnergyResetLate(Player player)
    {
        if (Owner.Player == null)
            return;
        Flash();
        await FirepowerController.Ignite(new ThrowingPlayerChoiceContext(), Amount, Owner.Player);
    }

    public override async Task AfterOverheatDamage(PlayerChoiceContext choiceContext, int damage, int damageMitigated, Player overheater)
    {
        if (overheater != Owner.Player || damage <= 0)
            return;
        await PowerCmd.Apply<RagingFirePower>(choiceContext, Owner, damage, Owner, null);
    }
}