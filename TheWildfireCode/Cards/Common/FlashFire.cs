using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Cards.Status;
using TheWildfire.TheWildfireCode.Cards.Variables;

namespace TheWildfire.TheWildfireCode.Cards.Common;

public class FlashFire() : TheWildfireCard(1,
    CardType.Skill, CardRarity.Common,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(9, ValueProp.Move), new CardsVar(1), new AfterburnVar(2)];
    public override bool GainsBlock => true;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromCard<Afterburn>()];
    protected override HashSet<CardTag> CanonicalTags
    {
        get => new HashSet<CardTag>() { WildfireKeywords.ShowAfterburn };
    }
    

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (CombatState == null)
            return;
        await CommonActions.CardBlock(this, play);
        await CommonActions.Draw(this, choiceContext);
        await Afterburn.CreateAfterburn(DynamicVars["Afterburn"].IntValue, Owner, CombatState);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3);
    }
}