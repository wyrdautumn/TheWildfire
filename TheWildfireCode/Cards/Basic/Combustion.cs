using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Cards.Variables;
using TheWildfire.TheWildfireCode.Firepower;

namespace TheWildfire.TheWildfireCode.Cards.Basic;

public class Ignition() : TheWildfireCard(1,
    CardType.Skill, CardRarity.Basic,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new StokeVar(3)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [WildfireKeywords.Ignite];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await FirepowerController.Stoke(DynamicVars["Stoke"].IntValue, Owner);
        await FirepowerController.Ignite(choiceContext, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Stoke"].UpgradeValueBy(2);
    }
}