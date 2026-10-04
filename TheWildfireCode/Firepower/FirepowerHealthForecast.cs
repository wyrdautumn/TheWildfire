using BaseLib.Hooks;
using Godot;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;

namespace TheWildfire.TheWildfireCode.Firepower;

public class FirepowerHealthForecast : IHealthBarForecastSource
{
    public IEnumerable<HealthBarForecastSegment> GetHealthBarForecastSegments(HealthBarForecastContext context)
    {
        int amount = 0;
        if (context.CombatState != null)
        {
            Player? localPlayer = LocalContext.GetMe(context.CombatState.RunState);
            if (localPlayer == null || localPlayer.PlayerCombatState == null ||
                FirepowerController.Firepower.Get(localPlayer.PlayerCombatState) <= 0)
                amount = 0;
            else if (context.Creature.IsEnemy)
                amount = Math.Max(0, FirepowerController.Firepower.Get(localPlayer.PlayerCombatState) - context.Creature.Block);
            else if (context.Creature.Player != null && context.Creature.Player == LocalContext.GetMe(context.CombatState.RunState))
            {
                Player player = context.Creature.Player;
                amount = Math.Max(0, FirepowerController.CalculateOverheatDamage(player) - context.Creature.Block);
            }
        }
        return [new HealthBarForecastSegment(amount, Color.FromHtml("#fa95ff"), HealthBarForecastDirection.FromRight, 1)];
    }

}