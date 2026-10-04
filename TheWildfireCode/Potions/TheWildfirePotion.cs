using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
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
    public virtual Task AfterExert(PlayerChoiceContext choiceContext, int amount, Player exerter, bool fullExert)  => Task.CompletedTask;
    public virtual Task AfterBurnStatusTrigger(PlayerChoiceContext choiceContext, Player burnee)  => Task.CompletedTask;

    public virtual Task AfterOverheatDamage(PlayerChoiceContext choiceContext, int damage, int damageMitigated,
        Player overheater) => Task.CompletedTask;
    public virtual Task FlowAchieved(PlayerChoiceContext choiceContext, Player achiever, CardType type) => Task.CompletedTask;
}