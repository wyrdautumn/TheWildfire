using BaseLib.Abstracts;
using BaseLib.Hooks;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Nodes.Combat;
using TheWildfire.TheWildfireCode.Nodes;
using TheWildfire.TheWildfireCode.Powers;

namespace TheWildfire.TheWildfireCode.Firepower;

public class FirepowerConstitutionHealthBarSegmentSingleton() : CustomSingletonModel(HookType.Combat),IHealthBarForecastSource
{
    public static readonly AddedNode<NHealthBar, WildfireHpTrackers> WildfireHealthTrackers =
        new("res://TheWildfire/scenes/wildfire_hp_trackers.tscn",(bar,counters) =>
        {
        });
    
    public IEnumerable<HealthBarForecastSegment> GetHealthBarForecastSegments(HealthBarForecastContext context)
    {
        int result;
        ConstitutionPower? constitution = context.Creature.GetPower<ConstitutionPower>();
        if (context.Creature.Player == null || context.Creature.Player.PlayerCombatState == null || !context.Creature.IsAlive || constitution == null)
            result = 0;
        else
        {
            Player player = context.Creature.Player;
            int amount1 = FirepowerController.CalculateOverheatDamage(player);
            int amount2 = constitution.GetAbsorbRemaining();
            if (amount1 >= amount2)
                result = amount2;
            else
                result = amount1;
        }
        return [new HealthBarForecastSegment(result, Color.FromHtml("#a55ad9"), HealthBarForecastDirection.FromLeft, 1, null, null, HealthBarForecastLeftOriginLayout.Chained, 0, false)];
    }
}