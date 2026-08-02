using BaseLib.Cards.Variables;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Cards.Token;
using TheWildfire.TheWildfireCode.Firepower;
using TheWildfire.TheWildfireCode.Powers;

namespace TheWildfire.TheWildfireCode.Cards.Rare;

public class BlazingScourge() : TheWildfireCard(1,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(5, ValueProp.Move),
        new CalculationBaseVar(1),
        new CalculationExtraVar(1),
        new CalculatedVar("hits").WithMultiplier(Calc),
        new DynamicVar("RagingBase", 2),
        new DynamicVar("RagingExtra", 2),
        new CustomCalculatedVar("Raging").WithMultiplier(Calc)
    ];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<RagingFirePower>()];
    
    private static decimal Calc(CardModel card, Creature? arg2)
    {
        return FirepowerController.CalculateOverheatDamageWithoutMitigation(card.Owner);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play, (int) ((CalculatedVar) DynamicVars["hits"]).Calculate(null), vfx: "vfx/vfx_attack_slash").Execute(choiceContext);
        await CommonActions.ApplySelf<RagingFirePower>(choiceContext, this, ((CalculatedVar)DynamicVars["Raging"]).Calculate(null));
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1);
        DynamicVars["RagingBase"].UpgradeValueBy(1);
        DynamicVars["RagingExtra"].UpgradeValueBy(1);
    }
}