using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;

namespace TheWildfire.TheWildfireCode.Powers;

public class OverflowingAthleticism : TheWildfirePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterOverflow(PlayerChoiceContext choiceContext, int amount, Player overflower)
    {
        if (overflower != Owner.Player)
            return;
        int power = CombatState.RunState.Rng.Niche.NextInt(0, 2);
        if (power == 0)
            await PowerCmd.Apply<StrengthPower>(choiceContext, Owner, 1, Owner, null);
        else if (power == 1)
            await PowerCmd.Apply<DexterityPower>(choiceContext, Owner, 1, Owner, null);
        else if (power == 2)
            await PowerCmd.Apply<ConstitutionPower>(choiceContext, Owner, 1, Owner, null);
        else
            GD.Print("It shouldn't be possible to see this.");
    }
}