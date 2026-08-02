using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Cards.Variables;
using TheWildfire.TheWildfireCode.Firepower;
using TheWildfire.TheWildfireCode.Powers;

namespace TheWildfire.TheWildfireCode.Cards.Rare;

public class CrowsMomentum() : TheWildfireCard(1,
    CardType.Power, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new FlareVar(8), new PowerVar<CrowsMomentumPower>(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<StrengthPower>()];
    protected override HashSet<CardTag> CanonicalTags
    {
        get => new HashSet<CardTag>() { WildfireKeywords.FlareTag };
    }

    protected override bool IsPlayable
    {
        get
        {
            var ownerPlayerCombatState = this.Owner.PlayerCombatState;
            return ownerPlayerCombatState != null &&
                   FirepowerController.Firepower.Get(ownerPlayerCombatState) >=
                   this.DynamicVars["Flare"].IntValue;
        }
    }

    protected override bool ShouldGlowGoldInternal => IsPlayable;
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.ApplySelf<CrowsMomentumPower>(choiceContext, this);
        await FirepowerController.Flare(choiceContext, DynamicVars["Flare"].IntValue, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Flare"].UpgradeValueBy(-4);
    }
}