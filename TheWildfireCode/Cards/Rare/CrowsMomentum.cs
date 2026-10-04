using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Firepower;
using TheWildfire.TheWildfireCode.Powers;

namespace TheWildfire.TheWildfireCode.Cards.Rare;

public class CrowsMomentum() : TheWildfireCard(1,
    CardType.Power, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<CrowsMomentumPower>(1), new PowerVar<CrowsMomentumUpgradePower>(1),
        new DynamicVar("Flare", 10)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.Static(WildfireKeywords.ExertAll), HoverTipFactory.FromCard<CrowsMomentum>(IsUpgraded)];
    protected override HashSet<CardTag> CanonicalTags
    {
        get => new HashSet<CardTag>() { WildfireKeywords.ExertTag };
    }
    
    protected override bool IsPlayable
    {
        get
        {
            var ownerPlayerCombatState = this.Owner.PlayerCombatState;
            if (ownerPlayerCombatState != null &&
                FirepowerController.Firepower.Get(ownerPlayerCombatState) >=
                this.DynamicVars["Flare"].IntValue)
                return true;
            if (Owner.HasPower<OverdrivePower>())
                return true;
            return false;
        }
    }

    protected override bool ShouldGlowGoldInternal => IsPlayable;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await FirepowerController.ExertAll(choiceContext, Owner, true);
        if (IsUpgraded)
            await CommonActions.ApplySelf<CrowsMomentumUpgradePower>(choiceContext, this);
        else
            await CommonActions.ApplySelf<CrowsMomentumPower>(choiceContext, this);
    }

    protected override void OnUpgrade()
    {

    }
}