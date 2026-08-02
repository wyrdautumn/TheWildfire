using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using TheWildfire.TheWildfireCode.Character;
using TheWildfire.TheWildfireCode.Firepower;

namespace TheWildfire.TheWildfireCode.Potions;

[Pool(typeof(TheWildfirePotionPool))]
public abstract class TheWildfirePotion : CustomPotionModel, IFirepowerListener
{
    public virtual Task AfterIgnite(PlayerChoiceContext choiceContext, int amount, Player igniter) => Task.CompletedTask;
    public virtual Task AfterScorch(PlayerChoiceContext choiceContext, int amount, Player scorcher) => Task.CompletedTask;
    public virtual Task AfterExert(PlayerChoiceContext choiceContext, int amount, Player exerter)  => Task.CompletedTask;
    public virtual Task AfterFlare(PlayerChoiceContext choiceContext, int amount, Player flarer)  => Task.CompletedTask;
    public virtual Task AfterburnPlayed(PlayerChoiceContext choiceContext, Player afterburner)  => Task.CompletedTask;
    public virtual Task AfterBurnStatusTrigger(PlayerChoiceContext choiceContext, Player burnee)  => Task.CompletedTask;

    public virtual Task AfterOverheatDamage(PlayerChoiceContext choiceContext, int damage, int damageMitigated,
        Player overheater) => Task.CompletedTask;
}