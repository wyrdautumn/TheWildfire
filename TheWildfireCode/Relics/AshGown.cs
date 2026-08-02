using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Powers;

namespace TheWildfire.TheWildfireCode.Relics;

public class AshGown : TheWildfireRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Rare;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(WildfireKeywords.Scorch),HoverTipFactory.FromPower<HeatExhaustionPower>()];

    public override async Task AfterScorch(PlayerChoiceContext choiceContext, int amount, Player scorcher)
    {
        if (scorcher == Owner && Owner.Creature.CombatState != null)
            await PowerCmd.Apply<HeatExhaustionPower>(choiceContext, Owner.Creature.CombatState.HittableEnemies, 1,
                Owner.Creature, null);
    }
}