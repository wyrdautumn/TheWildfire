using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Relics;

namespace TheWildfire.TheWildfireCode.Relics;

public class ElementalInfuser() : TheWildfireRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Uncommon;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.Static(WildfireKeywords.FullExert)];

    public override async Task AfterExert(PlayerChoiceContext choiceContext, int amount, Player exerter, bool fullExert)
    {
        if (exerter == Owner && fullExert)
            await CardPileCmd.Draw(choiceContext, Owner);
    }
}