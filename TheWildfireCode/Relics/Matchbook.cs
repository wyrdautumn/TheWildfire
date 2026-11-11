using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Cards.Token;
using TheWildfire.TheWildfireCode.Firepower;
using TheWildfire.TheWildfireCode.Relics;

namespace TheWildfire.TheWildfireCode.Relics;

public class Matchbook() : TheWildfireRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Rare;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromCard<Firebolt>()];

    public override async Task AfterShuffle(PlayerChoiceContext choiceContext, Player shuffler)
    {
        if (shuffler == Owner && Owner.Creature.CombatState != null)
            await Firebolt.CreateInHand(Owner, Owner.Creature.CombatState);
    }
}