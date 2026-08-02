using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Firepower;

namespace TheWildfire.TheWildfireCode.Powers;

public class VolcanicEscapePower : TheWildfirePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(WildfireKeywords.Scorch)];
    
        public override async Task BeforeDamageReceived(PlayerChoiceContext choiceContext, Creature target, decimal amount, ValueProp props,
            Creature? dealer, CardModel? cardSource)
        {
            if (Owner.Player == null || Owner.Player.PlayerCombatState == null)
                return;
            if (target == Owner && dealer != Owner && props.IsPoweredAttack())
            {
                for(int i = Amount; i > 0; i--)
                {
                    await FirepowerController.Scorch(choiceContext, Owner.Player);
                }
                for(int i = Amount; i > 0; i--)
                {
                    await CreatureCmd.GainBlock(Owner,
                        FirepowerController.Firepower.Get(Owner.Player.PlayerCombatState), ValueProp.Unpowered, null,
                        true);
                }
                await PowerCmd.Remove(this);
            }
        }
}