using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using TheWildfire.TheWildfireCode.Cards;

namespace TheWildfire.TheWildfireCode.Powers;

public class OverflowingAthleticismPower : TheWildfirePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.Static(WildfireKeywords.FlareStatic),HoverTipFactory.FromPower<StrengthPower>(),HoverTipFactory.FromPower<DexterityPower>(),HoverTipFactory.FromPower<ConstitutionPower>()];
    
    public override async Task AfterFlare(PlayerChoiceContext choiceContext, int amount, Player flarer)
    {
        if (flarer != Owner.Player)
            return;
        int power = CombatState.RunState.Rng.Niche.NextInt(0, 299);
        if (power <= 99)
            await PowerCmd.Apply<StrengthPower>(choiceContext, Owner, 1, Owner, null);
        else if (power <= 199)
            await PowerCmd.Apply<DexterityPower>(choiceContext, Owner, 1, Owner, null);
        else if (power <= 299)
            await PowerCmd.Apply<ConstitutionPower>(choiceContext, Owner, 1, Owner, null);
        else
            GD.Print("It shouldn't be possible to see this.");
    }
}