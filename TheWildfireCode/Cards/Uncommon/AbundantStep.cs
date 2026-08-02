using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Firepower;

namespace TheWildfire.TheWildfireCode.Cards.Uncommon;

public class AbundantStep() : TheWildfireCard(0,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(6, ValueProp.Move)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.Static(WildfireKeywords.IgniteStatic),HoverTipFactory.Static(WildfireKeywords.FlareStatic), HoverTipFactory.Static(WildfireKeywords.Rekindle)];
    

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
    
    public override async Task AfterExert(PlayerChoiceContext choiceContext, int amount, Player exerter)
    {
        if (exerter != Owner || amount < 10 || this.Pile == null || this.Pile.Type != PileType.Exhaust)
            return;
        await CardPileCmd.Add(this, PileType.Hand);
    }

    public override async Task AfterFlare(PlayerChoiceContext choiceContext, int amount, Player flarer)
    {
        if (flarer != Owner || amount < 10 || this.Pile == null || this.Pile.Type != PileType.Exhaust)
            return;
        await CardPileCmd.Add(this, PileType.Hand);
    }
}