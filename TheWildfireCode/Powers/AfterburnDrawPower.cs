using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using TheWildfire.TheWildfireCode.Cards.Status;

namespace TheWildfire.TheWildfireCode.Powers;

public class AfterburnDrawPower : TheWildfirePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [FromAfterburnWithDraw()];
    
    private static IHoverTip FromAfterburnWithDraw()
    {
        CardModel preview = (CardModel) ModelDb.Card<Afterburn>().MutableClone();
        preview.DynamicVars.CalculationBase.UpgradeValueBy(1);
        preview.DynamicVars.RecalculateForUpgradeOrEnchant();
        preview.UpdateDynamicVarPreview(CardPreviewMode.Normal, null, preview.DynamicVars);
        return new CardHoverTip(preview);
    }
}