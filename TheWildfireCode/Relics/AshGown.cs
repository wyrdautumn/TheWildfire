using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheWildfire.TheWildfireCode.Powers;
using TheWildfire.TheWildfireCode.Relics;

namespace TheWildfire.TheWildfireCode.Relics;

public class AshGown() : TheWildfireRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Uncommon;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<RagingFirePower>(5)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<RagingFirePower>()];

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == Owner)
            await PowerCmd.Apply<RagingFirePower>(choiceContext, Owner.Creature, DynamicVars["RagingFirePower"].BaseValue, Owner.Creature, null);
    }
}