using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using TheWildfire.TheWildfireCode.Cards;

namespace TheWildfire.TheWildfireCode.Relics;

public class BoxOfTinder : TheWildfireRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Common;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.Static(WildfireKeywords.IgniteStatic)];
}