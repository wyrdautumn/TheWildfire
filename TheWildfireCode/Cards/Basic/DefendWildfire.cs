using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Cards.ModInterop;
using TheWildfire.TheWildfireCode.Cards.ModInterop.AncientsAwakened;

namespace TheWildfire.TheWildfireCode.Cards.Basic;

public class DefendWildfire : TheWildfireCard
{
    public DefendWildfire() : base(1,
        CardType.Skill, CardRarity.Basic,
        TargetType.Self)
    {
        AncientsAwakenedPerfectedInterop.AddPerfectedCardForCustomCharacters(ModelDb.Card<KineticDefend>(),this);
    }
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(5, ValueProp.Move)];
    public override bool GainsBlock => true;

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