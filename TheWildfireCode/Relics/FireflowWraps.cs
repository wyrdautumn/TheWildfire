using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using TheWildfire.TheWildfireCode.Cards;

namespace TheWildfire.TheWildfireCode.Relics;

public class FireflowWraps : TheWildfireRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Uncommon;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.Static(WildfireKeywords.FlareStatic)];
}