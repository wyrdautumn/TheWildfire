using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Cards.Token;
using TheWildfire.TheWildfireCode.Cards.Variables;
using TheWildfire.TheWildfireCode.Firepower;
using TheWildfire.TheWildfireCode.Powers;

namespace TheWildfire.TheWildfireCode.Cards.Rare;

public class CrowsMomentum() : TheWildfireCard(1,
    CardType.Power, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<CrowsMomentumPower>(1), new PowerVar<CrowsMomentumUpgradePower>(1),
        new ExertVar(10)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromCard<SolarPower>(IsUpgraded)];
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
                this.DynamicVars["Exert"].IntValue)
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
        int exert = ResolveExert();
        if (IsUpgraded)
            await CommonActions.ApplySelf<CrowsMomentumUpgradePower>(choiceContext, this);
        else
            await CommonActions.ApplySelf<CrowsMomentumPower>(choiceContext, this);
        await FirepowerController.Exert(choiceContext, exert, Owner, true);
    }

    protected override void OnUpgrade()
    {

    }
}