using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Cards.Basic;

namespace TheWildfire.TheWildfireCode.Cards.ModInterop.AncientsAwakened;

public class KineticDefend : TheWildfireAncientsAwakenedCard
{
    public KineticDefend() : base(0, CardType.Skill, CardRarity.Token, TargetType.Self)
    {
        
    }
    
    public override CardPoolModel VisualCardPool => AncientsAwakenedPerfectedInterop.PerfectedCardPool(Pool) ?? base.VisualCardPool;
    
    public override bool GainsBlock => true;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [..AddAncientsAwakened()];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(5, ValueProp.Move)];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [WildfireKeywords.AttackFlow];
    
    protected override HashSet<CardTag> CanonicalTags
    {
        get => new HashSet<CardTag>() { CardTag.Defend };
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3);
    }
}