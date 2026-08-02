using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Cards.Variables;
using TheWildfire.TheWildfireCode.Firepower;
using TheWildfire.TheWildfireCode.Powers;

namespace TheWildfire.TheWildfireCode.Cards.Uncommon;

public class SteamRoom() : TheWildfireCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new ExertVar(7), new PowerVar<ConstitutionPower>(1),
    new PowerVar<HeatExhaustionPower>(2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<HeatExhaustionPower>(),HoverTipFactory.FromPower<ConstitutionPower>()];
    
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
        await CommonActions.Apply<HeatExhaustionPower>(choiceContext, this, play);
        int exert = ResolveExert();
        if (exert >= DynamicVars["Exert"].IntValue)
            await CommonActions.ApplySelf<ConstitutionPower>(choiceContext, this);
        await FirepowerController.Exert(choiceContext, exert, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["ConstitutionPower"].UpgradeValueBy(1);
        DynamicVars["HeatExhaustionPower"].UpgradeValueBy(1);
    }
}