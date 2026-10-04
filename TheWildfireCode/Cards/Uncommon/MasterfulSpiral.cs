using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Cards.Variables;
using TheWildfire.TheWildfireCode.Firepower;
using TheWildfire.TheWildfireCode.Powers;

namespace TheWildfire.TheWildfireCode.Cards.Uncommon;

public class MasterfulSpiral() : TheWildfireCard(1,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10, ValueProp.Move), new ExertVar(12)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [WildfireKeywords.PowerFlow];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.Static(WildfireKeywords.FullExert)];
    protected override HashSet<CardTag> CanonicalTags
    {
        get => new HashSet<CardTag>() { WildfireKeywords.ExertTag };
    }

    protected override bool ShouldGlowGoldInternal
    {
        get
        {
            if (Owner.HasPower<PowerFlowPower>())
                return false;
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
        bool fullExert = false;
        await CommonActions.CardAttack(this, play,vfx:"vfx/vfx_attack_slash").Execute(choiceContext);
        if (exert >= DynamicVars["Exert"].IntValue)
            fullExert = true;
        await FirepowerController.Exert(choiceContext, exert, Owner, fullExert);
        if (exert >= DynamicVars["Exert"].IntValue && !Owner.HasPower<PowerFlowPower>())
            await FlowSingleton.AchieveFlow(choiceContext, Owner, this, CardType.Power);
    }

    public bool IsFullExert()
    {
        int exert = ResolveExert();
        if (exert >= DynamicVars["Exert"].IntValue)
            return true;
        return false;
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
    }
}