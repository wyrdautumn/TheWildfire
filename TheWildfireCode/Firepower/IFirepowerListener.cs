using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace TheWildfire.TheWildfireCode.Firepower;

public interface IFirepowerListener
{
    Task AfterIgnite(PlayerChoiceContext choiceContext, int amount, Player igniter);
    Task AfterScorch(PlayerChoiceContext choiceContext, int amount, Player scorcher);
    Task AfterExert(PlayerChoiceContext choiceContext, int amount, Player exerter);
    Task AfterFlare(PlayerChoiceContext choiceContext, int amount, Player flarer);
    Task AfterburnPlayed(PlayerChoiceContext choiceContext, Player afterburner);
    Task AfterBurnStatusTrigger(PlayerChoiceContext choiceContext, Player burnee);
    Task AfterOverheatDamage(PlayerChoiceContext choiceContext, int damage, int damageMitigated, Player overheater);
}