using System.Diagnostics.CodeAnalysis;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Cards.Variables;
using TheWildfire.TheWildfireCode.Firepower;

namespace TheWildfire.TheWildfireCode.Cards.ModInterop.AncientsAwakened;

public class FinalGate : TheWildfireAncientsAwakenedCard
{
    public FinalGate() : base(1,
    CardType.Skill, CardRarity.Ancient,
    TargetType.Self)
    {
        this.AddExperimentalCardForCustomCharacters(ModelDb.Character<Character.TheWildfire>());
    }
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IgniteVar(6)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [..AddAncientsAwakened(), HoverTipFactory.Static(WildfireKeywords.Scorch)];
    
    protected override HashSet<CardTag> CanonicalTags
    {
        get => new HashSet<CardTag>() { WildfireKeywords.ScorchTag };
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
        await FirepowerController.Ignite(choiceContext, DynamicVars["Ignite"].IntValue, Owner);
        await FirepowerController.Scorch(choiceContext, Owner);
    }
    
    protected override CardLocation GetResultLocationForCardPlay()
    {
        CardLocation locationForCardPlay = base.GetResultLocationForCardPlay();
        if (locationForCardPlay.pileType == PileType.Discard)
            locationForCardPlay.pileType = PileType.Hand;
        return locationForCardPlay;
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Ignite"].UpgradeValueBy(4);
    }
}