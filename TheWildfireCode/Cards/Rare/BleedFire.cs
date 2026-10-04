using BaseLib.Extensions;
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

public class BleedFire() : TheWildfireCard(0,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    private bool _shouldExhaust = false;
    private int _currentExhaust = 0;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new ExertVar(6), new PowerVar<StrengthPower>(2), new PowerVar<ConstitutionPower>(2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<StrengthPower>(), HoverTipFactory.FromPower<ConstitutionPower>(), HoverTipFactory.FromKeyword(CardKeyword.Exhaust), HoverTipFactory.Static(WildfireKeywords.FullExert)];

    protected override HashSet<CardTag> CanonicalTags
    {
        get => new HashSet<CardTag>() { WildfireKeywords.ExertTag };
    }
    
    protected override CardLocation GetResultLocationForCardPlay()
    {
        CardLocation locationForCardPlay = base.GetResultLocationForCardPlay();
        if (_shouldExhaust)
            locationForCardPlay.pileType = PileType.Exhaust;
        _shouldExhaust = false;
        return locationForCardPlay;
    }
    
    protected override bool ShouldGlowGoldInternal
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

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        int exert = ResolveExert();
        bool fullExert;
        if (exert >= DynamicVars["Exert"].IntValue)
        {
            _shouldExhaust = false;
            fullExert = true;
        }
        else
        {
            _shouldExhaust = true;
            fullExert = false;
        }
        await FirepowerController.Exert(choiceContext, exert, Owner, fullExert);
        await CommonActions.ApplySelf<StrengthPower>(choiceContext, this);
        await CommonActions.ApplySelf<ConstitutionPower>(choiceContext, this);
        DynamicVars["Exert"].BaseValue += 1;
        _currentExhaust = DynamicVars["Exert"].IntValue;
    }
    
    protected override void AfterDowngraded()
    {
        if (_currentExhaust > DynamicVars["Exhaust"].IntValue)
            DynamicVars["Exhaust"].BaseValue = _currentExhaust;
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Strength.UpgradeValueBy(1);
        DynamicVars["ConstitutionPower"].UpgradeValueBy(1);
    }
}