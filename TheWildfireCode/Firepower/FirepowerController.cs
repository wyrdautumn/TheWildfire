using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;

namespace TheWildfire.TheWildfireCode.Cards;

public class FirepowerController() : CustomSingletonModel(HookType.Combat)
{
    public static readonly SpireField<PlayerCombatState, int> Firepower = new(() => 0);

    public static void Stoke(Player player, int stoke)
    {
        if (player.PlayerCombatState == null)
            return;
        int val = Firepower.Get(player.PlayerCombatState);
        Firepower.Set(player.PlayerCombatState, val + stoke);
        
    }

    public static async Task AfterStoke(ICombatState combatState, int amount, Player stoker)
    {
        
    }
    
    
    
}