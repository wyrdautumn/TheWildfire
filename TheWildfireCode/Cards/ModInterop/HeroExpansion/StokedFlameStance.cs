using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Powers;

namespace TheWildfire.TheWildfireCode.Cards.ModInterop.HeroExpansion;

public class StokedFlameStance() : TheWildfireHeroExpansionCard(1,
    CardType.Skill, CardRarity.Common,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [WildfireKeywords.AttackFlow, WildfireKeywords.PowerFlow];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [..AddHeroExpansion(),HoverTipFactory.FromCard<TalonSurge>(IsUpgraded)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (CombatState == null)
            return;
        List<TalonSurge> surges = TalonSurge.Create(Owner, DynamicVars.Cards.IntValue, CombatState).ToList();
        if (IsUpgraded)
            CardCmd.Upgrade(surges, CardPreviewStyle.None);
        CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardsToCombat(surges, PileType.Draw, Owner, CardPilePosition.Random));
    }

    protected override void OnUpgrade()
    {
        
    }
}