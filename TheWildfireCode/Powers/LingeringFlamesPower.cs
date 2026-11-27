using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheWildfire.TheWildfireCode.Powers;

public class LingeringFlamesPower : TheWildfirePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer, DamageResult result, ValueProp props,
        Creature target, CardModel? cardSource)
    {
        if (dealer != Owner || !props.IsPoweredAttack() || result.UnblockedDamage <= 0)
            return;
        bool applyNew = true;
        foreach (PowerModel power in target.Powers.Where(p => p is LingeringFlamesDebuff lingering && lingering.CheckOwner() == Owner))
        {
            await PowerCmd.ModifyAmount(choiceContext, power, Amount, Owner, null);
            applyNew = false;
        }
        if (applyNew)
        {
            await PowerCmd.Apply<LingeringFlamesDebuff>(choiceContext, target, Amount, Owner, null);
        }
    }
}