using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using TheWildfire.TheWildfireCode.Cards;

namespace TheWildfire.TheWildfireCode.Powers;

public class OverdrivePower : TheWildfirePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.Static(WildfireKeywords.FullExert),HoverTipFactory.Static(WildfireKeywords.ExertStatic)];
}