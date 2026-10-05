using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Cards.Variables;
using TheWildfire.TheWildfireCode.Firepower;
using TheWildfire.TheWildfireCode.Powers;

namespace TheWildfire.TheWildfireCode.Cards.Uncommon;

public class DeepBreath() : TheWildfireCard(0,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1), new IgniteVar(10)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.Static(WildfireKeywords.ExertAll), HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];

    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        if (card == this)
        {
            await FirepowerController.Ignite(choiceContext, DynamicVars["Ignite"].IntValue, Owner);
        }
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        bool fullExert = false;
        await CommonActions.Draw(this, choiceContext);
        await FirepowerController.ExertAll(choiceContext, Owner, fullExert);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Ignite"].UpgradeValueBy(5);
    }
}