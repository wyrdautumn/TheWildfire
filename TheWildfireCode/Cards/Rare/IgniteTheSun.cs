using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Firepower;

namespace TheWildfire.TheWildfireCode.Cards.Rare;

public class IgniteTheSun() : TheWildfireCard(2,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.Static(WildfireKeywords.IgniteStatic),HoverTipFactory.Static(WildfireKeywords.Scorch)];
    
    protected override HashSet<CardTag> CanonicalTags
    {
        get => new HashSet<CardTag>() { WildfireKeywords.ScorchTag };
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (Owner.PlayerCombatState == null)
            return;
        int ignite = FirepowerController.Firepower.Get(Owner.PlayerCombatState);
        await FirepowerController.Ignite(choiceContext, ignite, Owner);
        int scorch = 1;
        if (IsUpgraded)
            scorch += 1;
        await FirepowerController.Scorch(choiceContext, Owner, true, scorch);
    }

    protected override void OnUpgrade()
    {

    }
}