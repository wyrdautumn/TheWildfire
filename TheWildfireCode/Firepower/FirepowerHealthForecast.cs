using BaseLib.Hooks;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Nodes.Combat;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Nodes;

namespace TheWildfire.TheWildfireCode.Firepower;

public class FirepowerHealthForecast : IHealthBarForecastSource
{
    public static readonly AddedNode<NHealthBar, WildfireHpTrackers> WildfireHealthTrackers =
        new("res://TheWildfire/scenes/wildfire_hp_trackers.tscn",(bar,counters) =>
        {
        });
    
    public IEnumerable<HealthBarForecastSegment> GetHealthBarForecastSegments(HealthBarForecastContext context)
    {
        int amount = 0;
        if (context.CombatState != null)
        {
            Player? localPlayer = LocalContext.GetMe(context.CombatState.RunState);
            if (localPlayer == null || localPlayer.PlayerCombatState == null ||
                FirepowerController.Firepower.Get(localPlayer.PlayerCombatState) <= 0 || context.Creature.Player == null || context.Creature.Player != LocalContext.GetMe(context.CombatState.RunState))
                amount = 0;
            else
            {
                Player player = context.Creature.Player;
                amount = Math.Max(0, FirepowerController.CalculateOverheatDamage(player) - context.Creature.Block);
            }
        }
        return [new HealthBarForecastSegment(amount, Color.FromHtml("#fa95ff"), HealthBarForecastDirection.FromRight, 1)];
    }

}