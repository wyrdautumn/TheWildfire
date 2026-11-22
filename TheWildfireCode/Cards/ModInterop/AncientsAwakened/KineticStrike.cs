using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Cards.Basic;
using TheWildfire.TheWildfireCode.Cards.Status;
using TheWildfire.TheWildfireCode.Cards.Variables;
using TheWildfire.TheWildfireCode.Powers;

namespace TheWildfire.TheWildfireCode.Cards.ModInterop.AncientsAwakened;

public class KineticStrike : TheWildfireAncientsAwakenedCard
{
    public KineticStrike() : base(1, CardType.Attack, CardRarity.Token, TargetType.AnyEnemy)
    {
    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [..AddAncientsAwakened(), GenerateAfterburnTooltip()];
    
    public override CardPoolModel VisualCardPool => AncientsAwakenedPerfectedInterop.PerfectedCardPool(Pool) ?? base.VisualCardPool;
    
    protected override HashSet<CardTag> CanonicalTags
    {
        get => new HashSet<CardTag>() { WildfireKeywords.ShowAfterburn, CardTag.Strike };
    }
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(9, ValueProp.Move), new PowerVar<AfterburnDrawPower>(1), new AfterburnVar(1)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play,vfx:"vfx/vfx_attack_slash").Execute(choiceContext);
        if (CombatState == null)
            return;
        await CommonActions.ApplySelf<AfterburnDrawPower>(choiceContext, this);
        await Afterburn.CreateAfterburn(DynamicVars["Afterburn"].IntValue, Owner, CombatState);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1);
        DynamicVars["AfterburnDrawPower"].UpgradeValueBy(1);
    }
}