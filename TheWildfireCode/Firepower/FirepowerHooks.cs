using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace TheWildfire.TheWildfireCode.Firepower;

public class FirepowerHooks
{
    public static async Task AfterIgnite(ICombatState combatState, PlayerChoiceContext choiceContext, int amount, Player igniter)
    {
        foreach (var model in combatState.IterateHookListeners().ToList())
        {
            if (model is IFirepowerListener listener)
            {
                await listener.AfterIgnite(choiceContext, amount, igniter);
                model.InvokeExecutionFinished();
            }
        }
    }

    public static async Task AfterScorch(ICombatState combatState, PlayerChoiceContext choiceContext, int amount, Player scorcher)
    {
        foreach (var model in combatState.IterateHookListeners().ToList())
        {
            if (model is IFirepowerListener listener)
            {
                await listener.AfterScorch(choiceContext, amount, scorcher);
                model.InvokeExecutionFinished();
            }
        }
    }
    
    public static async Task AfterExert(ICombatState combatState, PlayerChoiceContext choiceContext, int amount, Player exerter)
    {
        foreach (var model in combatState.IterateHookListeners().ToList())
        {
            if (model is IFirepowerListener listener)
            {
                await listener.AfterExert(choiceContext, amount, exerter);
                model.InvokeExecutionFinished();
            }
        }
    }

    public static async Task AfterFlare(ICombatState combatState, PlayerChoiceContext choiceContext, Player flarer, int amount)
    {
        foreach (var model in combatState.IterateHookListeners().ToList())
        {
            if (model is IFirepowerListener listener)
            {
                await listener.AfterFlare(choiceContext, amount, flarer);
                model.InvokeExecutionFinished();
            }
        }
    }
    
    public static async Task AfterburnPlayed(ICombatState combatState, PlayerChoiceContext choiceContext, Player afterburner)
    {
        foreach (var model in combatState.IterateHookListeners().ToList())
        {
            if (model is IFirepowerListener listener)
            {
                await listener.AfterburnPlayed(choiceContext, afterburner);
                model.InvokeExecutionFinished();
            }
        }
    }
    
    public static async Task AfterBurnStatusTrigger(ICombatState combatState, PlayerChoiceContext choiceContext, Player burnee)
    {
        foreach (var model in combatState.IterateHookListeners().ToList())
        {
            if (model is IFirepowerListener listener)
            {
                await listener.AfterBurnStatusTrigger(choiceContext, burnee);
                model.InvokeExecutionFinished();
            }
        }
    }
    
    public static async Task AfterOverheatDamage(ICombatState combatState, PlayerChoiceContext choiceContext, int damage, int damageMitigated,
        Player overheater)
    {
        foreach (var model in combatState.IterateHookListeners().ToList())
        {
            if (model is IFirepowerListener listener)
            {
                await listener.AfterOverheatDamage(choiceContext,  damage,  damageMitigated, overheater);
                model.InvokeExecutionFinished();
            }
        }
    }
}