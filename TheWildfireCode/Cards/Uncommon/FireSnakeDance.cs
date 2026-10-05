using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Cards.Status;
using TheWildfire.TheWildfireCode.Cards.Variables;
using TheWildfire.TheWildfireCode.Firepower;

namespace TheWildfire.TheWildfireCode.Cards.Uncommon;

public class FireSnakeDance() : TheWildfireCard(0,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2), new AfterburnVar(4)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromCard<Afterburn>(),HoverTipFactory.ForEnergy(this), HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];
    
    protected override HashSet<CardTag> CanonicalTags
    {
        get => new HashSet<CardTag>() { WildfireKeywords.ShowAfterburn };
    }
    

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.Draw(this, choiceContext);
        if (CombatState != null)
            await Afterburn.CreateAfterburn(DynamicVars["Afterburn"].IntValue, Owner, CombatState);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }
}