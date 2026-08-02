using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Cards.Variables;
using TheWildfire.TheWildfireCode.Firepower;
using TheWildfire.TheWildfireCode.Powers;

namespace TheWildfire.TheWildfireCode.Cards.Rare;

public class BleedFire() : TheWildfireCard(1,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<StrengthPower>(1), new PowerVar<DexterityPower>(1), new PowerVar<ConstitutionPower>(1),
    new ExertVar(6)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<StrengthPower>(),HoverTipFactory.FromPower<DexterityPower>(),HoverTipFactory.FromPower<ConstitutionPower>(), HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];
    
    protected override bool ShouldGlowGoldInternal
    {
        get
        {
            var ownerPlayerCombatState = this.Owner.PlayerCombatState;
            return ownerPlayerCombatState != null &&
                   FirepowerController.Firepower.Get(ownerPlayerCombatState) >=
                   this.DynamicVars["Exert"].IntValue;
        }
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (Owner.PlayerCombatState == null)
            return;
        int exert = ResolveExert();
        await CommonActions.ApplySelf<StrengthPower>(choiceContext, this);
        await CommonActions.ApplySelf<DexterityPower>(choiceContext, this);
        await CommonActions.ApplySelf<ConstitutionPower>(choiceContext, this);
        await FirepowerController.Exert(choiceContext, exert, Owner);
    }

    protected override CardLocation GetResultLocationForCardPlay()
    {
        CardLocation locationForCardPlay = base.GetResultLocationForCardPlay();
        if (ResolveExert() >= DynamicVars["Exert"].IntValue)
            return locationForCardPlay;
        locationForCardPlay.pileType = PileType.Exhaust;
        return locationForCardPlay;
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Exert"].UpgradeValueBy(-2);
    }
}